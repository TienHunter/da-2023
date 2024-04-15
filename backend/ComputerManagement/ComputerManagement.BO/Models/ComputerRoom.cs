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
        /// <summary>
        /// tình trạng
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// có đang được sử dụng không
        /// </summary>
        [NotMapped]
        public bool Pending { get; set; }
        public ICollection<ScheduleBookRoom> ScheduleBookRooms { get; } = new List<ScheduleBookRoom>();
    }
}
