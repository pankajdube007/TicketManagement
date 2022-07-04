public struct IsFileSizeAllowedArgs
{
    private long _FileSize;
    private long? _MaxFileSize;

    /// <summary>
    /// Summary for IsFileSizeAllowedArgs
    /// </summary>
    /// <param name="fileSize">default file size</param>
    /// <param name="maxFileSize">max file size business wanted to upload </param>
    public IsFileSizeAllowedArgs(long fileSize, long? maxFileSize)
    {
        _FileSize = fileSize;
        _MaxFileSize = maxFileSize;
    }

    /// <summary>
    /// default file size
    /// </summary>
    public long FileSize
    {
        get
        {
            return _FileSize;
        }
    }

    /// <summary>
    /// max file size business wanted to upload
    /// </summary>
    public long? MaxFileSize
    {
        get
        {
            return _MaxFileSize;
        }
        set
        {
            _MaxFileSize = value;
        }
    }
}