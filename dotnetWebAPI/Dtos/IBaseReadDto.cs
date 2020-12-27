using System;

namespace WebAPI.Dtos
{
    public interface IBaseReadDto
    {
        public int? CreatedById { get; set; }
        public UserAuditDto CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedById { get; set; }
        public UserAuditDto UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class UserAuditDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
