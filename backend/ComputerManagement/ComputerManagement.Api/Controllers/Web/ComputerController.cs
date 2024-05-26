﻿using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Implement;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Controllers.Web
{
    public partial class ComputerController(IComputerService computerService) : BaseController<ComputerDto, Computer>(computerService)
    {
        private readonly IComputerService _computerService = computerService;

        [HttpPost("GetListByComputerRoomId/{computerRoomId}")]
        public virtual async Task<IActionResult> GetListByComputerRoomId([FromRoute] Guid
            computerRoomId, [FromBody] PagingParam pagingParam)
        {
            var rs = new ServiceResponse();
            rs.Data = await _computerService.GetListComputerByComputerRoomIdAsync(computerRoomId, pagingParam);
            return Ok(rs);

        }

        [HttpPost("GetListFilterBySoftware/{softwareId}")]
        public virtual async Task<IActionResult> GetList([FromRoute] Guid softwareId, [FromBody] PagingParam pagingParam)
        {
            var rs = new ServiceResponse();
            var (computerDtos, totalCount) = await _computerService.GetListBySoftwareIdAsync(softwareId, pagingParam);
            rs.Data = new
            {
                List = computerDtos,
                Total = totalCount
            };
            return Ok(rs);

        }
    }
}
