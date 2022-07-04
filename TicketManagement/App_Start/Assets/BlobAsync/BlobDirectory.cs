using System;
using System.Linq;

/// <summary>
/// Summary description for BlobDirectory
/// </summary>
public class BlobDirectory : IFileStoreEntry
{
    private readonly string _path;
    private readonly DateTime _lastModifiedUtc;
    private readonly string _name;
    private readonly string _directoryPath;

    public BlobDirectory(string path, DateTime lastModifiedUtc)
    {
        _path = path;
        _lastModifiedUtc = lastModifiedUtc;
        _name = _path.Split('/').Last();
        _directoryPath = _path.Length > _name.Length ? _path.Substring(0, _path.Length - _name.Length - 1) : string.Empty;
    }

    public string Path { get { return _path; } }
    public string Name { get { return _name; } }
    public string DirectoryPath { get { return _directoryPath; } }
    public long Length { get { return 0; } }
    public DateTime LastModifiedUtc { get { return _lastModifiedUtc; } }
    public bool IsDirectory { get { return true; } }
}