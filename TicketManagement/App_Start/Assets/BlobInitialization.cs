using System;

/// <summary>
/// Summary description for AzureInitilization
/// </summary>
public sealed class BlobInitialization
{
    private static readonly Lazy<BlobInitialization> lazy =
        new Lazy<BlobInitialization>(() => new BlobInitialization());

    public static BlobInitialization Instance { get { return lazy.Value; } }

    private readonly MediaBlobStorageOptions _mediaBlobStorageOptions;
    private readonly IMediaFileStore _mediaFileStore;

    private BlobInitialization()
    {
        _mediaBlobStorageOptions = new MediaBlobStorageOptions();

        var connectionString = _mediaBlobStorageOptions.ConnectionString;
        var containerName = _mediaBlobStorageOptions.ContainerName;

        if (MediaBlobStorageOptionsCheck.CheckOptions(connectionString, containerName))
        {
            var fileStore = new BlobFileStore(_mediaBlobStorageOptions);
            var mediaBaseUri = fileStore.BaseUri;
            _mediaFileStore = new MediaFileStore(fileStore, mediaBaseUri.ToString());

            if (!string.IsNullOrEmpty(_mediaBlobStorageOptions.PublicHostName))
                mediaBaseUri = new UriBuilder(mediaBaseUri) { Host = _mediaBlobStorageOptions.PublicHostName }.Uri;

            _mediaFileStore = new MediaFileStore(fileStore, mediaBaseUri.ToString());
        }
    }

    public IMediaFileStore GetMediaFileStore()
    {
        return _mediaFileStore;
    }
}