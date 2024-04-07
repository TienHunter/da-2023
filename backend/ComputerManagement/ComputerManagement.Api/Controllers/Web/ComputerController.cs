using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Controllers.Web
{
    public partial class CompanyController : BaseController<ComputerDto, Computer>
    {
        private readonly IComputerService _computerService;
        public CompanyController(IComputerService computerService) : base(computerService)
        {
            _computerService = computerService;
        }

        [HttpGet("{macAddress}")]
        public async Task<IActionResult> GetCommputerByMacAddress([FromRoute] string macAddress)
        {
            var rs = new ServiceResponse();
            rs.Data = await _computerService.GetComputerByMacAddress(macAddress);
            return Ok();
        }
    }
}
