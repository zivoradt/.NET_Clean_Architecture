using AutoMapper;
using MVC.Models;
using MVC.Services;
using MVC.Services.Base;
using System.ComponentModel;

namespace MVC.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveTypeDto, CreateLeaveTypeVM>().ReverseMap();
            CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<RegisterVM, RegistrationRequest>().ReverseMap();
            CreateMap<CreateLeaveRequestDto, CreateLeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequestDto, LeaveRequestVM>()

                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
            CreateMap<LeaveRequestListDto, LeaveRequestVM>()

                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
            CreateMap<Employee, EmployeeVM>().ReverseMap();
            CreateMap<LeaveAllocationDto, LeaveAllocationVM>().ReverseMap();
        }
    }

    public class DateToStringConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }
    }
}