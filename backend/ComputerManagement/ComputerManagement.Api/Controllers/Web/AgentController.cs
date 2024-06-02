
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.DTO.Agent;
using ComputerManagement.BO.DTO.File;
using ComputerManagement.BO.Models;
using ComputerManagement.Controllers.Web;
using ComputerManagement.Service.Implement;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Web
{

    public class AgentController(IAgentService agentService) : BaseController<AgentDto,AgentModel>(agentService)
    {
        private readonly IAgentService _agentService = agentService;
        [HttpGet("GetFirst")]
        public async Task<IActionResult> GetFirst()
        {
            var rs = new ServiceResponse();
            rs.Data = await _agentService.GetFirstAsync();
            return Ok(rs);

        }
    }
}
