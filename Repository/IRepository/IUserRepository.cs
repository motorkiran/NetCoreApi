public interface IUserRepository : IGenericRepository<User>
{
    void DisableUser(int id);
}