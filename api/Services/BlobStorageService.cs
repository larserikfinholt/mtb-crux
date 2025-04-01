using Azure.Storage.Blobs;
using Api.Options;
using Microsoft.Extensions.Options;

namespace Api.Services;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string blobName);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(IOptions<BlobStorageOptions> options)
    {
        var blobClient = new BlobServiceClient(options.Value.ConnectionString);
        _containerClient = blobClient.GetBlobContainerClient(options.Value.ContainerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var blobClient = _containerClient.GetBlobClient(blobName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, true);

        return blobClient.Uri.ToString();
    }

    public async Task DeleteImageAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(new Uri(blobName).Segments.Last());
        await blobClient.DeleteIfExistsAsync();
    }
}