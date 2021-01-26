using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class LoginModel
    {
        [MinLength(6), MaxLength(20), Required]
        public string Username { get; set; }
        [MinLength(6), MaxLength(20), Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
