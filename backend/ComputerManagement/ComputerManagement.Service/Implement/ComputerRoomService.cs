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
    public class ComputerRoomService : BaseService<ComputerRoomDto, ComputerRoom>, IComputerRoomService
    {
        private readonly IComputerRoomRepo _computerRoomRepo;
        public ComputerRoomService(IServiceProvider serviceProvider, IComputerRoomRepo computerRoomRepo) : base(serviceProvider, computerRoomRepo)
        {
            _computerRoomRepo = computerRoomRepo;
        }
    }
}
