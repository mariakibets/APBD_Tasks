namespace Tutorial_11.Services.Interfaces;

public interface IUserService
{
    Task<bool> RegisterAsync(string username, string password);
    Task<(string accessToken, string refreshToken)?> LoginAsync(string username, string password);

    Task<(string accessToken, string refreshToken)?> RefreshTokenAsync(string token);
}