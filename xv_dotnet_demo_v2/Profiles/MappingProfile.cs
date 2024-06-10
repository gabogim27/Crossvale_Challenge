using AutoMapper;
using xv_dotnet_demo.Dtos;
using xv_dotnet_demo_v2_domain.Authorization;
using xv_dotnet_demo_v2_domain.Entities;

namespace xv_dotnet_demo.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Token, TokenDto>().ReverseMap();
            CreateMap<IssuerData, IssuerDataDto>().ReverseMap();
            CreateMap<Names, NameDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
        }
    }
}
