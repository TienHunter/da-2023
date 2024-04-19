﻿using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.BO.DTO
{
    public class ComputerRoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// số hàng
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// số dãy
        /// </summary>
        public int Col { get; set; }
        public int MaxCapacity { get; set; }
        public int CurrentCapacity { get; set; }
        /// <summary>
        /// tình trạng
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// có đang được sử dụng không
        /// </summary>
        public bool Pending { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
