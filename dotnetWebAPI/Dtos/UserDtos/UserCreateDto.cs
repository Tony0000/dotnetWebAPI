using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.UserDtos
{
    public class UserCreateDto : UserBaseDto
    {
        [Required, MinLength(6)]
        public string Password { get; set; }
    }
}
