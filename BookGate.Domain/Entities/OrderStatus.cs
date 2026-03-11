using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Domain.Entities
{
    public class OrderStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        // Navigation property: Một trạng thái có thể có nhiều đơn hàng
        public ICollection<Order> Orders { get; set; }
    }
}
