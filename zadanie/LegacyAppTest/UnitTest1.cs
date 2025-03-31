using LegacyApp;
using LegacyApp.interfaces;

namespace LegacyAppTest;

public class UserServiceTest
{
    private UserService _userService;

    public UserServiceTest()
    {
        _userService = new UserService();
    }

    [Fact]
    public void AddUser_ShouldReturnFalse_WhenInvalidName()
    {
        var result = _userService.AddUser("", "Doe", "johndoe@gmail.com", new DateTime(1982, 3, 21), 1);
        
        Assert.False(result);
    }
    
    
    [Fact]
    public void AddUser_InvalidEmail_ShouldReturnFalse()
    {
        
        var result = _userService.AddUser("John", "Doe", "invalid-email", DateTime.UtcNow.AddYears(-25), 1);
        Assert.False(result);
    }
    
    
    [Fact]
    public void AddUser_ValidUserWithLowCredit_ShouldReturnFalse()
    {
        var result = _userService.AddUser("John", "Doe", "john@example.com", DateTime.UtcNow.AddYears(-30), 2);
        Assert.False(result);
    }
    
    
    [Fact]
    public void AddUser_Underage_ShouldReturnFalse()
    {
        var result = _userService.AddUser("John", "Doe", "john@example.com", DateTime.UtcNow.AddYears(-18), 1);
        Assert.False(result);
    }
    
    
    [Fact]
    public void AddUser_ValidUserWithEnoughCredit_ShouldReturnTrue()
    {
        var result = _userService.AddUser("John", "Doe", "john@example.com", DateTime.UtcNow.AddYears(-30), 1);
        Assert.True(result);
    }

}