using ComputerManagement.BO.Models;
using ComputerManagement.Data;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Implement
{
    public class ComputerRepo : BaseRepo<Computer>, IComputerRepo
    {
        public ComputerRepo(AppDbContext dbContext) : base(dbContext)
        {

        }

        public override async Task<Computer?> GetAsync(Guid id)
        {
            return await _dbSet.Include(c => c.ComputerRoom).SingleOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<(List<Computer>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<Computer>();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(e => e.Name.Contains(keySearch));
            }

            switch (fieldSort?.ToLower())
            {

                case "Name":
                    query = sortAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                    break;
                case "UpdatedAt":
                    query = sortAsc ? query.OrderBy(e => e.UpdatedAt) : query.OrderByDescending(e => e.UpdatedAt);
                    break;
                case "CreatedAt":
                    query = sortAsc ? query.OrderBy(e => e.CreatedAt) : query.OrderByDescending(e => e.CreatedAt);
                    break;

                default:
                    query = query.OrderByDescending(e => e.UpdatedAt);
                    break;
            }
            var totalCount = await query.CountAsync();
            if (pageNumber > 0 && pageSize > 0)
            {
                entities = await query
                .Include(c => c.ComputerRoom)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.Include(c => c.ComputerRoom).ToListAsync();
            }


            return (entities, totalCount);
        }
    }
}
