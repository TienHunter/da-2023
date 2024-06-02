using ComputerManagement.BO.DTO.ConfigOption;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IConfigOptionService : IBaseService<ConfigOptionDto, ConfigOption>
    {
        /// <summary>
        /// lấy thông tin cấu hình theo name
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        Task<ConfigOptionDto> GetByOptionNameAsync(string optionName);
    }
}
