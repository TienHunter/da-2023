using AutoMapper;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Mapper
{
    public class ComputerMapper : Profile
    {
        public ComputerMapper()
        {
            CreateMap<ComputerDto, Computer>()
                .ForMember(dest => dest.ListErrorId, opt => opt.MapFrom(src => string.Join(";", src.ListErrorId)));
            CreateMap<Computer, ComputerDto>()
               .ForMember(dest => dest.ListErrorId, opt => opt.MapFrom(src => src.ListErrorId.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()));

        }
    }
}
