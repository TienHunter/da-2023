using ComputerManagement.BO.DTO;
using ComputerManagement.BO.DTO.MonitorSession;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class MonitorSessionService(IServiceProvider serviceProvider, IMonitorSessionRepo monitorSessionRepo) : BaseService<MonitorSessionDto, MonitorSession>(serviceProvider, monitorSessionRepo), IMonitorSessionService
    {
        private readonly IMonitorSessionRepo _monitorSessionRepo = monitorSessionRepo;
        private readonly IComputerRoomRepo _computerRoomRepo = serviceProvider.GetService(typeof(IComputerRoomRepo))  as IComputerRoomRepo;

        public async Task<(List<MonitorSessionDto>, int)> GetListComputerByComputerRoomIdAsync(Guid computerRoomId, PagingParam pagingParam)
        {
            var (entities, totalCount) = await _monitorSessionRepo.GetListComputerByComputerRoomIdAsync(computerRoomId, pagingParam.KeySearch, pagingParam.PageNumber, pagingParam.PageSize, pagingParam.FieldSort, pagingParam.SortAsc);
            var dtos = _mapper.Map<List<MonitorSessionDto>>(entities);
            return (dtos, totalCount);
        }

        public override async Task ValidateBeforeAddAsync(MonitorSession monitorSession)
        {
            await base.ValidateBeforeAddAsync(monitorSession);
            // check trùng phiên
            var monitorSessionConflicting = await _monitorSessionRepo.GetQueryable().Where(m =>
            (m.StartDate < monitorSession.EndDate && m.EndDate > monitorSession.StartDate) || // Kiểm tra trường hợp chồng chéo
            (m.StartDate >= monitorSession.StartDate && m.EndDate <= monitorSession.EndDate) || // Kiểm tra trường hợp một phiên nằm trong phiên khác
            (m.StartDate <= monitorSession.StartDate && m.EndDate >= monitorSession.EndDate)) // Kiểm tra trường hợp một phiên chứa phiên khác
            .FirstOrDefaultAsync();

            if(monitorSessionConflicting != null)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.ConflicMonitorSessionTime
                };
            };
            _ = await _computerRoomRepo.GetAsync(monitorSession.ComputerRoomId) ?? throw new BaseException { StatusCode = HttpStatusCode.NotFound, Code = ServiceResponseCode.NotFoundComputerRoom };
        }

        public override async Task HandleDataBeforeMapAddAsync(MonitorSessionDto dto)
        {
            await base.HandleDataBeforeMapAddAsync(dto);
            dto.Domains = dto.Domains.Where(domain => !string.IsNullOrEmpty(domain)).ToList();
        }
    }
}
