using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace FloraMind_V1.Models
{
    public class RegisterViewModel // kayıt ol
    {
        [Required]
        public string UserName { get; set; }


        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifreler Uyuşmuyor!")] // şifre kontrol
        public string CheckPassword { get; set; }
    }
}
