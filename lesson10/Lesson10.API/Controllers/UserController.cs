using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Lesson10.Application;
using Lesson10.Application.DTOs;

namespace Lesson10.API.Controllers;

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
    public async Task<IActionResult> Create([FromBody] UserCreateDto newUserDto)
    {
        await _userService.RegisterUserAsync(newUserDto);
        return Ok("Kullanıcı başarıyla kaydedildi.");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dtoList = await _userService.GetAllUsersAsync();
        return Ok(dtoList);
    }
}
