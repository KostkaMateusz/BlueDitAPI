using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Bluedit.Infrastructure.StorageService;

public class AzureStorageService : IAzureStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;
    private readonly string _containerName;

    public AzureStorageService(IConfiguration configuration, IWebHostEnvironment env)
    {
        // create a container client object

        _containerName = configuration["BlobStorage:ContainerName"] ??
                         throw new ArgumentNullException(nameof(configuration));

        if (string.Equals(env.EnvironmentName, "Development"))
        {
            var connectionString =
                configuration.GetSection("BlobStorage").GetValue<string>("AzureStorageConnectionString");

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _containerClient = new BlobContainerClient(connectionString, _containerName);

            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        else
        {
            var blobAddres = configuration["BlobStorage:AzureBlobContainerLink"] ??
                             throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrEmpty(blobAddres))
                throw new ArgumentNullException(nameof(blobAddres));

            _blobServiceClient = new BlobServiceClient(new Uri(blobAddres), new DefaultAzureCredential());

            _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        }
    }

    public void CreateStorage()
    {
        //Create a container
        _blobServiceClient.CreateBlobContainer(_containerName);
    }

    public async Task SaveFile(Guid fileName, IFormFile file)
    {
        // Get a reference to a blob
        var blobClient = _containerClient.GetBlobClient(fileName.ToString());

        //Save file Stream to blob
        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, true);
    }

    public async Task<byte[]> GetFileData(Guid imageGuid)
    {
        var imageBlobs = _containerClient.GetBlobsAsync(prefix: imageGuid.ToString());

        var listofBlobs = new List<BlobItem>();
        await foreach (var blob in imageBlobs) listofBlobs.Add(blob);
        var imageBlob = listofBlobs.FirstOrDefault();

        if (imageBlob is null)
            throw new NullReferenceException(nameof(imageBlob));
        //throw new NotFoundException($"Image with id:{imageGuid} was not found");

        var blobClient = _containerClient.GetBlobClient(imageBlob.Name);

        using var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream);
        var fileContent = stream.ToArray();

        return fileContent;
    }

    public async Task DeleteImage(Guid imageGuid)
    {
        var imageBlobs = _containerClient.GetBlobsAsync(prefix: imageGuid.ToString());

        await foreach (var blob in imageBlobs)
        {
            var blobClient = _containerClient.GetBlobClient(blob.Name);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}