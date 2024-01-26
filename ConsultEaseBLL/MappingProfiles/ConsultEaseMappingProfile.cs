using AutoMapper;
using ConsultEaseBLL.DTOs.Appointment;
using ConsultEaseBLL.DTOs.Authentication;
using ConsultEaseBLL.DTOs.CounsellingCategory;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseDAL.Entities;

namespace ConsultEaseBLL.MappingProfiles;

public class ConsultEaseMappingProfile: Profile
{
    public ConsultEaseMappingProfile()
    {

        CreateMap<UserDto, User>().ReverseMap();
        
        CreateMap<GetAppointmentDto, Appointment>().ReverseMap();
        CreateMap<CreateAppointmentDto, Appointment>().ReverseMap();
        CreateMap<UpdateApppointmentDto, Appointment>().ReverseMap();

        CreateMap<GetCounsellingCategoryDto, CounsellingCategory>().ReverseMap();
        CreateMap<UpdateCounsellingCategoryDto, CounsellingCategory>().ReverseMap();
        CreateMap<CreateCounsellingCategoryDto, CounsellingCategory>().ReverseMap();
    }
}