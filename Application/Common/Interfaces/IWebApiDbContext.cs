using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IWebApiDbContext
    {
        public DbSet<User> Users { get; set; }
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
