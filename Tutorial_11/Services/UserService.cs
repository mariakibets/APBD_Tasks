using Microsoft.EntityFrameworkCore;
using Tutorial_11.DAL;
using Tutorial_11.Helpers;
using Tutorial_11.Models;
using Tutorial_11.Services.Interfaces;

namespace Tutorial_11.Services;

public class UserService : IUserService
{
    private readonly UserContext _context;
    private readonly AuthHelper _auth;

    public UserService(UserContext context, AuthHelper auth)
    {
        _context = context;
        _auth = auth;
    }

    public async Task<bool> RegisterAsync(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == username))
            return false;
    
        var (hash, salt) = _auth.HashPassword(password);
    
        var user = new User
        {
            Username = username,
            Email = username,
            PasswordHash = hash,
            PasswordSalt = salt
        };
    
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    public async Task<(string accessToken, string refreshToken)?> LoginAsync(string username, string password)
    {
        var user = await _context.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == username);
    
        if (user == null || !_auth.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            return null;
    
        var accessToken = _auth.CreateAccessToken(user);
        var refreshToken = _auth.GenerateRefreshToken();
    
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
        });
    
        await _context.SaveChangesAsync();
    
        return (accessToken, refreshToken);
    }
    
    public async Task<(string accessToken, string refreshToken)?> RefreshTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null ||  refreshToken.Expires < DateTime.UtcNow)
            return null; 
        
        var user = refreshToken.User;

        var newAccessToken = _auth.CreateAccessToken(user);
        var newRefreshToken = _auth.GenerateRefreshToken();
        
        _context.RefreshTokens.Remove(refreshToken);
        
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken ,
            Expires = DateTime.UtcNow.AddMinutes(20)
        });

        await _context.SaveChangesAsync();

        return (newAccessToken, newRefreshToken);
    }
}