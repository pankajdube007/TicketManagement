#region Using

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using GoldTasks = System.Threading.Tasks;

#endregion Using

/// <summary>
/// Provides an <see cref="IFileStore"/> implementation that targets an underlying Azure Blob Storage account.
/// </summary>
/// <remarks>
/// Azure Blob Storage has very different semantics for directories compared to a local file system, and
/// some special consideration is required for make this provider conform to the semantics of the
/// <see cref="IFileStore"/> interface and behave in an expected way.
///
/// Directories have no physical manifestation in blob storage; we can obtain a reference to them, but
/// that reference can be created regardless of whether the directory exists, and it can only be used
/// as a scoping container to operate on blobs within that directory namespace.
///
/// As a consequence, this provider generally behaves as if any given directory always exists. To
/// simulate "creating" a directory (which cannot technically be done in blob storage) this provider creates
/// a marker file inside the directory, which makes the directory "exist" and appear when listing contents
/// subsequently. This marker file is ignored (excluded) when listing directory contents.
/// </remarks>
public class BlobFileStore : IFileStore
{
    #region Ctor

    public BlobFileStore(BlobStorageOptions options)
    {
        _options = options;
        _clock = DateTime.UtcNow;
        _storageAccount = CloudStorageAccount.Parse(_options.ConnectionString);
        _blobClient = _storageAccount.CreateCloudBlobClient();
        _blobContainer = _blobClient.GetContainerReference(_options.ContainerName);

        _verifyContainerTask = GoldTasks.Task.Run(async () =>
         {
             try
             {
                 await _blobContainer.CreateIfNotExistsAsync();
                 //await GoldTasks.Task.Delay(200);
                 await _blobContainer.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
                 //await GoldTasks.Task.Delay(200);
             }
             catch (Exception ex)
             {
                 throw new FileStoreException(string.Format("Error while creating or setting permissions on container {0}.", _options.ContainerName), ex);
             }
         });
        _verifyContainerTask.Wait(200);
    }

    #endregion Ctor

    #region Field Member

    private const string _directoryMarkerFileName = "gold.sample.txt";

    private readonly BlobStorageOptions _options;
    private readonly DateTime _clock;
    private readonly CloudStorageAccount _storageAccount;
    private readonly CloudBlobClient _blobClient;
    private readonly CloudBlobContainer _blobContainer;
    private readonly GoldTasks.Task _verifyContainerTask;

    #endregion Field Member

    #region Method Implementation

    public Uri BaseUri
    {
        get
        {
            var uriBuilder = new UriBuilder(_blobContainer.Uri);
            uriBuilder.Path = this.Combine(uriBuilder.Path, _options.BasePath);
            return uriBuilder.Uri;
        }
    }

    public async Task<IFileStoreEntry> GetFileInfoAsync(string path)
    {
        await _verifyContainerTask;

        var blob = GetBlobReference(path);

        if (!await blob.ExistsAsync())
            return null;

        await blob.FetchAttributesAsync();

        return new BlobFile(path, blob.Properties);
    }

    public async Task<IFileStoreEntry> GetDirectoryInfoAsync(string path)
    {
        await _verifyContainerTask;

        var blobDirectory = GetBlobDirectoryReference(path);

        return new BlobDirectory(path, _clock);
    }

    public async Task<IEnumerable<IFileStoreEntry>> GetDirectoryContentAsync(string path = "")
    {
        await _verifyContainerTask;

        var blobDirectory = GetBlobDirectoryReference(path);

        BlobContinuationToken continuationToken = null;

        var results = new List<IFileStoreEntry>();

        do
        {
            var segment =
                await blobDirectory.ListBlobsSegmentedAsync(
                    useFlatBlobListing: false,
                    blobListingDetails: BlobListingDetails.Metadata,
                    maxResults: null,
                    currentToken: continuationToken,
                    options: null,
                    operationContext: null);

            foreach (var item in segment.Results)
            {
                var itemName = WebUtility.UrlDecode(item.Uri.Segments.Last());
                var itemPath = this.Combine(path, itemName);

                if ((Type)item == typeof(CloudBlobDirectory))
                    //var directoryItem = (CloudBlobDirectory)item;
                    results.Add(new BlobDirectory(itemPath, _clock));
                else if ((Type)item == typeof(CloudBlockBlob))
                {
                    var blobItem = (CloudBlockBlob)item;
                    if (itemName != _directoryMarkerFileName)
                        results.Add(new BlobFile(itemPath, blobItem.Properties));
                }
            }
            continuationToken = segment.ContinuationToken;
        }
        while (continuationToken != null);

        return
            results
                .OrderByDescending(x => x.IsDirectory)
                .ToArray();
    }

    public async Task<bool> TryCreateDirectoryAsync(string path)
    {
        await _verifyContainerTask;

        // Since directories are only created implicitly when creating blobs, we simply pretend like
        // we created the directory, unless there is already a blob with the same path.

        var blob = GetBlobReference(path);

        var isExists = await blob.ExistsAsync();
        await GoldTasks.Task.Delay(200);
        if (isExists)
            throw new FileStoreException(string.Format("Cannot create directory because the path {0} already exists and is a file.", path));

        // Create a directory marker file to make this directory appear when listing directories.
        var placeholderBlob = GetBlobReference(this.Combine(path, _directoryMarkerFileName));
        await placeholderBlob.UploadTextAsync("This is a directory marker file created by Gold Core. It is safe to delete it.");

        return true;
    }

    public async Task<bool> TryDeleteFileAsync(string path)
    {
        await _verifyContainerTask;

        var blob = GetBlobReference(path);

        return await blob.DeleteIfExistsAsync();
    }

    public async Task<bool> TryDeleteDirectoryAsync(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new FileStoreException("Cannot delete the root directory.");

        await _verifyContainerTask;

        var blobDirectory = GetBlobDirectoryReference(path);

        BlobContinuationToken continuationToken = null;
        var blobsWereDeleted = false;

        do
        {
            var segment =
                await blobDirectory.ListBlobsSegmentedAsync(
                    useFlatBlobListing: true,
                    blobListingDetails: BlobListingDetails.None,
                    maxResults: null,
                    currentToken: continuationToken,
                    options: null,
                    operationContext: null);

            foreach (var item in segment.Results)
            {
                if ((Type)item == typeof(CloudBlob))
                {
                    var blob = (CloudBlob)item;
                    await blob.DeleteAsync();
                    blobsWereDeleted = true;
                }
            }

            continuationToken = segment.ContinuationToken;
        }
        while (continuationToken != null);

        return blobsWereDeleted;
    }

    /// <summary>
    /// Renames or moves a file to another location in the file store.
    /// </summary>
    /// <param name="oldPath">The path of the file to be renamed/moved.</param>
    /// <param name="newPath">The new path of the file after the rename/move.</param>
    /// <exception cref="FileStoreException">Thrown if the specified file does not exist or if the <paramref name="newPath"/> path already exists.</exception>
    public async GoldTasks.Task MoveFileAsync(string oldPath, string newPath)
    {
        await _verifyContainerTask;

        await CopyFileAsync(oldPath, newPath);

        await TryDeleteFileAsync(oldPath);
    }

    /// <summary>
    /// Creates a copy of a file in the file store.
    /// </summary>
    /// <param name="srcPath">The path of the source file to be copied.</param>
    /// <param name="dstPath">The path of the destination file to be created.</param>
    /// <exception cref="FileStoreException">Thrown if the specified file does not exist or if the <paramref name="dstPath"/> path already exists.</exception>
    public async GoldTasks.Task CopyFileAsync(string srcPath, string dstPath)
    {
        if (srcPath == dstPath)
            throw new ArgumentException(string.Format("The values for {0} and {1} must not be the same.", srcPath, dstPath));

        await _verifyContainerTask;

        var oldBlob = GetBlobReference(srcPath);
        var newBlob = GetBlobReference(dstPath);

        if (!await oldBlob.ExistsAsync())
            throw new FileStoreException(string.Format("Cannot copy file {0} because it does not exist.", srcPath));

        if (await newBlob.ExistsAsync())
            throw new FileStoreException(string.Format("Cannot copy file {0} because a file already exists in the new path {1}.", srcPath, dstPath));

        var operationId = await newBlob.StartCopyAsync(oldBlob);

        while (newBlob.CopyState.Status == CopyStatus.Pending)
            await GoldTasks.Task.Delay(250);

        if (newBlob.CopyState.Status != CopyStatus.Success)
            throw new FileStoreException(string.Format("Error while copying file {0}; copy operation failed with status {1} and description {2}.", srcPath, newBlob.CopyState.Status, newBlob.CopyState.StatusDescription));
    }

    public async Task<Stream> GetFileStreamAsync(string path)
    {
        await _verifyContainerTask;

        var blob = GetBlobReference(path);

        if (!await blob.ExistsAsync())
            throw new FileStoreException(string.Format("Cannot get file stream because the file {0} does not exist.", path));

        return await blob.OpenReadAsync();
    }

    public async GoldTasks.Task CreateFileFromStream(string path,
        Stream inputStream,
        string contentType,
        bool overwrite = false)
    {
        await _verifyContainerTask;
        var blob = GetBlobReference(path);

        if (!overwrite && await blob.ExistsAsync())
            throw new FileStoreException(string.Format("Cannot create file {0} because it already exists.", path));

        blob.Properties.ContentType = contentType;

        await blob.UploadFromStreamAsync(inputStream);
        await GoldTasks.Task.Delay(200);

        await blob.SetPropertiesAsync();
        await GoldTasks.Task.Delay(200);
    }

    public void UploadFileFromStream(string path,Stream inputStream,string contentType,bool overwrite = false)
    {
        _verifyContainerTask.Wait();
        var blob = GetBlobReference(path);

        if (!overwrite && blob.Exists())
            throw new FileStoreException(string.Format("Cannot create file {0} because it already exists.", path));

        blob.Properties.ContentType = contentType;
        blob.UploadFromStream(inputStream);
        blob.SetProperties();
    }

    public string UploadFileWithSasToken(string path, Stream inputStream, string contentType, string permission = "read", int keepAlive = 60, bool overwrite = false)
    {
        _verifyContainerTask.Wait();
        var blob = GetBlobReference(path);

        if (!overwrite && blob.Exists())
            throw new FileStoreException(string.Format("Cannot create file {0} because it already exists.", path));

        blob.Properties.ContentType = contentType;
        blob.UploadFromStream(inputStream);
        blob.SetProperties();

        SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();

        sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
        sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(keepAlive);

        switch (permission)
        {
            case "read":
                sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
                break;

            case "write":
                sasConstraints.Permissions = SharedAccessBlobPermissions.Write;
                break;

            case "readwrite":
                sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;
                break;

            default:
                sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
                break;
        }

        string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

        return blob.Uri + sasBlobToken;
    }

    #endregion Method Implementation

    #region Utilities

    private CloudBlockBlob GetBlobReference(string path)
    {
        var blobPath = this.Combine(_options.BasePath, path);
        var blob = _blobContainer.GetBlockBlobReference(blobPath);

        return blob;
    }

    private CloudBlobDirectory GetBlobDirectoryReference(string path)
    {
        var blobDirectoryPath = this.Combine(_options.BasePath, path);
        var blobDirectory = _blobContainer.GetDirectoryReference(blobDirectoryPath);

        return blobDirectory;
    }

    #endregion Utilities
}