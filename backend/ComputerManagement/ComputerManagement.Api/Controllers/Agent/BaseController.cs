﻿using ComputerManagement.BO.DTO;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComputerManagement.API.Controllers.Agent
{
    [Route("api-agent/[controller]")]
    [ApiController]
    public abstract partial class BaseController<TDto, TModel>(IBaseService<TDto, TModel> baseService) : ControllerBase
    {
        protected IBaseService<TDto, TModel> _baseService = baseService;

        //[HttpGet("{id}")]
        //public virtual async Task<IActionResult> GetById([FromRoute] Guid id)
        //{
        //    var rs = new ServiceResponse
        //    {
        //        Data = await _baseService.GetAsync(id)
        //    };
        //    return Ok(rs);
        //}

        //[HttpGet("GetList")]
        //public virtual async Task<IActionResult> GetList([FromQuery] PagingParam pagingParam)
        //{
        //    var rs = new ServiceResponse();
        //    var (enitties, totalCount) = await _baseService.GetListAsync(pagingParam);
        //    rs.Data = new
        //    {
        //        List = enitties,
        //        Total = totalCount
        //    };
        //    return Ok(rs);

        //}

        //[HttpPost("")]
        //public virtual async Task<IActionResult> Add([FromBody] TDto dto)
        //{
        //    var rs = new ServiceResponse
        //    {
        //        Data = await _baseService.AddAsync(dto)
        //    };
        //    return Ok(rs);
        //}

        //[HttpPut("{id}")]
        //public virtual async Task<IActionResult> Update([FromBody] TDto dto,[FromRoute] [Required] Guid id)
        //{
        //    var rs = new ServiceResponse
        //    {
        //        Data = await _baseService.UpdateAsync(dto)
        //    };
        //    return Ok(rs);
        //}
    }
}