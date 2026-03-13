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
        public string OrderId { get; set; } = string.Empty;

        [Required]
        public int Id { get; set; }

        [Required]
        public string StatusId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Ward { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ReceiverPhone { get; set; } = string.Empty;

        [ForeignKey("Id")]
        public Auth? Auth { get; set; }

        [ForeignKey("StatusId")]
        public OrderStatus? OrderStatus { get; set; }

        public ICollection<OrderDetail>? OrderDetail { get; set; }
    }
}
