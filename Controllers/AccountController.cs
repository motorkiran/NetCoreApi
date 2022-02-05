using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    private readonly IJWTService _jwtService;
    public AccountController(IUserService userService, IJWTService jwtService, ILogger<UserController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _jwtService = jwtService;
        _logger = logger;
    }

    [AllowAnonymous]
    [Route("CreateUser")]
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var result = _userService.CreateUser(createUserDto);

        if (result.IsSuccess)
        {
            return Ok(new
            {
                Message = result.Message
            });
        }
        else
        {
            return BadRequest(new
            {
                Message = result.Message
            });
        }
    }

    [Route("Authenticate")]
    [HttpPost]
    public IActionResult Authenticate([FromBody] LoginDto loginDto)
    {
        var authResult = _userService.GetUserByEmailAndPassword(loginDto.Email, loginDto.Password);

        if (!authResult.IsSuccess)
            return BadRequest(new { message = authResult.Message });

        var token = _jwtService.GetToken(loginDto);

        return Ok(new
        {
            Email = authResult.Result.Email,
            Token = token
        });
    }
}