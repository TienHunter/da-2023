using ComputerManagement.API.Controllers.Agent;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Agent
{
    public partial class ComputerController(IComputerService computerService) : BaseController<ComputerDto, Computer>(computerService)
    {
        private readonly IComputerService _computerService = computerService;

        [HttpPut("update-state/{macAddress}")]
        public async Task<IActionResult> UpdateStateByMacAddress([FromRoute] string macAddress)
        {
            var rs = new ServiceResponse();
            rs.Data = await _computerService.UpdateStateByMacAddressAsync(macAddress);
            return Ok();
        }
    }
}
