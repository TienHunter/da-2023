using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TDto,TModel> : ControllerBase
    {
        protected IBaseService<TDto,TModel> _baseService;

        public BaseController(IBaseService<TDto, TModel> baseService)
        {
            _baseService = baseService;
        }

        
    }
}
