using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("software")]
    public class SoftwareModel : BaseModel
    {
        /// <summary>
        /// Tên phần mềm
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// cờ update
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// cờ cài đặt
        /// </summary>
        public bool IsInstall { get; set; }
    }
}
