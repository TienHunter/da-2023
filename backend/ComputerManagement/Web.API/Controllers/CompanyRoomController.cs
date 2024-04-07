using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyRoomController : BaseController<ComputerRoomDto, ComputerRoom>
    {
        private readonly IComputerRoomService _computerRoomService;
        public CompanyRoomController(IComputerRoomService computerRoomService) : base(computerRoomService)
        {
            _computerRoomService = computerRoomService;
        }
    }
}
