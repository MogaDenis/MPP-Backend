using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPP_Backend.Data.Models
{
    [Table("Car")]
    public class Car
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("make")]
        [StringLength(50)]
        public string Make { get; set; } = null!;

        [Column("model")]
        [StringLength(50)]
        public string Model { get; set; } = null!;

        [Column("colour")]
        [StringLength(50)]
        public string Colour { get; set; } = null!;

        [Column("imageUrl")]
        public string ImageUrl { get; set; } = null!;

        [Column("ownerId")]
        [StringLength(50)]
        public int OwnerId { get; set; }

        //[ForeignKey("OwnerId")]
        //[InverseProperty("Cars")]
        //public virtual Owner Owner { get; set; } = null!;
    }
}
