using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP_Backend.Data.Models
{
    public enum UserRole
    {
        REGULAR,
        MANAGER,
        ADMIN
    }

    [Index(nameof(Email), IsUnique = true)]
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column("email")]
        [Required]
        public string Email { get; set; } = null!;

        [Column("password")]
        [Required]
        public string Password { get; set; } = null!;

        [Column("userRole")]
        [Required]
        public UserRole Role { get; set; }
    }
}
