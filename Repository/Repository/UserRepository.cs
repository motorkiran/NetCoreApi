public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public void DisableUser(int id)
    {
        var updateQuery = $"{UserQuery.DisableUser}{id}";
        ExecuteScript(updateQuery);
    }
}