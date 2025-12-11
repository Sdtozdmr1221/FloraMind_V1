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

        public DateTime RegistrationDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } // şifre hash'i

        public string Role { get; set; } // kullanıcı rolü

        public bool IsBanned { get; set; } // ban durumu


        //ilişkiler

        [ValidateNever]
        public ICollection<UserPlant> UserPlants { get; set; } // kullanıcı-bitki ilişkisi
        public ICollection<Plant> Plants { get; set; } // kullanıcı bitkileri
        public ICollection<Content> Contents { get; set; } // kullanıcı içerikleri



    }
}
