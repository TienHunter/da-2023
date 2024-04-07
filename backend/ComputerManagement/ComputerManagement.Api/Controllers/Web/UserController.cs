using ComputerManagement.BO.DTO;
using ComputerManagement.BO.DTO.Users;
using ComputerManagement.BO.Models;
using ComputerManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Controllers.Web
{
    [ApiController]
    public class UserController : BaseController<UserDto, User>
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public  async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var rs = await _userService.Login(userLogin);
            return Ok(rs);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public  async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            var rs = await _userService.Register(userRegister);
            return Ok(rs);
        }

        [HttpPut("ChangePassword")]
        public  async Task<IActionResult> ChangePassword([FromBody] UserChangePassword userChangePassword)
        {
            var rs = await _userService.ChangePassword(userChangePassword);

            return Ok(rs);
        }

        [HttpPut("Logout")]
        public IActionResult Logout()
        {
            var rs = new ServiceResponse();
            _userService.Logout();
            return Ok(rs);
        }

        [HttpPut("check-login")]
        public virtual IActionResult CheckLogin()
        {
            var rs = new ServiceResponse();
            return Ok(rs);
        }

    }
}
