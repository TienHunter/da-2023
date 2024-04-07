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
        public string SoftwareCode { get; set; }
        public string SoftwareName { get; set;}
        public string Version { get; set;}

        [ForeignKey("Computer")]
        public Guid ComputerId { get; set;}
        public Computer computer { get; set;}
    }
}
