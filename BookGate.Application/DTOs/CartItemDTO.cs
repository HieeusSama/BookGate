using BookGate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Application.DTOs
{
    public class CartItemDTO
    {
        [Key]
        public string CartItemId { get; set; } = string.Empty;

        public string? FileUrl { get; set; }
        public int Id { get; set; }
        public string BookId { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; } = 1;

        public string Title { get; set; } = string.Empty;

        public decimal SellingPrice { get; set; }
    }
}
