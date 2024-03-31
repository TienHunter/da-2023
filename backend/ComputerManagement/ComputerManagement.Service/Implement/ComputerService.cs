using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Lib.Interface;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class ComputerService : BaseService<ComputerDto,Computer>, IComputerService
    {

        private readonly IComputerRepo _computerRepo;
        public ComputerService(IServiceProvider serviceProvider, IComputerRepo computerRepo) : base(serviceProvider, computerRepo)
        {
            _computerRepo = computerRepo;
        }
    }
}
