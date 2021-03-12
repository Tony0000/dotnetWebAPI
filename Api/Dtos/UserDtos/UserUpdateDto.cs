using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.UserDtos
{
    public class UserUpdateDto : UserBaseDto
    {
        [Required]
        public int? Id { get; set; }
    }
}
