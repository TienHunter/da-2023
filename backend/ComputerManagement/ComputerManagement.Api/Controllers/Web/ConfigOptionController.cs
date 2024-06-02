﻿using ComputerManagement.BO.DTO.ConfigOption;
using ComputerManagement.BO.Models;
using ComputerManagement.Controllers.Web;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Web
{
    public class ConfigOptionController(IConfigOptionService configOptionService) : BaseController<ConfigOptionDto, ConfigOption>(configOptionService)
    {
    }
}
