using System.ComponentModel.DataAnnotations;

namespace FloraMind_V1.Models
{
    public class LoginViewModel //Login Modeli
    {
        [Required]
        public string Email { get; set; } //login için email

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } //login için şifre
    }
}
