using ComputerManagement.API.Controllers.Agent;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Agent
{
    public class SoftwareController(ISoftwareService softwareService) : BaseController<SoftwareDto, SoftwareModel>(softwareService)
    {
        private readonly ISoftwareService _softwareService = softwareService;

        /// <summary>
        /// lấy tất cả phần mềm
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var rs = new ServiceResponse();
            var paging = new PagingParam();
            rs.Data = await _softwareService.GetListAsync(paging);
            return Ok(rs);
        }
    }
}
