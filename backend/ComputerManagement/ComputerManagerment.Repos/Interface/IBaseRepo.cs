using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Interface
{
    public interface IBaseRepo<T> 
    {
        IQueryable<T> GetQueryable();
        Task<(List<T>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc);

        Task<T?> GetAsync(Guid id);

        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T entity);

        Task<int> SaveChangeAsync();
    }
}
