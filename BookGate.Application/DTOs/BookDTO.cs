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

        [Required(ErrorMessage = "Vui lòng nhập tên sách.")]
        [StringLength(200, ErrorMessage = "Tên sách không được vượt quá 200 ký tự.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên tác giả.")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số lượng.")]
        [Range(1, 10000, ErrorMessage = "Số lượng phải lớn hơn 0 và nhỏ hơn 10,000.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn nhà xuất bản.")]
        public string PublisherId { get; set; } = string.Empty;

        public string PublisherName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn ngày xuất bản.")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thể loại.")]
        public string Genre { get; set; } = string.Empty;

        public string? FileUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá nhập.")]
        [Range(1000, 100000000, ErrorMessage = "Giá nhập phải từ 1,000 đến 100,000,000 VNĐ.")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá bán.")]
        [Range(1000, 100000000, ErrorMessage = "Giá bán phải từ 1,000 đến 100,000,000 VNĐ.")]
        public decimal SellingPrice { get; set; }
    }
}
