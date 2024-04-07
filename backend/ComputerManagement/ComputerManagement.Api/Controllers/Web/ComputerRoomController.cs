using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Controllers.Web
{
    [Authorize]
    public class ComputerRoomController : BaseController<ComputerRoomDto, ComputerRoom>
    {
        private readonly IComputerRoomService _computerRoomService;
        public ComputerRoomController(IComputerRoomService computerRoomService) : base(computerRoomService)
        {
            _computerRoomService = computerRoomService;
        }
    }
}
