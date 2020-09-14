using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbGallery.Models
{
    public class BlobImageModel
    {
        public string BlobImageUri { get; set; }
        public string BlobImageName { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
