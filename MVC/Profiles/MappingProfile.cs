using AutoMapper;
using MVC.Models;
using MVC.Services;
using MVC.Services.Base;

namespace MVC.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveTypeDto, CreateLeaveTypeVM>().ReverseMap();
            CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<RegisterVM, RegistrationRequest>().ReverseMap();
        }
    }
}