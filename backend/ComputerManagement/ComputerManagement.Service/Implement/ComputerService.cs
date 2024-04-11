using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Lib.Interface;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class ComputerService(IServiceProvider serviceProvider, IComputerRepo computerRepo) : BaseService<ComputerDto, Computer>(serviceProvider, computerRepo), IComputerService
    {

        private readonly IComputerRepo _computerRepo = computerRepo;

        public async Task<ComputerDto> GetComputerByMacAddress(string macAddress)
        {
            var computer = await _computerRepo.GetQueryable().Where(c => c.MacAddress == macAddress).FirstOrDefaultAsync() ?? throw new BaseException
            {
                StatusCode = HttpStatusCode.NotFound,
                Code = ServiceResponseCode.NotFoundComputer,
            };

            return _mapper.Map<ComputerDto>(computer);
        }

        public async Task<bool> UpdateStateByMacAddressAsync(string macAddress)
        {
            var computerExist = await _computerRepo.GetQueryable().Where(c => c.MacAddress == macAddress).FirstOrDefaultAsync();
            base.CheckNullModel(computerExist);
            computerExist.State = ComputerState.On;
            computerExist.StateTime = DateTime.Now;

            return await _computerRepo.UpdateAsync(computerExist);

            // có thể bắn emit lên client để cập nhật lại state computer
        }
    }
}
