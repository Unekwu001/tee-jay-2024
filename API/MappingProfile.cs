using AutoMapper;
using Data.Dtos;
using Data.Models;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdminUserDto, AdminUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<AttendantDto, Attendant>().ReverseMap();
        }
    }

}
