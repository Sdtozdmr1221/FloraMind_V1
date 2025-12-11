using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace FloraMind_V1.Models
{
    public class RegisterViewModel // kayıt ol
    {
        [Required]
        public string UserName { get; set; } // kullanıcı adı


        [Required]
        public string Email { get; set; } // email


        [Required]
        public string Password { get; set; } // şifre

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Şifreler Uyuşmuyor!")]
        public string CheckPassword { get; set; } // şifre kontrol
    }
}
