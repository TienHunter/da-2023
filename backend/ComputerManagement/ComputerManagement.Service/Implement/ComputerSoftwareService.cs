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
    public class ComputerSoftwareService(IServiceProvider serviceProvider, IComputerSoftwareRepo computerSoftwareRepo) : BaseService<ComputerSoftwareDto, ComputerSoftware>(serviceProvider, computerSoftwareRepo), IComputerSoftwareService
    {
        private readonly IComputerSoftwareRepo _computerSoftwareRepo = computerSoftwareRepo;
        private readonly IComputerRepo _computerRepo = serviceProvider.GetService(typeof(IComputerRepo)) as IComputerRepo;
        public async Task<bool> UpsertAsync(ComputerSoftwareDto csDto)
        {
            var rs = false;
            var computerSoftwareExist = await _computerSoftwareRepo.GetQueryable().Where(cs => cs.ComputerId == csDto.ComputerId && cs.SoftwareId == csDto.SoftwareId).FirstOrDefaultAsync();
            if(computerSoftwareExist != null)
            {
                // update
                await this.BeforeUpdateAsync(computerSoftwareExist);
                rs = await _computerSoftwareRepo.UpdateAsync(computerSoftwareExist);
            }else
            {
                // insert
                // check exist computer
                _ = await _computerRepo.GetQueryable().Where(c => c.Id == csDto.ComputerId).FirstOrDefaultAsync() ?? throw new BaseException
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Code = ServiceResponseCode.NotFoundComputer
                };   
                var computerSoftware = _mapper.Map<ComputerSoftware>(csDto);
                await this.BeforeAddAsync(computerSoftware);
                rs = await _computerSoftwareRepo.AddAsync(computerSoftware);
            }
            return rs;
        }
    }
}
