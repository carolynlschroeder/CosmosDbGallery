using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace CosmosDbGallery.BusinessLogic
{
    public class BusinessMethods
    {
        public static string GetFileExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.'));
        }

        public static byte[] FileToBytes(IFormFile file)
        {
            byte[] bytes = null;
            using var ms = new MemoryStream() ;
            file.CopyToAsync(ms).GetAwaiter();
                bytes = ms.ToArray();
                return bytes;
        }
    }
}
