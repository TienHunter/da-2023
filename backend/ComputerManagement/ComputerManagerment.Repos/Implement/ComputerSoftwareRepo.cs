﻿using ComputerManagement.BO.Models;
using ComputerManagement.Data;
using ComputerManagerment.Repos.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Implement
{
    public class ComputerSoftwareRepo(AppDbContext dbContext) : BaseRepo<ComputerSoftware>(dbContext), IComputerSoftwareRepo
    {
    }
}
