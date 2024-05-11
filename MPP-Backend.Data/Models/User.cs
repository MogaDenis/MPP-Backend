using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP_Backend.Data.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("email")]
        [Required]
        public string Email { get; set; } = null!;

        [Column("password")]
        [Required]
        public string Password { get; set; } = null!;
    }
}
