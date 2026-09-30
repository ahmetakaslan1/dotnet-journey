using Microsoft.AspNetCore.Mvc;
using Lesson8.Application;
using Lesson8.Application.DTOs;
using System.Collections.Generic;

namespace Lesson8.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    // Dependency Injection ile UserService'i alıyoruz
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        // Artık Controller'dan dışarıya User modelimiz asla çıkmıyor.
        // Doğrudan kuryeler (UserResponseDto) gidiyor.
        List<UserResponseDto> result = _userService.GetAllUsers();
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create([FromBody] UserCreateDto newUserDto)
    {
        // İnternetten gelen ham veri kuryesi (UserCreateDto), doğrudan servise yollanır.
        _userService.RegisterUser(newUserDto);
        return Ok("Kullanıcı DTO üzerinden başarıyla kaydedildi.");
    }
}
