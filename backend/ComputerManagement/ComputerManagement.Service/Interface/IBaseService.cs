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
        /// <summary>
        /// thêm đói tượng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Guid> AddAsync(TDto dto);
        Task<bool> UpdateAsync(TDto dto, Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<Guid> BeforeAddAsync(TModel model);
        Task AfterAddAsync(TModel model);
        Task BeforeUpdateAsync(TModel model);
        Task AfterUpdateAsync(TModel model);
        Task ValidateBeforeAddAsync(TModel model);
        Task ValidateBeforeUpdateAsync(TModel model);
        Task ValidateBeforeDeleteAsync(TModel model);
        Task BeforeMapUpdateAsync(TDto dto, TModel model);
        Task<(List<TDto>, int)> GetListAsync(PagingParam pagingParam);
    }
}
