public interface IUserService
{
    public User Get(int id);
    public List<User> Get();
    public ResultObjectDto CreateUser(CreateUserDto dto);
    public List<UserListDto> GetUserDtoList();
    public ResultObjectDto GetUserByEmailAndPassword(string email, string password);
}