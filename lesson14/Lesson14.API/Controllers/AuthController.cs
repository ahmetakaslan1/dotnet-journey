using Microsoft.AspNetCore.Mvc;
using Lesson14.Application.DTOs;
using Lesson14.Application.Services;

namespace Lesson14.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost(" ")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var message = await _authService.RegisterAsync(dto);
        return Ok(new { message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok(new { token });
    }
}
