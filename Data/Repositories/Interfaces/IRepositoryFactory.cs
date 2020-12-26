namespace Data.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IUserRepository Users { get; }
        void SaveChanges();
    }
}
