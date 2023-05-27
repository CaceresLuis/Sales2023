namespace Sales.API.Helpers
{
    public interface IFileStorage
    {
        Task RemoveFileAsync(string path, string containerName);
        Task<string> SaveFileAsync(byte[] content, string extension, string containerName);
        async Task<string> EditFileAsync(byte[] content, string extension, string containerNamme, string path)
        {
            if(path is not null)
            {
                await RemoveFileAsync(path, containerNamme);
            }
            return await SaveFileAsync(content, extension, containerNamme);
        }
    }
}
