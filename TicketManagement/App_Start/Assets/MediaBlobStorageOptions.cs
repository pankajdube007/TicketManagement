using System;
using System.Configuration;

/// <summary>
/// Summary description for MediaBlobStorageOptions
/// </summary>
public class MediaBlobStorageOptions : BlobStorageOptions
{
    private readonly string _connectionString;
    private readonly string _containerName;
    private readonly string _basePath;
    private readonly string _publicHostName;

    private const string connectionString = "Gold.Media.Azure.ConnectionString",
        containerName = "Gold.Media.Azure.ContainerName",
        basePath = "Gold.Media.Azure.BasePath",
        publicHostName = "Gold.Media.Azure.PublicHostName";

    public MediaBlobStorageOptions()
    {
        _connectionString = ToString(connectionString);
        _containerName = ToString(containerName);
        _basePath = ToString(basePath);
        _publicHostName = ToString(publicHostName);
    }

    public override string ConnectionString { get { return _connectionString; } }
    public override string ContainerName { get { return _containerName; } }
    public override string BasePath { get { return _basePath; } }
    public string PublicHostName { get { return _publicHostName; } }

    private string ToString(string _appSettingKey)
    {
        string _appSettingValue = string.Empty;
        try
        {
            _appSettingValue = ConfigurationManager.AppSettings[_appSettingKey];
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException(string.Format("Specified AppSetting Key {0} is not found in config file. Details: {1}", _appSettingKey, ex.Message));
        }
        return _appSettingValue;
    }
}