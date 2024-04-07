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
        Task<TDto> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<TModel> BeforeAddAsync(TDto dto);
        Task AfterAddAsync(TModel model);
        Task<TModel> BeforeUpdateAsync(TDto dto);
        Task AfterUpdateAsync(TModel model);
        Task ValidateBeforeAddAsync(TModel model);
        Task ValidateBeforeUpdateAsync(TModel model);
        Task ValidateBeforeDeleteAsync(Guid id);
        Task<(List<TDto>, int)> GetListAsync(PagingParam pagingParam);
    }
}
