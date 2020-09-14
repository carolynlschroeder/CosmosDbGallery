using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosDbGallery.Data
{
    public class Image
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("imageName")]
        public string ImageName { get; set; }
        [JsonProperty("like")]
        public int Likes { get; set; }
        [JsonProperty("docType")]
        public string DocType { get; set; }
        [JsonProperty("imageId")]
        public string ImageId { get; set; }

    }
}
