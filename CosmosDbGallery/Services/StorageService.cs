using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CosmosDbGallery.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CosmosDbGallery.Services
{
    public class StorageService
    {

        private IConfiguration _config;

        public StorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task AddToAzureStorage(byte[] fileData, string fileName)
        {
            await UploadFileToBlobAsync(fileData, fileName);
        }

        private async Task UploadFileToBlobAsync(byte[] fileData, string strFileName)
        {
            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(_config["StorageConnectionString"]);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "demo";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            var blockBlob = cloudBlobContainer.GetBlockBlobReference(strFileName);
            using (var stream = new MemoryStream(fileData, writable: false))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }

        }

        public async Task<List<BlobImageModel>> GetBlobs()
        {

            var list = new List<BlobImageModel>();

            CloudStorageAccount cloudStorageAccount =
                CloudStorageAccount.Parse(_config["StorageConnectionString"]);
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("demo");

            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> results = new List<IListBlobItem>();
            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            }
            while (continuationToken != null);

            foreach (IListBlobItem item in results)
            {
                var blobImage = new BlobImageModel();
                var b = (CloudBlockBlob)item;
                blobImage.BlobImageUri = b.Uri.ToString();
                blobImage.BlobImageName = b.Uri.Segments.Last();
                blobImage.LastModifiedDate = b.Properties.LastModified.Value.UtcDateTime;
                list.Add(blobImage);
            }

            return list;

        }
    }
}
    