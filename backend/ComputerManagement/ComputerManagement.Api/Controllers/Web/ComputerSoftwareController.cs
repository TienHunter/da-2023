﻿using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Controllers.Web;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Web
{
    public class ComputerSoftwareController(IComputerSoftwareService computerSoftwareService) : BaseController<ComputerSoftwareDto, ComputerSoftware>(computerSoftwareService)
    {
    }
}
