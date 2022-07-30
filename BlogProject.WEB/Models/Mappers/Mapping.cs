using AutoMapper;
using BlogProject.Models.Entities.Concrrete;
using BlogProject.WEB.Models.DTOs;

namespace BlogProject.WEB.Models.Mappers
{
    public class Mapping : Profile 
    {
        public Mapping()
        {
            CreateMap<AppUser, CreateUserDTO>().ReverseMap(); 
        }
    }
}
