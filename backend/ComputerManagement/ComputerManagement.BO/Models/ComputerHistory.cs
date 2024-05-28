﻿using ComputerManagement.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.Models
{
    [Table("computer_history")]
    public class ComputerHistory
    {
        [Key]
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime LogTime { get; set; }
        public ComputerLevelLog Level { get; set; }
        public Guid ComputerId { get; set; }
        public Guid ComputerRoomId { get; set; }
        public Guid MonitorSessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
