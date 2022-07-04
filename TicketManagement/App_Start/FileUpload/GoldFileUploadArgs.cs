using System.IO;

#region Properties

public struct GoldFileUploadArgs
{
    private string _FileName;
    private string _ExtName;
    private string _FolderName;
    private bool _IsSystemFileName;
    private bool _IsSystemModifiledFileName;
    private Stream _StreamToUpload;

    /// <summary>
    /// Summary for GoldFileUploadArgs
    /// </summary>
    /// <param name="fileName">Default Name of the file</param>
    /// <param name="extName">Default extension of the file</param>
    /// <param name="folderName">folder name where the file will resides</param>
    /// <param name="isSystemFileName">true/false</param>
    /// <param name="isSystemModifiledFileName">true/false</param>
    /// <param name="streamToUpload">file stream object</param>
    public GoldFileUploadArgs(string fileName, string folderName, string extName, bool isSystemFileName, bool isSystemModifiledFileName, Stream streamToUpload)
    {
        _FileName = fileName;
        _ExtName = extName;
        _FolderName = folderName;
        _IsSystemFileName = isSystemFileName;
        _IsSystemModifiledFileName = isSystemModifiledFileName;
        _StreamToUpload = streamToUpload;
    }

    /// <summary>
    /// Default Name of the file
    /// </summary>
    public string FileName
    {
        get
        {
            return _FileName;
        }
    }

    /// <summary>
    /// Default extension of the file
    /// </summary>
    public string ExtName
    {
        get
        {
            return _ExtName;
        }
    }

    /// <summary>
    /// folder name where the file will resides
    /// </summary>
    public string FolderName
    {
        get
        {
            return _FolderName;
        }
    }

    /// <summary>
    /// true/false
    /// </summary>
    public bool IsSystemFileName
    {
        get
        {
            return _IsSystemFileName;
        }
    }

    /// <summary>
    /// true/false
    /// </summary>
    public bool IsSystemModifiledFileName
    {
        get
        {
            return _IsSystemModifiledFileName;
        }
    }

    /// <summary>
    /// file stream object
    /// </summary>
    public Stream StreamToUpload
    {
        get
        {
            return _StreamToUpload;
        }
    }
}

#endregion Properties