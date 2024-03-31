using ComputerManagement.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("computer")]
    public class Computer : BaseModel
    {
        public string AddressIP {  get; set; }
        public string Name { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ComputerState State { get; set; }
        public ComputerCondition Condition { get; set; }

        [ForeignKey("ComputerRoom")]
        public Guid ComputerRoomId { get; set; }
        public ComputerRoom ComputerRoom { get; set; }
    }
}
