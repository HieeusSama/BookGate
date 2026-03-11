using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Domain.Entities
{
    public class OrderDetail
    {
        [Key]
        public string OrderDetailId { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public string BookId { get; set; } // Kiểu string để khớp với nvarchar(450) của bảng Books

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
