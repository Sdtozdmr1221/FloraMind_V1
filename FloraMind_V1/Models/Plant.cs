using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        public DateTime? DateAdded { get; set; } = DateTime.UtcNow; //Bitkinin Eklenme tarihi

        [ForeignKey("User")]
        public int UserID { get; set; } //Bitki Sahibi Kullanıcı ID'si
        public User User { get; set; } //Bitki Sahibi Kullanıcı Nesnesi


        //ilişkiler

        public ICollection<Content> Contents { get; set; } //Bitkiye Ait İçerikler



    }
}
