using AutoMapper;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogService _logService;
    private readonly IServiceBase _serviceBase;
    private readonly IJWTService _jwtService;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogService logService, IServiceBase serviceBase, IJWTService jWTService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logService = logService;
        _serviceBase = serviceBase;
        _jwtService = jWTService;
    }

    public User Get(int id)
    {
        var user = _userRepository.Get(id);

        return user;
    }

    public ResultObjectDto CreateUser(CreateUserDto dto)
    {
        var user = new User();

        user = _mapper.Map<User>(dto);

        _serviceBase.BindBaseProperty(user);
        user.Password = Tools.HashedPassword(user.Password);
        var result = _userRepository.Insert(user);

        return result;
    }

    public List<User> Get()
    {
        var userList = _userRepository.Get().ToList();

        return userList;
    }

    public List<UserListDto> GetUserDtoList()
    {
        var dtoList = new List<UserListDto>();
        var userList = _userRepository.Get();

        foreach (var item in userList)
        {
            var dto = _mapper.Map<UserListDto>(item);
            dtoList.Add(dto);
        }

        return dtoList;
    }

    public ResultObjectDto GetUserByEmailAndPassword(string email, string password)
    {
        var result = new ResultObjectDto();

        var user = _userRepository.GetUserByEmail(email);

        if (user != null)
        {
            if (Tools.VerifPassword(password, user.Password))
            {
                result.IsSuccess = true;
                result.Result = user;
                return result;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Wrong password";

                _logService.Error("Wrong password");
            }
        }
        {
            result.IsSuccess = false;
            result.Message = "User not found";

            _logService.Error("User not found");
        }

        return result;
    }
}