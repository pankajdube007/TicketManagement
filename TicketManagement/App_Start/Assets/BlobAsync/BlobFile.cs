using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;

/// <summary>
/// Summary description for BlobFile
/// </summary>
public class BlobFile : IFileStoreEntry
{
    private readonly string _path;
    private readonly BlobProperties _blobProperties;
    private readonly string _name;
    private readonly string _directoryPath;

    public BlobFile(string path, BlobProperties blobProperties)
    {
        _path = path;
        _blobProperties = blobProperties;
        _name = _path.Split('/').Last();
        _directoryPath = _path.Substring(0, _path.Length - _name.Length - 1);
    }

    public string Path { get { return _path; } }

    public string Name { get { return _name; } }

    public string DirectoryPath { get { return _directoryPath; } }

    public long Length { get { return _blobProperties.Length; } }

    public DateTime LastModifiedUtc { get { return _blobProperties.LastModified.GetValueOrDefault().UtcDateTime; } }

    public bool IsDirectory { get { return false; } }
}