using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookGate.Application.DTOs
{
    public class BookDTO
    {
        [Key]
        public string BookId { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public string PublisherId { get; set; } = string.Empty;

        public string PublisherName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required]
        public string Genre { get; set; } = string.Empty;

        public string? FileUrl { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal SellingPrice { get; set; }
    }
}
