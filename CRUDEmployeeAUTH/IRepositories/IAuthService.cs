public interface IAuthService
{
    Task<string> RegisterAsync(UserRegister userRegister);
    Task<string> LoginAsync(UserLogin userLogin);
}
