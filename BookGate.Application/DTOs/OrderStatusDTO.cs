using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookGate.Application.DTOs
{
    public class OrderStatusDTO
    {
        [Key]
        public string StatusId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; } = string.Empty;

        public string OrderId { get; set; } = string.Empty;
    }
}
