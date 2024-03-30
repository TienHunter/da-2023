using ComputerManagement.BO.DTO;
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
        Task<ServiceResponse> Login(UserLogin userLogin);
    }
}
