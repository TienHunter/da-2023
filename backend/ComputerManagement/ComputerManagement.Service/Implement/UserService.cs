using ComputerManagement.BO.DTO;
using ComputerManagement.BO.DTO.Users;
using ComputerManagement.BO.Lib.Interface;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class UserService : BaseService<UserDto, User>, IUserService
    {
        private readonly IJwtGenerator _jwtGenerate;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepop _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IServiceProvider serviceProvider, IUserRepop userRepo) : base(serviceProvider, userRepo) {
            _userRepo = userRepo;
            _jwtGenerate = serviceProvider.GetService(typeof(IJwtGenerator)) as IJwtGenerator;
            _passwordHasher = serviceProvider.GetService(typeof(IPasswordHasher)) as IPasswordHasher;
            _httpContextAccessor = serviceProvider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

        }

        public async Task<ServiceResponse> Login(UserLogin userLogin)
        {
            var rs = new ServiceResponse();
            var userExist = await _userRepo.GetQueryable()
                                    .Where(u => u.Username == userLogin.Accountname || u.Email == userLogin.Accountname)
                                    .FirstOrDefaultAsync() ?? throw new BaseException
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Code = ServiceResponseCode.NotUserLogin
                };
            bool verifyPassword = _passwordHasher.Verify(userExist.Password, userLogin.Password);
            if (verifyPassword)
            {
                var accessToken = _jwtGenerate.GenerateJwt(userExist);

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    // Gán Access Token vào cookie
                    httpContext.Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
                    {
                        HttpOnly = true, // Chỉ có thể truy cập từ phía máy chủ
                        Secure = true, // Chỉ gửi cookie qua kết nối HTTPS
                        SameSite = SameSiteMode.Strict // Giảm nguy cơ tấn công CSRF
                                                       // Có thể thêm các thuộc tính khác của cookie ở đây
                    });
                }
                 rs.Data = true;
                return rs;
            }else
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Code = ServiceResponseCode.WrongPassword
                };
            }
        }

        public async Task<ServiceResponse> Register(UserRegister userRegister)
        {
            var rs = new ServiceResponse();
            var useExistByUsername = await _userRepo.GetQueryable().Where(u => u.Username == userRegister.Username).FirstOrDefaultAsync();
            if(useExistByUsername != null)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.UsernameConflict,
                };
            }

            var useExistByEmail = await _userRepo.GetQueryable().Where(u => u.Email == userRegister.Email).FirstOrDefaultAsync();
            if (useExistByEmail != null)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.EmailConflict,
                };
            }

            var user = _mapper.Map<User>(userRegister);
            await base.BeforeSaveAsync(user);
            user.Password = _passwordHasher.Hash(user.Password);
            // xử lý phân quyền và trạng thái làm sau
            //user.RoleID = (int)UserRole.Teacher;
            //user.State = UserState.Active;

            rs.Data = await _userRepo.AddAsync(user);
            return rs;
        }

        public async Task<ServiceResponse> ChangePassword(UserChangePassword userChangePassword)
        {
            var rs = new ServiceResponse();
            var user = await _userRepo.GetAsync(_contextData.UserId) ?? throw new BaseException
            {
                StatusCode= HttpStatusCode.NotFound,
                Code = ServiceResponseCode.Exception
            };

            var veryfyPassword = _passwordHasher.Verify(user.Password, userChangePassword.OldPassword);
            if(!veryfyPassword)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Code = ServiceResponseCode.WrongPassword
                };
            }
            user.CmEntityState = CmEntityState.Update;
            await base.BeforeSaveAsync(user);
            user.Password = _passwordHasher.Hash(userChangePassword.NewPassword);

            rs.Data = await _userRepo.UpdateAsync(user);

            return rs;
        }

        public Task<ServiceResponse> ResetPassword(UserResetPassword userResetPassword)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                if (httpContext.Request.Cookies["AccessToken"] != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    httpContext.Response.Cookies.Append("AccessToken", "", cookieOptions);
                }
            }
        }
    }
}
