using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.DTO
{
    public class SoftwareDto : BaseDto
    {
        /// <summary>
        /// Tên phần mềm
        /// </summary>
        [Required]
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
