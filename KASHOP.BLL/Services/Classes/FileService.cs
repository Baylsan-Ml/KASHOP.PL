using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Classes
{
    public class FileService : IFileService
    {
        private readonly string[] _AllowedExtentions = { ".jpg", ".png", ".webp", ".jpeg", ".svg" };
        private const long _maxFileSize = 5 *1024 * 1024; //5MB
        private readonly Cloudinary _cloudinary;

        public FileService(IConfiguration configuration)
        {
            var Account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(Account);
        }
        public async Task<Result<FileUploadResult>> UploadAsync(IFormFile file)
        {

                if (file is null || file.Length == 0)
                {
                    return Result<FileUploadResult>.Fail("No File was Provided");
                }

                var extention = Path.GetExtension(file.FileName).ToLower();
                if (!_AllowedExtentions.Contains(extention))
                {
                    return Result<FileUploadResult>.Fail($"File type {extention} is not allowed");
                }
                if (file.Length > _maxFileSize)
                {
                    return Result<FileUploadResult>.Fail("File size exceeds the 5MB limit");
                }

               using(var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "KASHOP"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if(uploadResult.Error != null)
                {
                    return Result<FileUploadResult>.Fail(uploadResult.Error.Message);
                }

                return Result<FileUploadResult>.Ok(new FileUploadResult
                {
                    Url = uploadResult.SecureUrl.ToString(),
                    PublicId = uploadResult.PublicId
                });
                   
            }
        }

        public async Task<Result<bool>> Delete(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = _cloudinary.DestroyAsync(deletionParams);

            if(result.Result.Error != null)
            {
                return Result<bool>.Fail(result.Result.Error.Message);
            }
            return Result<bool>.Ok(true, "File deleted successfully!");
        }
    }
}
