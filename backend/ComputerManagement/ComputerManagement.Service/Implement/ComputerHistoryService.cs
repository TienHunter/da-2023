using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Implement;
using ComputerManagerment.Repos.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class ComputerHistoryService(IServiceProvider serviceProvider, IComputerHistoryRepo computerHistoryRepo) : BaseService<ComputerHistoryDto, ComputerHistory>(serviceProvider, computerHistoryRepo), IComputerHistoryService
    {
        private readonly IComputerHistoryRepo _computerHistoryRepo = computerHistoryRepo;
        private readonly IComputerRepo _computerRepo = serviceProvider.GetService(typeof(IComputerRepo)) as IComputerRepo;
        public override async Task<Guid> AddAsync(ComputerHistoryDto computerHistoryDto)
        {
            var computerHistory = _mapper.Map<ComputerHistory>(computerHistoryDto);
            var newGuid = await this.BeforeAddAsync(computerHistory);
            computerHistory.Id = Guid.NewGuid();
            var isSaveSuccess = false;
            // validate before add
            await this.ValidateBeforeAddAsync(computerHistory);

            _ = await _computerRepo.GetAsync(computerHistory.ComputerId)
                                            ?? throw new BaseException
                                            {
                                                StatusCode = HttpStatusCode.NotFound,
                                                Code = ServiceResponseCode.NotFoundComputer
                                            };

            var isSuccess = await _computerHistoryRepo.AddAsync(computerHistory);
            if (isSuccess)
            {
                return newGuid;
            }else
            {
                throw new BaseException();
            }
        }

        public async Task<(List<ComputerHistoryDto>, int)> GetListByComputerIdAsync(Guid computerId, PagingParam pagingParam)
        {
            (List<ComputerHistory> computerHistories, int total) = await _computerHistoryRepo.GetListByComputerIdAsync(pagingParam.PageNumber, pagingParam.PageSize, computerId);
            var dtos = _mapper.Map<List<ComputerHistoryDto>>(computerHistories);

            return (dtos, total);
        }
    }
}
