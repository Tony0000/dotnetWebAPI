using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class LoginViewModel
    {
        [MinLength(6), MaxLength(20), Required]
        public string Username { get; set; }
        [MinLength(6), MaxLength(20), Required]
        public string Password { get; set; }

    }
}
