using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FloraMind_V1.Models
{
    public class User // kullanıcı sınıfı

    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } // şifre hash'i



        //ilişkiler

        [ValidateNever]
        public ICollection<UserPlant> UserPlants { get; set; }
        public ICollection<Plant> Plants { get; set; }
        public ICollection<Content> Contents { get; set; }



    }
}
