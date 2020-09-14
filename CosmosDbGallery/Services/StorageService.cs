using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CosmosDbGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CosmosDbGallery.Services
{
    public class StorageService
    {

        private IConfiguration _config;

        public StorageService(IConfiguration config)
        {
            _config = config;
        }

        public void AddToAzureStorage(IFormFile file, string fileName)
        {
            var _task = Task.Run(() => this.UploadFileToBlobAsync(file,fileName));
            _task.Wait();
        }

        private async Task UploadFileToBlobAsync(IFormFile file, string strFileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_config["StorageConnectionString"]);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("demo");
            BlobClient blobClient = containerClient.GetBlobClient(strFileName);
            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream, true);

        }
    }
}
    