using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ITOS_LEARN.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Username must be between 1 and 50 characters.", MinimumLength = 1)]  // กำหนดความยาวระหว่าง 1 ถึง 50 ตัวอักษร
        public required string Username { get; set; }
        public DateTime LoginTime { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "IpAddress must be between 1 and 50 characters.", MinimumLength = 1)]  // กำหนดความยาวระหว่าง 1 ถึง 50 ตัวอักษร
        public required string IpAddress { get; set; }
    }
}
