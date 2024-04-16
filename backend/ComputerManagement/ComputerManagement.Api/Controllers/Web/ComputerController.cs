using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Controllers.Web
{
    public partial class ComputerController : BaseController<ComputerDto, Computer>
    {
        private readonly IComputerService _computerService;
        public ComputerController(IComputerService computerService) : base(computerService)
        {
            _computerService = computerService;
        }
    }
}
