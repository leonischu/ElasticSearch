using AutoMapper;
using JobPortal.Application.DTO;
using JobPortal.Domain.Models;

namespace JobPortal.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<Job, JobDto>();
        
        }
    }
}
