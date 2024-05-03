using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class SoftwareService(IServiceProvider serviceProvider, ISoftwareRepo softwareRepo) : BaseService<SoftwareDto, SoftwareModel>(serviceProvider, softwareRepo), ISoftwareService
    {
        private readonly ISoftwareRepo _softwareRepo = softwareRepo;
    }
}
