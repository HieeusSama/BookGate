using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Domain.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int AuthId { get; set; }

        [Required]
        public string BookId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        // Navigation properties
        [ForeignKey("AuthId")]
        public Auth Auth { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
