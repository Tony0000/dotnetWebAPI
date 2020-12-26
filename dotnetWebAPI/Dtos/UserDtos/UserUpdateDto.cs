using System.ComponentModel.DataAnnotations;

namespace dotnetWebAPI.Dtos.UserDtos
{
    public class UserUpdateDto : UserBaseDto
    {
        [Required]
        public int? Id { get; set; }
    }
}
