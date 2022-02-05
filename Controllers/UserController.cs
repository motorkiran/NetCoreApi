using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static Enums;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IRedisService _redisService;
    public UserController(IUserService userService, ILogger<UserController> logger, IMemoryCache memoryCache, IRedisService redisService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _logger = logger;
        _memoryCache = memoryCache;
        _redisService = redisService;
    }

    [HttpGet("{id}")]
    public ObjectResult Get(int id)
    {
        return Ok(_userService.Get(id));
    }

    [ServiceFilter(typeof(PermissonFilter))]
    [SecurityActionAttribute((int)UserControllerPermission.User, (int)UserPermissionAction.GetUserPermissionsById)]
    [Route("Get")]
    [HttpGet]
    public ObjectResult Get()
    {
        if (!_memoryCache.TryGetValue(Constants.UserList, out List<User> userList))
        {
            userList = _userService.Get();

            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromDays(10)
            };

            _memoryCache.Set(Constants.UserList, userList, cacheExpiryOptions);
        }

        return Ok(userList);
    }

    [Route("GetUserDtoList")]
    [HttpGet]
    public ObjectResult GetUserDtoList()
    {
        if (_redisService.IsSet(Constants.UserDtoList))
        {
            return Ok(_redisService.Get<List<UserListDto>>(Constants.UserDtoList));
        }
        else
        {
            var userdtoList = _userService.GetUserDtoList();
            _redisService.Set(Constants.UserDtoList, userdtoList);
            return Ok(userdtoList);
        }
    }
}