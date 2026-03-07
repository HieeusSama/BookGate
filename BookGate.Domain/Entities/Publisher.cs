using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookGate.Domain.Entities
{
    public class Publisher
    {
        [Key]
        public string PublisherId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PublisherName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Address { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
