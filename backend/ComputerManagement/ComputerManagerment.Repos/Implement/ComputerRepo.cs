using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Data;
using ComputerManagerment.Repos.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagerment.Repos.Implement
{
    public class ComputerRepo(AppDbContext dbContext) : BaseRepo<Computer>(dbContext), IComputerRepo
    {
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
                .Select(c => new Computer
                {
                    Id = c.Id,
                    MacAddress = c.MacAddress,
                    Name = c.Name,
                    Row = c.Row,
                    Col = c.Col,
                    ListErrorId = c.ListErrorId,
                    OS = c.OS,
                    CPU = c.CPU,
                    RAM = c.RAM,
                    HardDriver = c.HardDriver,
                    HardDriverUsed = c.HardDriverUsed,
                    ComputerRoomId = c.ComputerRoomId,
                    ComputerRoom = new ComputerRoom
                    {
                        Id = c.ComputerRoom.Id,
                        Name = c.ComputerRoom.Name,
                        Row = c.ComputerRoom.Row,
                        Col = c.ComputerRoom.Col,
                        CurrentCapacity = c.ComputerRoom.CurrentCapacity,
                        MaxCapacity = c.ComputerRoom.MaxCapacity,
                    }
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
            {
                entities = await query.Include(c => c.ComputerRoom).Select(c => new Computer
                {
                    Id = c.Id,
                    MacAddress = c.MacAddress,
                    Name = c.Name,
                    Row = c.Row,
                    Col = c.Col,
                    ListErrorId = c.ListErrorId,
                    OS = c.OS,
                    CPU = c.CPU,
                    RAM = c.RAM,
                    HardDriver = c.HardDriver,
                    HardDriverUsed = c.HardDriverUsed,
                    ComputerRoomId = c.ComputerRoomId,
                    ComputerRoom = new ComputerRoom
                    {
                        Id = c.ComputerRoom.Id,
                        Name = c.ComputerRoom.Name,
                        Row = c.ComputerRoom.Row,
                        Col = c.ComputerRoom.Col,
                        CurrentCapacity = c.ComputerRoom.CurrentCapacity,
                        MaxCapacity = c.ComputerRoom.MaxCapacity,
                    }
                }).ToListAsync();
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
            entities = await query.Include(c => c.ComputerRoom).Include(c => c.ComputerState).ToListAsync();
            return entities;
        }
    }
}
