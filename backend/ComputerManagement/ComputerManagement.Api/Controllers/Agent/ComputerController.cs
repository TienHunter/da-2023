using ComputerManagement.API.Controllers.Agent;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Implement;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Agent
{
    public partial class ComputerController(IComputerService computerService) : BaseController<ComputerDto, Computer>(computerService)
    {
        private readonly IComputerService _computerService = computerService;


        [HttpGet("GetByMacAddress/{macAddress}")]
        public async Task<IActionResult> GetCommputerByMacAddress([FromRoute] string macAddress)
        {
            var rs = new ServiceResponse();
            rs.Data = await _computerService.GetComputerByMacAddress(macAddress);
            return Ok(rs);

        }


        [HttpPut("UpdateStateByMacAddress/{macAddress}")]
        public async Task<IActionResult> UpdateStateByMacAddress([FromRoute] string macAddress)
        {
            var rs = new ServiceResponse();
            rs.Data = await _computerService.UpdateStateByMacAddressAsync(macAddress);
            return Ok(rs);
        }

        /// <summary>
        /// cập nhật cấu hình máy
        /// </summary>
        /// <param name="computerId"></param>
        /// <param name="computerConfig"></param>
        /// <returns></returns>
        [HttpPut("UpdateComputerConfig/{computerId}")]
        public async Task<IActionResult> UpdateComputerConfig([FromRoute] Guid computerId, [FromBody] ComputerConfig computerConfig)
        {
            var rs = new ServiceResponse();
            rs.Data = await _computerService.UpdateComputerConfigAsync(computerConfig, computerId);
            return Ok(rs);
        }
    }
}

