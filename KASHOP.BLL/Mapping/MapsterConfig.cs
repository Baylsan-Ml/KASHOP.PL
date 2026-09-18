using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Mapping
{
    public  class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.User, src => src.CreatedBy.UserName)
                .Map(dest=> dest.Name, src => src.Translations.Where(t => t.Language == CultureInfo.CurrentUICulture.Name)
                .Select(t=>t.Name).FirstOrDefault());

            //Product Name and Description mapping based on the current UI culture
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.Name, src => src.Translations.Where(t => t.Language == CultureInfo.CurrentUICulture.Name)
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.Description, src => src.Translations.Where(t => t.Language == CultureInfo.CurrentUICulture.Name)
                .Select(t => t.Description).FirstOrDefault());
        }
    }
}
