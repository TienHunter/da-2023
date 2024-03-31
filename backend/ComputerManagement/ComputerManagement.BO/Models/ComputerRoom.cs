using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("computer_room")]
    public class ComputerRoom : BaseModel
    {
        public string Name { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
        public ICollection<Computer> Computers { get; } = new List<Computer>();
        public ICollection<ScheduleBookRoom> ScheduleBookRooms { get; } = new List<ScheduleBookRoom>();
    }
}
