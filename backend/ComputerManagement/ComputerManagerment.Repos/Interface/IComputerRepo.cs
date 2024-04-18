﻿using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Interface
{
    public interface IComputerRepo : IBaseRepo<Computer>
    {
        /// <summary>
        /// lấy danh sách máy tính theo id phòng máy
        /// </summary>
        /// <param name="computerId"></param>
        /// <param name="keySearch"></param>
        /// <param name="fieldSort"></param>
        /// <param name="sortAsc"></param>
        /// <returns></returns>
        Task<List<Computer>> GetListComputerByComputerRoomIdAsync(Guid computerRoomId, string keySearch, string fieldSort, bool sortAsc);
    }
}
