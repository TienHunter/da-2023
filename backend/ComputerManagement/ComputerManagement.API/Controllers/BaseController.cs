using ComputerManagement.BO.DTO;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TDto, TModel> : ControllerBase
    {
        protected IBaseService<TDto, TModel> _baseService;

        public BaseController(IBaseService<TDto, TModel> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("id")]
        public virtual async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var rs = new ServiceResponse
            {
                Data = await _baseService.GetAsync(id)
            };
            return Ok(rs);
        }

        [HttpGet("")]
        public virtual async Task<IActionResult> GetList()
        {
            throw new NotImplementedException();
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

        [HttpPut("")]
        public virtual async Task<IActionResult> Update([FromBody] TDto dto)
        {
            var rs = new ServiceResponse
            {
                Data = await _baseService.UpdateAsync(dto)
            };
            return Ok(rs);
        }
    }
}
