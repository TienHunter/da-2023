using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IComputerHistoryService : IBaseService<ComputerHistoryDto, ComputerHistory>
    {
        Task<(List<ComputerHistoryDto>, int)> GetListByComputerIdAsync(Guid computerId,PagingParam pagingParam);
    }
}
