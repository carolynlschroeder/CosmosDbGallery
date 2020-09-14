using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CosmosDbGallery.Data;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace CosmosDbGallery.Repositories
{
    public class ImageRepository
    {
        protected string DatabaseId = "GalleryDB";
        protected string CollectionId = "GalleryCollection";
        private readonly IDocumentClient _client;
        
        public ImageRepository(IDocumentClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Image>> GetItemsAsync(Expression<Func<Image, bool>> predicate)
        {
            IDocumentQuery<Image> query = _client.CreateDocumentQuery<Image>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new FeedOptions {EnableCrossPartitionQuery = true})
                .Where(predicate)
                .AsDocumentQuery();

            List<Image> results = new List<Image>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Image>());
            }

            return results;
        }


        public async Task<Document> CreateItemAsync(Image image)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                image);
        }

        public async Task<Document> UpdateItemAsync(string id, Image image)
        {
            return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), image);
        }
    }
}
