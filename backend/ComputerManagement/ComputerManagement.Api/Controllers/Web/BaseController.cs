using ComputerManagement.BO.DTO;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComputerManagement.Controllers.Web
{
    [Route("api-web/[controller]")]
    [ApiController]
    public abstract partial class BaseController<TDto, TModel> : ControllerBase
    {
        protected IBaseService<TDto, TModel> _baseService;

        public BaseController(IBaseService<TDto, TModel> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var rs = new ServiceResponse
            {
                Data = await _baseService.GetAsync(id)
            };
            return Ok(rs);
        }

        [HttpPost("GetList")]
        public virtual async Task<IActionResult> GetList([FromQuery] PagingParam pagingParam)
        {
            var rs = new ServiceResponse();
            var (enitties, totalCount) = await _baseService.GetListAsync(pagingParam);
            rs.Data = new
            {
                List = enitties,
                Total = totalCount
            };
            return Ok(rs);

        }

        [HttpPost("")]
        public virtual async Task<IActionResult> Add([FromBody] TDto dto)
        {
            var rs = new ServiceResponse
            {
                Data = await _baseService.AddAsync(dto)
            };
            return Ok(rs);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update([FromBody] TDto dto, [FromRoute] Guid id)
        {
            var rs = new ServiceResponse();
            rs.Data = await _baseService.UpdateAsync(dto,id);
            return Ok(rs);
        }
    }
}
