using ComputerManagement.BO;
using ComputerManagement.BO.Data;
using ComputerManagement.BO.Models;
using ComputerManagerment.Repos.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Implement
{
    public class UserRepo : BaseRepo<User>, IUserRepop
    {
        public UserRepo(AppDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
