using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Base
{
    public class BaseEntity : IEntity
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedById { get; set; }
        public virtual User UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
