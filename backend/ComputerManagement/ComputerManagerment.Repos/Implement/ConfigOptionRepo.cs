﻿using ComputerManagement.BO.Models;
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
    public class ConfigOptionRepo(AppDbContext dbContext) : BaseRepo<ConfigOption>(dbContext), IConfigOptionRepo
    {

        public override async Task<(List<ConfigOption>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, string fieldSort, bool sortAsc)
        {
            var query = _dbSet.AsQueryable();
            var entities = new List<ConfigOption>();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(e => e.OptionName.Contains(keySearch) || e.Desc.Contains(keySearch));
            }

            switch (fieldSort?.ToLower())
            {

                case "optionname":
                    query = sortAsc ? query.OrderBy(e => e.OptionName) : query.OrderByDescending(e => e.OptionName);
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
    }
}
