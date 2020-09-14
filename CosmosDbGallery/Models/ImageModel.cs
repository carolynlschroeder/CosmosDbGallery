using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbGallery.Models
{
    public class ImageModel
    {
        public Guid ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageUri { get; set; }
        public int TotalLikes { get; set; }
    }
}
