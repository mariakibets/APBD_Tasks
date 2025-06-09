using System.ComponentModel.DataAnnotations;

namespace Tutorial_11.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [MinLength(2),StringLength(50)]
    public string Username { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}