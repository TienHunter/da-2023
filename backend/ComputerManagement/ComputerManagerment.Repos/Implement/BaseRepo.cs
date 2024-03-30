using ComputerManagement.BO;
using ComputerManagement.BO.Data;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace ComputerManagerment.Repos.Implement
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        protected AppDbContext _dbContext;
        internal DbSet<T> _dbSet;

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
            _dbSet.Update(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<(List<T> entities, int totalCount)> GetListAsync(string keySearch, int pageNumber, int pageSize, List<string> fieldsSearch, Dictionary<string, string> fieldToSort)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<T>();
            if (pageNumber >= 0 && pageSize > 0)
            {
                // Apply search filter if searchKey and fieldsToSearch are provided
                if (!string.IsNullOrEmpty(keySearch) && fieldsSearch != null && fieldsSearch.Any())
                {
                    var searchConditions = new List<Expression<Func<T, bool>>>();
                    foreach (var field in fieldsSearch)
                    {
                        var propertyInfo = typeof(T).GetProperty(field);
                        if (propertyInfo != null)
                        {
                            Expression<Func<T, bool>> searchCondition = entity =>
                                propertyInfo.GetValue(entity) != null ? propertyInfo.GetValue(entity).ToString().Contains(keySearch) : false;

                            searchConditions.Add(searchCondition);
                        }
                    }
                    if (searchConditions != null && searchConditions.Any())
                    {
                        var combinedExpression = searchConditions.Aggregate((current, next) =>
                            Expression.Lambda<Func<T, bool>>(
                                Expression.Or(current.Body, next.Body),
                                current.Parameters));
                        query = query.Where(combinedExpression);
                    }

                }



                // Apply sorting if fieldsToSort is provided
                //if (fieldToSort != null)
                //{
                //    var sortExpressions = new List<Expression<Func<T, object>>>();
                //    foreach (var kvp in fieldToSort)
                //    {
                //        var propertyInfo = typeof(T).GetProperty(kvp.Key);
                //        if (propertyInfo != null)
                //        {
                //            Expression<Func<T, object>> sortExpression = entity => propertyInfo.GetValue(entity);
                //            sortExpressions.Add(kvp.Value == "asc" ? sortExpression : Expression.Lambda<Func<T, object>>(Expression.Negate(sortExpression.Body), sortExpression.Parameters));
                //        }
                //    }
                //    if (sortExpressions.Any())
                //    {
                //        var combinedSortExpression = sortExpressions.Aggregate(Expression.ThenBy);
                //        query = query.OrderBy(combinedSortExpression);
                //    }
                //}
                if (true)
                {
                    var propertyInfo = typeof(T).GetProperty("UpdateBy");
                    if (propertyInfo != null)
                    {
                        query = query.OrderBy(entity => propertyInfo.GetValue(entity)); // Default sorting by Id if fieldsToSort is not provided
                    }
                }
                entities = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await _dbSet.ToListAsync();
            }
            var totalCount = await query.CountAsync();

            return (entities, totalCount);
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T item)
        {
            _dbContext.Update(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
