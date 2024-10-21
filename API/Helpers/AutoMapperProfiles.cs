namespace API.Helpers;

using API.DTOs;
using API.Entities;
using AutoMapper;

public class AutoMapperProfiles : Profile
{
       public AutoMapperProfiles()
       {
              CreateMap<AppUser, MemberResponse>();
              CreateMap<Photo, PhotoResponse>();
       }
}
