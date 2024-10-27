namespace API.Helpers;

using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

public class AutoMapperProfiles : Profile
{
       public AutoMapperProfiles()
       {
              CreateMap<AppUser, MemberResponse>()
                     .ForMember(d => d.Age,
                            o => o.MapFrom(s => s.Birth.CalculateAge()))
                     .ForMember(d => d.PhotoUrl, 
                            o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain)!.Url));
              CreateMap<Photo, PhotoResponse>();
       }
}