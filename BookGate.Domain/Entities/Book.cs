using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Domain.Entities
{
    public class Book
    {
        [Key]
        public string BookId { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string PublisherId { get; set; } = string.Empty;

        [ForeignKey("PublisherId")]
        public virtual Publisher? Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required]
        public string Genre { get; set; } = string.Empty;

        public string? FileUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
    }
}