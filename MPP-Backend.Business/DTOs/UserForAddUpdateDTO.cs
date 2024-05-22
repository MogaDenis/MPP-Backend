using MPP_Backend.Data.Models;

namespace MPP_Backend.Business.DTOs
{
    public class UserForAddUpdateDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
