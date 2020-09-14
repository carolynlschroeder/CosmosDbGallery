using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmosDbGallery.Data;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CosmosDbGallery.Repositories
{
    public class LikeRepository
    {
        protected string DatabaseId = "GalleryDB";
        protected string CollectionId = "GalleryCollection";
        private readonly IDocumentClient _client;

        public LikeRepository(IDocumentClient client)
        {
            _client = client;
        }

        public async Task<Document> CreateItemAsync(Like like)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                like);
        }
    }

}
