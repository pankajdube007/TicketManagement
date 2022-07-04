public struct IsMimeTypeAllowedArgs
{
    private string _ExtName;
    private string[] _MineType;

    /// <summary>
    /// Summary for IsMimeTypeAllowedArgs
    /// </summary>
    /// <param name="extName">default file extension name</param>
    /// <param name="mineType">type of file user wanted to upload for this business</param>
    public IsMimeTypeAllowedArgs(string extName, string[] mineType)
    {
        _ExtName = extName;
        _MineType = mineType;
    }

    /// <summary>
    /// default file extension name
    /// </summary>
    public string ExtName
    {
        get
        {
            return _ExtName;
        }
    }

    /// <summary>
    /// type of file user wanted to upload for this business
    /// </summary>
    public string[] MineType
    {
        get
        {
            return _MineType;
        }
        set
        {
            _MineType = value;
        }
    }
}