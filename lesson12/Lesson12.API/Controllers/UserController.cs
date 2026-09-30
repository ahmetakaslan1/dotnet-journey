using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Lesson12.Application;
using Lesson12.Application.DTOs;

namespace Lesson12.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    // API endpoint'leri de artık async Task. Garson (Sunucu Thread'i) burada serbest kalacak!
    // IActionResult .NET dünyasında HTTP Cevap Paketinin Evrensel Arayüzüdür (Interface)
    public async Task<IActionResult> Create([FromBody] UserCreateDto newUserDto)
    {
        await _userService.RegisterUserAsync(newUserDto);
        return Ok("Kullanıcı başarıyla kaydedildi.");
    }

    [HttpGet]
    // GET endpoint'ini [Authorize] ile korumaya alıyoruz (Sadece VIP kartı - JWT - olanlar görebilir)
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> GetAll()
    {
        var dtoList = await _userService.GetAllUsersAsync();
        return Ok(dtoList);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto);
        
        if (token == null)
        {
            return Unauthorized("E-posta veya şifre hatalı!"); // 401 Unauthorized
        }

        // Başarılı ise JWT token'ı geriye dönüyoruz
        return Ok(new { Token = token, Message = "Giriş Başarılı!" });
    }
}
