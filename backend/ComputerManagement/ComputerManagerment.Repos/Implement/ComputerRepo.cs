﻿using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Data;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Implement
{
    public class ComputerRepo(AppDbContext dbContext) : BaseRepo<Computer>(dbContext), IComputerRepo
    {
        public override async Task<List<Computer>> GetListByIdsAsync(List<Guid> ids)
        {
            return await _dbSet.Where(cr => ids.Contains(cr.Id)).ToListAsync();
        }
        public override async Task<Computer?> GetAsync(Guid id)
        {
            var rs =  await _dbSet.Include(c => c.ComputerRoom).SingleOrDefaultAsync(c => c.Id == id);
          
            return rs;
        }

        public override async Task<(List<Computer>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc, Dictionary<string, string>? Filters = null)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<Computer>();
            query = query.Include(c => c.ComputerRoom);
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(e => e.Name.Contains(keySearch) || (e.ComputerRoom != null && e.ComputerRoom.Name.Contains(keySearch)));
            }

            switch (fieldSort?.ToLower())
            {
                case "computerroomname":
                    query = sortAsc ? query.OrderBy(e => e.ComputerRoom.Name) : query.OrderByDescending(e => e.ComputerRoom.Name);
                    break;
                case "name":
                    query = sortAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                    break;
                case "updatedat":
                    query = sortAsc ? query.OrderBy(e => e.UpdatedAt) : query.OrderByDescending(e => e.UpdatedAt);
                    break;
                case "createdat":
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
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.ToListAsync();
            }


            return (entities, totalCount);
        }

        public async Task<List<Computer>> GetListComputerByComputerRoomIdAsync(Guid computerRoomId, string keySearch, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<Computer>();
            query = query.Where(c => c.ComputerRoomId == computerRoomId);
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(e => e.Name.Contains(keySearch));
            }

            switch (fieldSort?.ToLower())
            {
               
                case "name":
                    query = sortAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                    break;
                case "updatedat":
                    query = sortAsc ? query.OrderBy(e => e.UpdatedAt) : query.OrderByDescending(e => e.UpdatedAt);
                    break;
                case "createdat":
                    query = sortAsc ? query.OrderBy(e => e.CreatedAt) : query.OrderByDescending(e => e.CreatedAt);
                    break;

                default:
                    query = query.OrderByDescending(e => e.UpdatedAt);
                    break;
            }
            entities = await query.Include(c => c.ComputerRoom).Include(c => c.ComputerState).ToListAsync();

            return entities;
        }

        public async Task<(List<Computer>, int)> GetListBySoftwareIdAsync(Guid softwareId, string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable();
            query = query.Include(c => c.ComputerRoom).Include(c => c.ComputerSoftwares.Where(cs => cs.SoftwareId == softwareId));
            var entities = new List<Computer>();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(e => e.Name.Contains(keySearch) || (e.ComputerRoom != null && e.ComputerRoom.Name.Contains(keySearch)));
            }

            switch (fieldSort?.ToLower())
            {
                case "computerroomname":
                    query = sortAsc ? query.OrderBy(e => e.ComputerRoom.Name) : query.OrderByDescending(e => e.ComputerRoom.Name);
                    break;
                case "name":
                    query = sortAsc ? query.OrderBy(e => e.Name) : query.OrderByDescending(e => e.Name);
                    break;
                case "updateat":
                    query = sortAsc ? query.OrderBy(e => e.UpdatedAt) : query.OrderByDescending(e => e.UpdatedAt);
                    break;
                case "createdat":
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
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.ToListAsync();
            }
            return (entities, totalCount);
        }

    }
}
