using AutoMapper;

namespace Neubel.Wow.Win.Authentication.WebAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DTO.LoginRequest, Core.Model.LoginRequest>();
            CreateMap<DTO.UserToken, Core.Model.LoginToken>().ReverseMap();
            CreateMap<DTO.User, Core.Model.User>().ReverseMap();
            CreateMap<DTO.Organization, Core.Model.Organization>().ReverseMap();
            CreateMap<DTO.Role, Core.Model.Role>().ReverseMap();
            CreateMap<DTO.ChangedPassword, Core.Model.ChangedPassword>();
            CreateMap<DTO.SecurityParameter, Core.Model.SecurityParameter>().ReverseMap();
        }
    }
}
