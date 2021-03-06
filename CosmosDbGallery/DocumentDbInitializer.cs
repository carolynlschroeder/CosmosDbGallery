﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CosmosDbGallery
{
    public class DocumentDbInitializer
    {
        private static IDocumentClient _client;

        public static void Initialize(IDocumentClient client)
        {
            _client = client;
            CreateDatabaseIfNotExistsAsync("GalleryDB").Wait();
            CreateCollectionIfNotExistsAsync("GalleryDB", "GalleryCollection", "imageId").Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync(string databaseId)
        {

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
        }

        private static async Task CreateCollectionIfNotExistsAsync(string DatabaseId, string CollectionId, string partitionkey = null)
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseId),
                new DocumentCollection
                {
                    Id = CollectionId,
                    PartitionKey = new PartitionKeyDefinition
                    {
                        Paths = new Collection<string> { "/" + partitionkey }
                    }
                });
        }
    }
}
