using ComputerManagement.BO.DTO;
using ComputerManagement.BO.DTO.Users;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service
{
    public interface IUserService : IBaseService<UserDto, User>
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<ServiceResponse> Login(UserLogin userLogin);

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userRegister"></param>
        /// <returns></returns>
        Task<ServiceResponse> Register(UserRegister userRegister);
        /// <summary>
        /// change password
        /// </summary>
        /// <param name="userChangePassword"></param>
        /// <returns></returns>
        Task<ServiceResponse> ChangePassword(UserChangePassword userChangePassword);

        Task<ServiceResponse> ResetPassword(UserResetPassword userResetPassword);

        void Logout();
    }
}
