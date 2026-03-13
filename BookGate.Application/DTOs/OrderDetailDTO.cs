using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Application.DTOs
{
    public class OrderDetailDTO
    {
        [Key]
        public string OrderDetailId { get; set; } = string.Empty;

        [Required]
        public string OrderId { get; set; } = string.Empty;

        [Required]
        public string BookId { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

    }
}
