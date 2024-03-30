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
        Task<TModel> GetAsync(Guid id);
        Task<TModel> AddAsync(TDto dto);
        Task<TModel> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task BeforeSaveAsync(TModel entity);
        Task AfterSaveAsync(TModel entity);
        Task<(List<TModel>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, List<string> fieldsSearch, Dictionary<string, string> fieldToSort);
    }
}
