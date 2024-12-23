using AutoMapper;
using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;

namespace boilerplate_app.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ReverseMap();

            CreateMap<User, RegisterDto>()
                .ReverseMap();
            //.ForMember(dest => dest.Id,opt => opt.Ignore());
        }
    }
}
