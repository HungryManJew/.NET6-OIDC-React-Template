using AutoMapper;
using OIDC.WebApp.DTOs;
using System.Security.Claims;

namespace OIDC.WebApp.Profiles
{
    public class AuthProfiles : Profile
    {
        public AuthProfiles()
        {
            CreateMap<Claim, UserClaimsDto>()
                .ForMember(dest => dest.Issuer, options => options.MapFrom(src => src.Issuer))
                .ForMember(dest => dest.OriginalIssuer, options => options.MapFrom(src => src.OriginalIssuer))
                .ForMember(dest => dest.Type, options => options.MapFrom(src => src.Type))
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Value))
                .ForMember(dest => dest.ValueType, options => options.MapFrom(src => src.ValueType));
        }
    }
}
