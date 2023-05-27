using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Sales.API.Helpers
{
    public class FileStorage : IFileStorage
    {
        private readonly AzureBlobKey _azureBlobKey;

        public FileStorage(IOptions<AzureBlobKey> azureBlobKey)
        {
            _azureBlobKey = azureBlobKey.Value;
        }

        public async Task RemoveFileAsync(string path, string containerName)
        {
            
            var client = new BlobContainerClient(_azureBlobKey.AzureStore, containerName);
            await client.CreateIfNotExistsAsync();
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
        {
            var client = new BlobContainerClient(_azureBlobKey.AzureStore, containerName);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);

            using var memori = new MemoryStream(content);
            await blob.UploadAsync(memori);

            return blob.Uri.ToString();
        }
    }
}
