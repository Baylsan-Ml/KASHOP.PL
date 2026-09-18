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
        public async Task<Result<string>> UploadAsync(IFormFile file)
        {

                if (file is null || file.Length == 0)
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = "No File was Provided",
                    };
                }
                var extention = Path.GetExtension(file.FileName).ToLower();
                if (!_AllowedExtentions.Contains(extention))
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = $"File type {extention} is not allowed",
                    };
                }
                if (file.Length > _maxFileSize)
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = "File size exeeds the 5MB limit",
                    };
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
                    return new Result<string>
                    {
                        Success = false,
                        Message = uploadResult.Error.Message,
                    };
                }
                    return new Result<string>
                    {
                        Success = true,
                        Message = "File uploaded successfully",
                        Data = uploadResult.SecureUrl.ToString()
                    };
            }

            // Ensure the Images directory exists
            var fileName = Guid.NewGuid().ToString() + extention;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Images", file.FileName);
                // Create the Images directory if it doesn't exist
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return new Result<string>
                {
                    Success = true,
                    Message = "Success",
                    Data = fileName
                };
        }
    }
}
