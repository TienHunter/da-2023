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
        private readonly IComputerRoomRepo _computerRoomRepo = serviceProvider.GetService(typeof(IComputerRoomRepo)) as IComputerRoomRepo;

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

        public override async Task<Guid> AddAsync(ComputerDto computerDto)
        {
            var computer = _mapper.Map<Computer>(computerDto);
            var newId = await this.BeforeAddAsync(computer);
            var isSaveSuccess = false;
            // validate before add
            await this.ValidateBeforeAddAsync(computer);

            var computerRoom = await _computerRoomRepo.GetAsync(computer.ComputerRoomId)
                                            ?? throw new BaseException
                                            {
                                                StatusCode = HttpStatusCode.NotFound,
                                                Code = ServiceResponseCode.NotFoundComputerRoom
                                            };
            if (computerRoom.CurrentCapacity + 1 > computerRoom.MaxCapacity)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.MaxCapacityComputerRoom
                };
            }
            computerRoom.CurrentCapacity += 1;


            try
            {
                await _uow.BeginTransactionAsync();
                isSaveSuccess = await _computerRepo.AddAsync(computer);
                await _computerRoomRepo.UpdateAsync(computerRoom);
                await _uow.CommitAsync();
                if (isSaveSuccess)
                {
                    await base.AfterSaveAsync(computer);
                }
            }
            catch
            {
                await _uow.RollbackAsync();
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Code = ServiceResponseCode.Error
                };
            }


            return newId;

        }

        public override async Task ValidateBeforeAddAsync(Computer computer)
        {
            await base.ValidateBeforeAddAsync(computer);
            // check conflic mac address
            var computerByMac = await _computerRepo.GetQueryable().Where(c => c.MacAddress == computer.MacAddress).FirstOrDefaultAsync();
            if (computerByMac != null)
            {
                throw new BaseException { StatusCode = HttpStatusCode.Conflict, Code = ServiceResponseCode.ConflicMacAddress };
            }
            var computerByNameAndComputerRoomID = await _computerRepo.GetQueryable().Where(c => c.Name == computer.Name && c.ComputerRoomId == computer.ComputerRoomId).FirstOrDefaultAsync();
            if (computerByNameAndComputerRoomID != null)
            {
                throw new BaseException { StatusCode = HttpStatusCode.Conflict, Code = ServiceResponseCode.ConflicNameComputer };
            }
            // check postion computer
            var computerByRowCol = await _computerRepo.GetQueryable().Where(c => c.Row == computer.Row && c.Col == computer.Col).FirstOrDefaultAsync();
            if (computerByRowCol != null)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.ConflicRowColComputer
                };
            }
        }

        public override async Task<(List<ComputerDto>, int)> GetListAsync(PagingParam pagingParam)
        {
            var (dtos, totalCount) = await base.GetListAsync(pagingParam);
            foreach (var item in dtos)
            {
                if (item.StateTime < DateTime.Now.AddMinutes(-3))
                {
                    item.State = ComputerState.Off;
                }
            }
            return (dtos, totalCount);
        }
        public async Task<List<ComputerDto>> GetListComputerByComputerRoomIdAsync(Guid computerRoomId, PagingParam pagingParam)
        {
            var computers = await _computerRepo.GetListComputerByComputerRoomIdAsync(computerRoomId, pagingParam.KeySearch, pagingParam.FieldSort, pagingParam.SortAsc);
            foreach (var item in computers)
            {
                if(item.StateTime < DateTime.Now.AddMinutes(-3))
                {
                    item.State = ComputerState.Off;
                }
            }
            return _mapper.Map<List<ComputerDto>>(computers);
        }

        public override async Task<Guid> BeforeAddAsync(Computer computer)
        {
            var newGuid = await base.BeforeAddAsync(computer);
            computer.State = ComputerState.Off;
            computer.StateTime = DateTime.Now;
            return newGuid;
        }

        public async Task<bool> UpdateComputerConfigAsync(ComputerConfig computerConfig, Guid computerId)
        {
            var computerExist = await _computerRepo.GetAsync(computerId) ?? throw new BaseException
            {
                StatusCode = HttpStatusCode.NotFound,
                Code = ServiceResponseCode.NotFoundComputer,
            };

            computerExist.OS = computerConfig.OS;
            computerExist.CPU = computerConfig.CPU;
            computerExist.RAM = computerConfig.RAM;
            computerExist.HardDriver = computerConfig.HardDriver;
            computerExist.HardDriverUsed = computerConfig.HardDriverUsed;

            var rs = await _computerRepo.UpdateAsync(computerExist);
            return rs;
        }
    }
}
