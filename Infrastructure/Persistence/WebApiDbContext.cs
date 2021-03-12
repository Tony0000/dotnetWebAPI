using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class WebApiDbContext : DbContext, IWebApiDbContext
    {
        private readonly IHttpContextAccessor _accessor;
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options,
            IHttpContextAccessor accessor) : base(options)
        {
            _accessor = accessor;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebApiDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var currentUser = GetUser();
            var currentUserId = currentUser?.Id;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedById = currentUserId;
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(AuditableEntity.CreatedById)).IsModified = false;
                        entry.Property(nameof(AuditableEntity.CreatedAt)).IsModified = false;

                        entry.Entity.UpdatedById = currentUserId;
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private User GetUser()
        {
            var username = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Users.FirstOrDefault(u => u.Username == username) ?? Users.FirstOrDefault();
        }
    }
}
