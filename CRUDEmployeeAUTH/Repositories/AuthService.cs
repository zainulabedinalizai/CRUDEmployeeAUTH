using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public AuthService(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public async Task<string> RegisterAsync(UserRegister userRegister)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(userRegister.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var company = await _userRepository.GetCompanyByNameAsync(userRegister.CompanyName);
        if (company == null)
        {
            company = new Company
            {
                CompanyName = userRegister.CompanyName
            };
            await _userRepository.AddCompanyAsync(company);
        }

        var user = new User
        {
            Email = userRegister.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password),
            Role = userRegister.Role,
            CompanyId = company.CompanyId
        };

        await _userRepository.AddAsync(user);
        return "User registered successfully.";
    }

    public async Task<string> LoginAsync(UserLogin userLogin)
    {
        var user = await _userRepository.GetByUsernameAsync(userLogin.Username);
        if (user != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
        {
            return GenerateToken(user);
        }

        throw new UnauthorizedAccessException("Invalid credentials.");
    }

    private string GenerateToken(User user)
    {
        var key = _config["Jwt:Key"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("CompanyId", user.CompanyId.ToString())
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:Expires"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
