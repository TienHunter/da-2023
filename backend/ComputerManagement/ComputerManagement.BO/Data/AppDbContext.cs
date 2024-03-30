using ComputerManagement.BO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options ) : base( options )
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ComputerRoom> ComputerRooms { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<User> ScheduleBookRoom { get; set; }
    }
}
