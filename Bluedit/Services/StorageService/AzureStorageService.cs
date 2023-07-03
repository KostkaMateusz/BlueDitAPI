﻿using Azure.Storage.Blobs;
using Microsoft.IdentityModel.Tokens;
using Azure.Identity;
using Azure.Storage.Blobs.Models;

namespace Bluedit.Services.StorageService;

public class AzureStorageService : IAzureStorageService
{
    //private string? connectionString;
    private readonly string containerName = string.Empty;
    private readonly string blobAddres = string.Empty;
    private BlobContainerClient containerClient;
    private BlobServiceClient blobServiceClient;

    public AzureStorageService(IConfiguration configuration, IWebHostEnvironment env)
    {
        // create a container client object

        blobAddres = configuration["BlobStorage:AzureBlobContainerLink"];
        containerName= configuration["BlobStorage:ContainerName"];

        //if (env.IsDevelopment())
        //{
        //    connectionString = configuration.GetSection("BlobStorage").GetValue<string>("AzureStorageConnectionString");

        //    if (connectionString.IsNullOrEmpty())
        //        throw new ArgumentNullException(nameof(connectionString));

        //    containerClient = new BlobContainerClient(connectionString, containerName);

        //    blobServiceClient = new BlobServiceClient(connectionString);
        //}
        //else
        //{
            if (string.IsNullOrEmpty(blobAddres))
                throw new ArgumentNullException(nameof(blobAddres));

            blobServiceClient = new BlobServiceClient(new Uri(blobAddres), new DefaultAzureCredential());

            containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        //}
    }

    public void CreateStorage()
    {
        //Create a container
        blobServiceClient.CreateBlobContainer(containerName);
    }

    public async Task SaveFile(Guid fileName, IFormFile file)
    {
        // Get a reference to a blob
        BlobClient blobClient = containerClient.GetBlobClient(fileName.ToString());

        //Save file Stream to blob
        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, true);        
    }

    public async Task<byte[]> GetFileData(Guid imageGuid)
    {
        var imageBlobs = containerClient.GetBlobsAsync(prefix: imageGuid.ToString());

        var listofBlobs = new List<BlobItem>();
        await foreach (var blob in imageBlobs)
        {
            listofBlobs.Add(blob);
        }
        var imageBlob = listofBlobs.FirstOrDefault();

        if (imageBlob is null)
            throw new NullReferenceException(nameof(imageBlob));
            //throw new NotFoundException($"Image with id:{imageGuid} was not found");

        var blobClient = containerClient.GetBlobClient(imageBlob.Name);

        using var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream);
        var fileContent = stream.ToArray();

        return fileContent;
    }

    public async Task DeleteImage(Guid imageGuid)
    {
        var imageBlobs = containerClient.GetBlobsAsync(prefix: imageGuid.ToString());

        await foreach (var blob in imageBlobs)
        {
            var blobClient = containerClient.GetBlobClient(blob.Name);
            await blobClient.DeleteIfExistsAsync();
        }
    }

}