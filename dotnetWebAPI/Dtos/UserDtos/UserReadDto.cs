using System;

namespace dotnetWebAPI.Dtos.UserDtos
{
    public class UserReadDto : UserBaseDto, IBaseReadDto
    {
        public int? CreatedById { get; set; }
        public UserAuditDto CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedById { get; set; }
        public UserAuditDto UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
