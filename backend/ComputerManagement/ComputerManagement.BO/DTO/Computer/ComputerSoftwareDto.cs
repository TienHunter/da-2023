using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.DTO
{
    public class ComputerSoftwareDto : BaseDto
    {
        [Required]
        public Guid ComputerId { get; set; }
        public ComputerDto? Computer { get; set; }
        [Required]
        public Guid SoftwareId { get; set; }
        [Required]
        public string SoftwareName { get; set; }
        /// <summary>
        /// phần mềm đã được cài đặt chưa
        /// </summary>
        public bool IsInstalled { get; set; }
    }
}
