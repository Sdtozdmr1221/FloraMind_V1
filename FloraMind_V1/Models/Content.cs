using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloraMind_V1.Models
{
    
    public class Content  //İçerik Sınıfı
    {

        [Key]
        public int ContentID { get; set; }


        [Required]
        [MaxLength(100)]
        public string Title { get; set; }


        [Required]
        public string Body { get; set; } //İçerik Metni


        public DateTime? DateCreated { get; set; } = DateTime.UtcNow; //İçeriğin Oluşturulma Tarihi

        //foreign keys
        public int UserID { get; set; } //İçeriğin Kullanıcı ID'si
        public User User { get; set; } //İçeriğin Kullanıcı Nesnesi


        [ForeignKey("Plant")]
        public int PlantID { get; set; } //İçeriğin Bitki ID'si
        public Plant Plant { get; set; } //İçeriğin Bitki Nesnesi
    }
}
