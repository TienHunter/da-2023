using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseController<ComputerDto, Computer>
    {
        private readonly IComputerService _computerService;
        public CompanyController(IComputerService computerService) : base(computerService)
        {
            _computerService = computerService;
        }
    }
}
