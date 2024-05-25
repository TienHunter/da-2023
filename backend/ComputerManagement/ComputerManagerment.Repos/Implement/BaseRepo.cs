using ComputerManagement.BO;
using ComputerManagement.Data;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
namespace ComputerManagerment.Repos.Implement
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        protected AppDbContext _dbContext;
        protected DbSet<T> _dbSet;

        public BaseRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet;
        }

        public virtual async Task<bool> AddAsync(T item)
        {
            _dbSet.AddAsync(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<(List<T>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T?> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> UpdateAsync(T item)
        {
            _dbContext.Update(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetListByIdsAsync(List<Guid> ids)
        {
            return await _dbSet.Where(entity => ids.Contains(this.GetIdFromEntity(entity))).ToListAsync();
        }

        private Guid GetIdFromEntity(T entity)
        {
            // Giả sử rằng mỗi thực thể có một thuộc tính ID có kiểu Guid.
            // Thay thế GetIdFromEntity bằng cách lấy giá trị ID của thực thể cụ thể.
            var property = typeof(T).GetProperty("ID"); // Thay "ID" bằng tên của thuộc tính ID thực sự
            if (property != null && property.PropertyType == typeof(Guid))
            {
                return (Guid)property.GetValue(entity);
            }
            throw new ArgumentException("Thực thể không có thuộc tính ID hoặc không phải là kiểu Guid.");
        }

        public async Task<bool> DeleteRangeAsync(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
