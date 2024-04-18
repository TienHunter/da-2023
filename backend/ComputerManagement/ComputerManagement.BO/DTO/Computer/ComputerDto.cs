﻿using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.DTO
{
    public class ComputerDto
    {
        public Guid Id { get; set; }
        [Required]
        public string MacAddress { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int Col { get; set; }
        public ComputerState State { get; set; }
        public DateTime StateTime { get; set; }
        /// <summary>
        /// tình trạng máy
        /// </summary>
        public List<string> ListErrorId { get; set; } = new List<string>();
        [Required]
        public Guid ComputerRoomId { get; set; }
        public ComputerRoom? ComputerRoom { get; set; } = null;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
