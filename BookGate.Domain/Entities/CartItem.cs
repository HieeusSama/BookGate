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
        public string CartItemId { get; set; } 

        [Required]
        public int Id { get; set; } 

        [Required]
        public string BookId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [ForeignKey("Id")]
        public Auth Auth { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
