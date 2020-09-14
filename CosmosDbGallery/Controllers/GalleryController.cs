using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CosmosDbGallery.BusinessLogic;
using CosmosDbGallery.Data;
using CosmosDbGallery.Models;
using CosmosDbGallery.Repositories;
using CosmosDbGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CosmosDbGallery.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ImageRepository _imageRepository;
        private readonly LikeRepository _likeRepository;
        private readonly IConfiguration _config;
        private readonly StorageService _storageService;

        public GalleryController(ImageRepository imageRepository, LikeRepository likeRepository, StorageService storageService ,IConfiguration config)
        {
            _imageRepository = imageRepository;
            _likeRepository = likeRepository;
            _storageService = storageService;
            _config = config;
        }
        
        public async Task<ActionResult> Index()
        {
            var model = new List<ImageModel>();
            var images = await _imageRepository.GetItemsAsync(x => x.DocType == "Image");
            var imageBaseUrl = _config["ImageBaseUrl"];
            model = images.Select(i => new ImageModel
            {
                ImageId = new Guid(i.Id),
                ImageName = i.ImageName,
                ImageUri = String.Concat(imageBaseUrl,i.ImageName),
                TotalLikes = i.Likes

            }).ToList();


            return View(model);
        }

        public JsonResult AddLike(Guid imageId)
        {
            var like = new Like
            {
                Id = Guid.NewGuid().ToString(),
                ImageId = imageId.ToString(), 
                DocType = "Like"
            };
            _likeRepository.CreateItemAsync(like).GetAwaiter();
            var images = _imageRepository.GetItemsAsync(x => x.DocType == "Image" && x.ImageId == imageId.ToString())
                .GetAwaiter().GetResult();
            var image = images.FirstOrDefault();
            image.Likes++;
            _imageRepository.UpdateItemAsync(image.Id, image).GetAwaiter();
            return Json(new {Message = string.Empty});
        }

        public ActionResult AddImage()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddImage(List<IFormFile> files)
        {
            var file = files.First();
            var name = Guid.NewGuid() + BusinessLogic.BusinessMethods.GetFileExtension(file.FileName);
            var fileData = BusinessMethods.FileToBytes(file);
            await _storageService.AddToAzureStorage(fileData, name);

            var id = Guid.NewGuid().ToString();

            var image = new Image
            {
                Id = id,
                ImageId = id,
                ImageName = name,
                DocType = "Image",
                Likes = 0
            };

            await _imageRepository.CreateItemAsync(image);
            return RedirectToAction("Index", "Gallery");
        }

    }
}
