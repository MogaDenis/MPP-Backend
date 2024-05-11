using System.ComponentModel.DataAnnotations;

namespace MPP_Backend.Business.Models
{
    public class CarForAddUpdateModel
    {
        [Required(ErrorMessage = "You must provide a make for a car.")]
        [MaxLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must provide a model for a car.")]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must provide a colour for a car.")]
        [MaxLength(50)]
        public string Colour { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must provide an image URL for a car.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must provide an owner id for a car.")]
        public int OwnerId { get; set; }
    }
}
