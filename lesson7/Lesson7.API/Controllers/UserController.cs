using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lesson7.Domain;
using Lesson7.Application;

// BİLMEN GEREKEN EN ÖNEMLİ ŞEY:
// Eğer buraya "using Lesson7.Infrastructure;" yazmaya kalkarsan C# KIZAR. HATA VERİR.
// Çünkü API projesinin, Infrastructure projesini "görme" yetkisi (referansı) yoktur.
// İşte "Derleme Zamanı Güvenliği" tam olarak budur!

namespace Lesson7.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    // Sadece Application projesindeki UserService'i tanır.
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        var users = _userService.GetAllUsers();
        return Ok(users); 
    }

    [HttpPost]
    public ActionResult Post([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("İsim boş olamaz!"); 
        }

        _userService.RegisterUser(name);
        return Ok($"'{name}' başarıyla eklendi!"); 
    }
}
