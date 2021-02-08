using System;
using Domain.Entities;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedById { get; set; }
        public virtual User UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
