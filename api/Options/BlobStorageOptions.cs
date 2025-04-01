namespace Api.Options;

public class BlobStorageOptions
{
    public const string Key = "BlobStorage";
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}