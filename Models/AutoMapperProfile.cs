using AutoMapper;
using CarRentalApi.Entities;

namespace CarRentalApi.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Camp , CampsModel>()
                .ForMember(c => c.StartDate , opt => opt.MapFrom(c => c.EventDate))
                .ForMember(c => c.EndDate , opt => opt.MapFrom(c => c.EventDate))
                .ReverseMap();

            CreateMap<Employee , EmployeeModel>()
                .ForMember(c => c.FullName , opt => opt.MapFrom(c => c.FirstName))
                .ReverseMap();
        }
    }
}
