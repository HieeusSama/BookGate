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
        public string StatusId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; }
    }
}
