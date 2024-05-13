﻿using MPP_Backend.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace MPP_Backend.Business.Models
{
    public class OwnerModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a first name.")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must provide a last name.")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
    }
}
