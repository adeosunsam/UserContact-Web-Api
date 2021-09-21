using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAthentication.DTOs;

namespace UserAthentication.BusinessLogic
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly Cloudinary _cloudinary;
        private readonly ImageSettings _imageSettings;

        public ImageService(IConfiguration configuration,
            IOptions<ImageSettings> imageSettings)
        {
            _imageSettings = imageSettings.Value;
            _configuration = configuration;
            _cloudinary = new Cloudinary(new Account(_imageSettings.AccountName,
                _imageSettings.ApiKey, _imageSettings.ApiSecret));
        }

        public async Task<UploadResult> UploadImage(IFormFile image)
        {
            var pictureFormat = false;
            var imageExtension = _configuration.GetSection("PhoteSettings:Formats").Get<List<string>>();

            foreach (var item in imageExtension)
            {
                if (image.FileName.EndsWith(item))
                {
                    pictureFormat = true;
                    break;
                }
            }

            if (pictureFormat == false)
            {
                throw new NotSupportedException("Image format not supported");
            }

            var uploadResult = new ImageUploadResult();

            using(var imageStream = image.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + image.FileName;

                uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(filename, imageStream),
                    Transformation = new Transformation().Radius("max").Chain().Crop("scale").Width(200).Height(200)
                });
            }
            return uploadResult;
        }
    }
}
