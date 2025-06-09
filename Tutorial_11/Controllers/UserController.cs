using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tutorial_11.DTO;
using Tutorial_11.Services.Interfaces;

namespace Tutorial_11.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    public IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("/login")]
    public async Task<ActionResult> LoginAsync(LoginDTO user)
    {
        var result = await _userService.LoginAsync(user.Email, user.Password);
        
        if (result == null)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(new
        {
            accessToken = result.Value.accessToken,
            refreshToken = result.Value.refreshToken
        });

    }

    [HttpPost("/register")]
    public async Task<ActionResult> RegisterAsync(RegisterUserDTO user)
    {
        var result = await _userService.RegisterAsync(user.Username, user.Password);
        
        if (!result)
            return BadRequest(new { message = "User with this email already exists" });

        return Ok(new { message = "User registered successfully" });
    }

    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _userService.RefreshTokenAsync(request.refreshToken);

        if (result == null)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        return Ok(new
        {
            accessToken = result.Value.accessToken,
            refreshToken = result.Value.refreshToken
        });
    }
}