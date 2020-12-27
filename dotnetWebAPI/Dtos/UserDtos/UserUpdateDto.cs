using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.UserDtos
{
    public class UserUpdateDto : UserBaseDto
    {
        [Required]
        public int? Id { get; set; }
    }
}
