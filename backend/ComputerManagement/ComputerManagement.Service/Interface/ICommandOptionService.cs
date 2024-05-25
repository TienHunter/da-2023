using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface ICommandOptionService : IBaseService<CommandOption, CommandOption>
    {
        /// <summary>
        /// upsert lệnh vào db qua queue
        /// </summary>
        /// <param name="commandParam"></param>
        /// <returns></returns>
        Task UpsertAsync(CommandParam commandParam);
    }
}
