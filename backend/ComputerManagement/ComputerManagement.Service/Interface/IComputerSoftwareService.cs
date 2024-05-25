using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IComputerSoftwareService : IBaseService<ComputerSoftwareDto, ComputerSoftware>
    {
        /// <summary>
        /// thêm mới/ cập nhật phần mềm đã dowload về agent
        /// </summary>
        /// <param name="computerSoftwareDto"></param>
        /// <returns></returns>
        Task<bool> UpsertAsync(ComputerSoftwareDto computerSoftwareDto);
    }
}
