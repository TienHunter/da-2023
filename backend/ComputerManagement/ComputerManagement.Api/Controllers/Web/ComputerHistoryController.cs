using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Controllers.Web;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.Api.Controllers.Web
{
    public partial class ComputerHistoryController(IComputerHistoryService computerHistoryService) : BaseController<ComputerHistoryDto, ComputerHistory>(computerHistoryService)
    {
        private readonly IComputerHistoryService _computerHistoryService = computerHistoryService;

        [HttpPost("GetListByComputerId/{computerId}")]
        public virtual async Task<IActionResult> GetListByComputerId([FromQuery] PagingParam pagingParam, [FromRoute] Guid computerId)
        {
            var rs = new ServiceResponse();
            var (enitties, totalCount) = await _computerHistoryService.GetListByComputerIdAsync(computerId,pagingParam);
            rs.Data = new
            {
                List = enitties,
                Total = totalCount
            };
            return Ok(rs);

        }
    }
}
