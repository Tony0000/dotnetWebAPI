using System;
using System.Linq;
using System.Security.Claims;
using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Data
{
    public class WebApiDbContext : DbContext
    {
        private IHttpContextAccessor _accessor;
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options,
            IHttpContextAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UpdatedBy)
                .WithMany()
                .HasForeignKey(u => u.UpdatedById);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var currentUser = GetUser();
            var currentUserId = currentUser?.Id;
            var entities = ChangeTracker.Entries().Where(
                x =>
                    x.Entity is BaseEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in entities)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedById = currentUserId;
                    ((BaseEntity)entry.Entity).CreatedAt = DateTime.Now;
                }
                else
                {
                    entry.Property(nameof(BaseEntity.CreatedById)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;

                    ((BaseEntity)entry.Entity).UpdatedById = currentUserId;
                    ((BaseEntity)entry.Entity).UpdatedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        private User GetUser()
        {
            var username = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Users.FirstOrDefault(u => u.Username == username) ?? Users.FirstOrDefault();
        }
    }
}
