using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Lib;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class UserService : BaseService<UserDto, User>, IUserService
    {
        private readonly IJwtGenerator _jwtGenerate;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepop _userRepo;
        public UserService(IServiceProvider serviceProvider, IUserRepop userRepo) : base(serviceProvider, userRepo) {
            _userRepo = userRepo;
            _jwtGenerate = serviceProvider.GetService(typeof(IJwtGenerator)) as IJwtGenerator;
            _passwordHasher = serviceProvider.GetService(typeof(IPasswordHasher)) as IPasswordHasher;

        }
        public async Task<ServiceResponse> Login(UserLogin userLogin)
        {
            var rs = new ServiceResponse();
            var userExist = await _userRepo.GetQueryable()
                                    .Where(u => u.Username == userLogin.Accountname || u.Email == userLogin.Accountname)
                                    .SingleAsync() ?? throw new BaseException
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Code = ServiceResponseCode.NotUserLogin
                };
            bool verifyPassword = _passwordHasher.Verify(userExist.Password, userLogin.Password);
            if (verifyPassword)
            {
                var accessToken = _jwtGenerate.GenerateJwt(userExist);
                rs.Data = accessToken;
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
    }
}
