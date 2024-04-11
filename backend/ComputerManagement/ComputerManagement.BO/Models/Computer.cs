using ComputerManagement.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("computer")]
    [Index(nameof(MacAddress), IsUnique = true)]
    public class Computer : BaseModel
    {
        /// <summary>
        /// địa chỉ mac
        /// </summary>
        public string MacAddress {  get; set; }

        /// <summary>
        /// số máy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// tọa độ x
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// tọa độ y
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// trạng thái máy
        /// </summary>
        public ComputerState State { get; set; }

        /// <summary>
        /// thời gian cập nhật trạng thái gần nhất
        /// </summary>
        public DateTime StateTime { get; set; }

        /// <summary>
        /// tình trạng máy
        /// </summary>
        public ComputerCondition Condition { get; set; }

        [ForeignKey("ComputerRoom")]
        public Guid ComputerRoomId { get; set; }
        public ComputerRoom ComputerRoom { get; set; }
        public ICollection<ComputerSoftware> ComputerSofewares { get; set; } = new List<ComputerSoftware>();
    }
}
