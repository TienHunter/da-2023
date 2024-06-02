using ComputerManagement.BO.DTO.Agent;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IAgentService : IBaseService<AgentDto, AgentModel>
    {
        /// <summary>
        /// lấy bản ghi đầu tiên
        /// </summary>
        /// <returns></returns>
        Task<AgentDto> GetFirstAsync();
    }
}
