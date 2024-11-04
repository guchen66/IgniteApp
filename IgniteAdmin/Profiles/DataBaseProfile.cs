using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
namespace IgniteAdmin.Profiles
{
    public class DataBaseProfile: Profile
    {
        public DataBaseProfile()
        {
            //配方DTO和实体类转换
            CreateMap<RecipeInfo, RecipeDto>();
            CreateMap<RecipeDto, RecipeInfo>();
            CreateMap<ProductDto, ProductInfo>();
            CreateMap<ProductInfo, ProductDto>();
            CreateMap<MaterialDto, MaterialInfo>();
            CreateMap<MaterialInfo, MaterialDto>();
        }
    }
}
