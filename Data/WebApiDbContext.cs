using Domain.Entities;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Http;

namespace Data
{
    public class WebApiDbContext : DbContext
    {
        //private IHttpContextAccessor _acessor;
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options
            /*IHttpContextAccessor accessor*/) : base(options)
        {
            //_acessor = accessor;
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
    }
}
