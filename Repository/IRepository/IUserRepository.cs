public interface IUserRepository : IGenericRepository<User>
{
    User GetUserByEmail(string email);
}