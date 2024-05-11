using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP_Backend.Data.Models
{
    [Table("Owner")]
    public class Owner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [InverseProperty("Owner")]
        public virtual ICollection<Car> Cars { get; set; } = [];
    }
}
