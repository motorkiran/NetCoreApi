public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IConfiguration configuration, ILogService logService) : base(configuration, logService)
    {
    }

    public User GetUserByEmail(string email)
    {
        var searchQuery = string.Format(UserQuery.GetUserByEmail, email);
        var user = GetListByExpression(searchQuery).ToList().FirstOrDefault();

        return user;
    }
}