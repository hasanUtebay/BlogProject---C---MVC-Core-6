using AutoMapper;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.WEB.Areas.Member.Models.DTOs;

namespace BlogProject.WEB.Areas.Member.Models.Mappers
{
    public class Mapping : Profile 
    {
      
        public Mapping()
        {           
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();        
           
            CreateMap<Article, CreateArticleDTO>().ReverseMap();
            CreateMap<UpdateArticleDTO,Article >().ReverseMap();

            CreateMap<UpdateAppUserDTO, AppUser>().ReverseMap();


        }
    }
}
