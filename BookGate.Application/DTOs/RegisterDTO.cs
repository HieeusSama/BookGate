using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookGate.Application.DTOs
{
    public enum UserRole
    {
        Admin,
        Member
    }
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng.")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string Password { get; set; } = string.Empty;

        [Phone]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đầy đủ.")]
        public string? FullName { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;
    }
}
