using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [Authorize(Policy = "SystemAdmin")]
    [HttpGet("all")]   
    public IActionResult GetAllEmployees()
    {
        var users = HttpContext.Items["CompanyUsers"] as IEnumerable<User>;
        if (users == null)
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [Authorize(Policy = "CompanyAdminOrSystemAdmin")]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var user = HttpContext.Items["UserProfile"] as User;
        if (user == null)
        {
            return NotFound("User profile not found.");
        }
        return Ok(user);
    }

    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [Authorize(Policy = "Employee")]
    [HttpGet("employee-profile")]
    public IActionResult GetEmployeeProfile()
    {
        var user = HttpContext.Items["UserProfile"] as User;
        if (user == null)
        {
            return NotFound("Employee profile not found.");
        }
        return Ok(user);
    }

    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [Authorize(Policy = "CompanyAdminOrSystemAdmin")]
    [HttpGet("getuserbyid")]
    public IActionResult GetUserById([FromQuery] string companyName)
    {
        var userIdFromMiddleware = HttpContext.Items["UserId"] as string;
        var user = HttpContext.Items["User"] as User;

        return Ok(new { userIdFromMiddleware, user });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] UserRegister userRegister)
    {
        var result = await _authService.RegisterAsync(userRegister);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] UserLogin userLogin)
    {
        var token = await _authService.LoginAsync(userLogin);
        return Ok(new { token });
    }
}
