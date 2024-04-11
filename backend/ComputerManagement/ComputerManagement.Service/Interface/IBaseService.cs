using ComputerManagement.BO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IBaseService<TDto, TModel>
    {
        Task<TDto> GetAsync(Guid id);
        Task<TDto> AddAsync(TDto dto);
        Task<TDto> UpdateAsync(TDto dto, Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task BeforeAddAsync(TModel model);
        Task AfterAddAsync(TModel model);
        Task BeforeUpdateAsync(TModel model);
        Task AfterUpdateAsync(TModel model);
        Task ValidateBeforeAddAsync(TModel model);
        Task ValidateBeforeUpdateAsync(TModel model);
        Task ValidateBeforeDeleteAsync(TModel model);
        Task<(List<TDto>, int)> GetListAsync(PagingParam pagingParam);
    }
}
