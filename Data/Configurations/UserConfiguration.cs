using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.Username)
                .IsUnique();
                
            builder.HasIndex(e => e.Email)
                .IsUnique();
            
            builder.Property(e => e.Username)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(e => e.Password)
                .IsRequired();

            builder.HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById);

            builder.HasOne(u => u.UpdatedBy)
                .WithMany()
                .HasForeignKey(u => u.UpdatedById);
        }
    }
}
