using System.ComponentModel.DataAnnotations;


namespace dotnetWebAPI.Dtos.UserDtos
{
    public abstract class UserBaseDto
    {
        [MinLength(6), MaxLength(20), Required]
        public string Username { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        public bool Active { get; set; } = true;
    }
}
