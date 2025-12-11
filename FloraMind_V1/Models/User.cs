using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FloraMind_V1.Models
{
    public class User // kullanıcı sınıfı

    {
        [Key]
        public int UserID { get; set; } // kullanıcı ID'si

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // isim

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } // email

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } // şifre hash'i



        //ilişkiler

        [ValidateNever]
        public ICollection<UserPlant> UserPlants { get; set; } // kullanıcı-bitki ilişkisi
        public ICollection<Plant> Plants { get; set; } // kullanıcı bitkileri
        public ICollection<Content> Contents { get; set; } // kullanıcı içerikleri



    }
}
