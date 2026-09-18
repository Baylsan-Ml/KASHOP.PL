using KASHOP.DAL.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task <Result<FileUploadResult>> UploadAsync (IFormFile file);
        Task <Result<bool>> Delete(string publicId);
    }
}
