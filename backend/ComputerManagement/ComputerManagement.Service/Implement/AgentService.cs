using ComputerManagement.BO.DTO.Agent;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class AgentService(IServiceProvider serviceProvider, IAgentRepo agentRepo) : BaseService<AgentDto, AgentModel>(serviceProvider, agentRepo), IAgentService
    {
        private readonly IAgentRepo _agentRepo = agentRepo;

        public async Task<AgentDto> GetFirstAsync()
        {
            var rs = await _agentRepo.GetQueryable().FirstOrDefaultAsync();
            
            return _mapper.Map<AgentDto>(rs);
        }
    }
}
