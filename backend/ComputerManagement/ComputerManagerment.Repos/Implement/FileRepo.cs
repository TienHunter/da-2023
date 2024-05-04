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
    public class FileRepo(AppDbContext dbContext) : BaseRepo<FileModel>(dbContext), IFileRepo
    {
        public override async Task<(List<FileModel>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<FileModel>();

            //if (!string.IsNullOrEmpty(keySearch))
            //{
            //    query = query.Where(e => e.Name.Contains(keySearch));
            //}

            switch (fieldSort?.ToLower())
            {

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
                .Include(f => f.Software)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.Include(f => f.Software).ToListAsync();
            }
            return (entities, totalCount);
        }

        public async Task<List<FileModel>> GetListFileBySoftwareId(Guid softwareId)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<FileModel>();
                entities = await _dbSet.Include(f=>f.Software).Where(f => f.SoftwareId == softwareId).ToListAsync();
            return entities;
        }
    }
}
