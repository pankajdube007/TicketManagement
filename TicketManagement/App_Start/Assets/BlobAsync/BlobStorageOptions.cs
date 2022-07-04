public abstract class BlobStorageOptions
{
    public virtual string ConnectionString { get; set; }
    public virtual string ContainerName { get; set; }
    public virtual string BasePath { get; set; }
}