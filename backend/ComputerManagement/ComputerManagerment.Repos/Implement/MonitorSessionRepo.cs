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
    public class MonitorSessionRepo(AppDbContext dbContext) : BaseRepo<MonitorSession>(dbContext), IMonitorSessionRepo
    {
        public override async Task<MonitorSession> GetAsync(Guid id)
        {
            return await _dbSet.Include(c => c.ComputerRoom).Include(c=>c.User).FirstOrDefaultAsync(c=>c.Id == id);
        }
        public override async Task<(List<MonitorSession>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<MonitorSession>();

            //if (!string.IsNullOrEmpty(keySearch))
            //{
            //    query = query.Where(e => e.Name.Contains(keySearch));
            //}

            switch (fieldSort?.ToLower())
            {

                case "startdate":
                    query = sortAsc ? query.OrderBy(e => e.StartDate) : query.OrderByDescending(e => e.StartDate);
                    break;
                case "enddate":
                    query = sortAsc ? query.OrderBy(e => e.StartDate) : query.OrderByDescending(e => e.StartDate);
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
                .Include(c => c.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.Include(c => c.ComputerRoom).Include(c => c.User).ToListAsync();
            }
            return (entities, totalCount);
        }

        public async Task<(List<MonitorSession>, int)> GetListComputerByComputerRoomIdAsync(Guid computerRoomId, string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable().Where(m => m.ComputerRoomId == computerRoomId);
            var entities = new List<MonitorSession>();

            //if (!string.IsNullOrEmpty(keySearch))
            //{
            //    query = query.Where(e => e.Name.Contains(keySearch));
            //}

            switch (fieldSort?.ToLower())
            {

                case "startdate":
                    query = sortAsc ? query.OrderBy(e => e.StartDate) : query.OrderByDescending(e => e.StartDate);
                    break;
                case "enddate":
                    query = sortAsc ? query.OrderBy(e => e.StartDate) : query.OrderByDescending(e => e.StartDate);
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
                .Include(c => c.User)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.Include(c => c.ComputerRoom).Include(c => c.User).ToListAsync();
            }


            return (entities, totalCount);
        }
    }
}
