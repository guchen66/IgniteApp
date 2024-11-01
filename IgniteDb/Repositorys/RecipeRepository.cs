using AutoMapper;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using IgniteShared.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.Repositorys
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AccessDbContext _context;
        private readonly IMapper _mapper;
        public RecipeRepository(AccessDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddRecipe(int id)
        {
            
        }

        public RecipeDto AddRecipe(RecipeDto dto)
        {
            RecipeInfo info = _mapper.Map<RecipeInfo>(dto);
            _context.RecipeInfos.Add(info);
            _context.SaveChanges();
            return dto;
        }

        public void EditRecipe(RecipeDto recipeDto)
        {
            // 从数据库中获取实体
            RecipeInfo recipeInfo = _context.RecipeInfos.Find(recipeDto.Id);

            if (recipeInfo != null)
            {
                // 使用 AutoMapper 将 DTO 映射到实体
                _mapper.Map(recipeDto, recipeInfo);

                // 手动设置实体状态为 Modified
                _context.Entry(recipeInfo).State = EntityState.Modified;

                // 保存更改到数据库
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"RecipeInfo with ID {recipeDto.Id} not found.");
            }

        }

        public RecipeDto GetRecipeById(int id)
        {
            var info= _context.RecipeInfos.Find(id);
            RecipeDto recipeDto=_mapper.Map<RecipeDto>(info);
            return recipeDto;
        }

        public List<RecipeDto> GetRecipes()
        {
            var recipeInfos = _context.RecipeInfos.ToList();
            List<RecipeDto> recipeDtos = _mapper.Map<List<RecipeDto>>(recipeInfos);
            return recipeDtos;
        }

        public void DeleteRecipe(RecipeDto dto)
        {
            RecipeInfo info = _context.RecipeInfos.Find(dto.Id);        //这里需要使用Find方法来跟踪EF6的数据库数据，Entity Framework 的上下文被（DbContext）跟踪
            _context.RecipeInfos.Remove(info);
            _context.SaveChanges();
        }
    }
}
