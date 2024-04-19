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
        /// <summary>
        /// tên phòng máy
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// số hàng
        /// </summary>
        public int Row { get;set; }
        
        /// <summary>
        /// số dãy
        /// </summary>
        public int Col { get;set; }

        /// <summary>
        /// số máy tối đa
        /// </summary>
        public int MaxCapacity { get; set; }

        /// <summary>
        /// số máy hiện tại
        /// </summary>
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
