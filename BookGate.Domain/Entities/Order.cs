using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Domain.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int AuthId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? PaymentDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Ward { get; set; }

        [Required]
        [StringLength(255)]
        public string StreetAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string ReceiverPhone { get; set; }

        // Navigation properties
        [ForeignKey("AuthId")]
        public Auth Auth { get; set; } // Liên kết với bảng người dùng

        [ForeignKey("StatusId")]
        public OrderStatus OrderStatus { get; set; } // Liên kết với bảng trạng thái

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
