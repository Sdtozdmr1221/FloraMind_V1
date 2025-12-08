using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FloraMind_V1.Models
{
    
    public class Plant  //Bitki Sınıfı
    {
        [Key]
        public int PlantID { get; set; }


        [Required]
        [MaxLength(100)]
        public string Name { get; set; }


        [MaxLength(100)]
        public string Species { get; set; } //Bitki Türü


        public DateTime DateAdded { get; set; } = DateTime.UtcNow; //Bitkinin Eklenme tarihi
        
        [ValidateNever]
        public User User { get; set; } //Bitki Sahibi Kullanıcı Nesnesi

        public int? UserID { get; set; }



        //ilişkiler
        [ValidateNever]
        public ICollection<Content> Contents { get; set; } //Bitkiye Ait İçerikler

        [ValidateNever]
        public ICollection<UserPlant> UserPlants { get; set; } //Kullanıcı-Bitki İlişkisi



    }
}
