using System.Linq;

/// <summary>
/// Summary description for FileUploadValidation
/// </summary>
public static class FileUploadValidation
{
    private static readonly long MaxSystemFileSize = 1024 * 1024 * 1024;
    private static readonly string[] SystemMimeType = { ".pdf", ".csv", ".xlsx", ".xls", ".docx", ".doc", ".jpeg", ".jpg", ".gif", ".png" };

    /// <summary>
    /// This type of mimetype allowed or not
    /// </summary>
    /// <param name="extName">default file extension name</param>
    /// <param name="mineType">type of file user wanted to upload for this business</param>
    /// <returns>true/false</returns>
    public static bool IsMimeTypeAllowed(IsMimeTypeAllowedArgs isMimeTypeAllowedArgs)
    {
        if (string.IsNullOrWhiteSpace(isMimeTypeAllowedArgs.ExtName))
            return false;

        if (isMimeTypeAllowedArgs.MineType.Count() <= 0)
            return false;

        return (isMimeTypeAllowedArgs.MineType.Any(SystemMimeType.Contains) && isMimeTypeAllowedArgs.MineType.Contains(isMimeTypeAllowedArgs.ExtName.ToLower()));
    }

    /// <summary>
    /// This file size is allowed or not
    /// </summary>
    /// <param name="maxFileSize">max file size business wanted to upload </param>
    /// <param name="fileSize">default file size</param>
    /// <returns>true/false</returns>
    public static bool IsFileSizeAllowed(IsFileSizeAllowedArgs isFileSizeAllowedArgs, out long size)
    {
        long userSize = isFileSizeAllowedArgs.FileSize;
        if (isFileSizeAllowedArgs.MaxFileSize.HasValue)
            long.TryParse(isFileSizeAllowedArgs.MaxFileSize.ToString(), out userSize);

        size = userSize < MaxSystemFileSize ? userSize : MaxSystemFileSize;

        return isFileSizeAllowedArgs.FileSize <= isFileSizeAllowedArgs.MaxFileSize && isFileSizeAllowedArgs.FileSize <= MaxSystemFileSize;
    }
}