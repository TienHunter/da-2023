
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("computer_software")]
    public class ComputerSoftware : BaseModel
    {
        [ForeignKey("Computer")]
        public Guid ComputerId { get; set;}
        public Computer Computer { get; set;}
        public Guid SoftwareId { get; set;}
        public string SoftwareName { get; set;}
        /// <summary>
        /// phần mềm đã được cài đặt chưa
        /// </summary>
        public bool IsInstalled { get; set;}
    }
}
