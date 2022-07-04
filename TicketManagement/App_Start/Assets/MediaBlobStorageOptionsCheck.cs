/// <summary>
/// Summary description for MediaBlobStorageOptionsCheck
/// </summary>
public static class MediaBlobStorageOptionsCheck
{
    public static bool CheckOptions(string connectionString, string containerName)
    {
        var optionsAreValid = true;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            optionsAreValid = false;
        }

        if (string.IsNullOrWhiteSpace(containerName))
        {
            optionsAreValid = false;
        }

        return optionsAreValid;
    }
}