using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories
{
    public class UserRepository : DataRepository<User>, IUserRepository
    {
        private WebApiDbContext _context;

        public UserRepository(WebApiDbContext context) : base(context)
        {
            _context = context;
        }

        public override void Delete(User entity)
        {
            entity.Active = false;
        }
    }
}
