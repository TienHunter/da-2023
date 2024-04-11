using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public override async Task ValidateBeforeAddAsync(ComputerRoom computerRoom)
        {
            await base.ValidateBeforeAddAsync(computerRoom);
            var computerRoomByName = await _computerRoomRepo.GetQueryable().Where(c => c.Name == computerRoom.Name).FirstOrDefaultAsync();
            if(computerRoomByName != null)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.ComputerRoomNameConflic
                };
            }
        }
    }
}
