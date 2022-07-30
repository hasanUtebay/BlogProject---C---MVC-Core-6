using AutoMapper;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.WEB.Areas.Admin.Models.DTOs;
using BlogProject.WEB.Areas.Member.Models.DTOs;

namespace BlogProject.WEB.Areas.Admin.Models.Mappers
{
    public class Mapping : Profile 
    {
      
        public Mapping()
        {
              
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

        }
    }
}
