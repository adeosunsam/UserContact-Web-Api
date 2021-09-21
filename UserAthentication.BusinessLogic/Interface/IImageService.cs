using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UserAthemtication.DTOs;

namespace UserAthentication.BusinessLogic
{
    public interface IImageService
    {
        Task<UploadResult> UploadImage(IFormFile image);
    }
} 