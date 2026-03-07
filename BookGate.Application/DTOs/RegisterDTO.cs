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
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? FullName { get; set; }

        public UserRole Role { get; set; } = UserRole.Member;
    }
}
