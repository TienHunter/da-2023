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
        /// <summary>
        /// vị trí hàng
        /// </summary>
        [Required]
        public int Row { get; set; }

        /// <summary>
        /// vị trí dãy
        /// </summary>
        [Required]
        public int Col { get; set; }

        /// <summary>
        /// trạng thái máy tính
        /// </summary>
        public ComputerState ComputerState { get; set; }
        /// <summary>
        /// tình trạng máy
        /// </summary>
        public List<ComputerErrorId> ListErrorId { get; set; } = new List<ComputerErrorId>();

        public string? OS { get; set; }
        public string? CPU { get; set; }
        public string? RAM { get; set; }
        public string? HardDriver { get; set; }
        public string? HardDriverUsed { get; set; }
        [Required]
        public Guid ComputerRoomId { get; set; }
        public ComputerRoomDto? ComputerRoom { get; set; } = null;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
