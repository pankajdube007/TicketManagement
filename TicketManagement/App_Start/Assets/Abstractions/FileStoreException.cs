using System;

/// <summary>
/// Summary description for FileStoreException
/// </summary>
public class FileStoreException : Exception
{
    public FileStoreException(string message) : base(message)
    {
    }

    public FileStoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}