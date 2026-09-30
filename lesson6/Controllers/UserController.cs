using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using lesson6.Models;
using lesson6.Services;

namespace lesson6.Controllers;

[ApiController]       // Bu sınıfın bir Web API olduğunu sisteme söyler.
[Route("api/users")]  // İnternet tarayıcısından "http://localhost:5000/api/users" yazılınca buraya gelineceğini belirtir.
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    // MÜKEMMEL MİMARİ:
    // Biz buraya asla "new UserService()" YAZMIYORUZ.
    // ASP.NET Core sistemi başlarken (Program.cs'deki ayarlarımızdan) UserService'i bulacak
    // ve Controller yaratılırken (birisi web sitesine girince) bunu OTOMATİK enjekte edecek!
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    // İnternet tarayıcısından bu adrese normal girildiğinde bu metod çalışır.
    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        var users = _userService.GetAllUsers();
        // Veritabanındaki listeyi alıp JSON formatına (Javascript Object Notation) çevirerek ekrana basar.
        return Ok(users); 
    }

 [HttpGet("a")]
    public ActionResult<List<User>> Get2()
    {
        var users = _userService.GetAllUsers();
        // Veritabanındaki listeyi alıp JSON formatına (Javascript Object Notation) çevirerek ekrana basar.
        return Ok(users); 
    }
    // POST: api/users?name=Ahmet
    // Sisteme dışarıdan veri kaydetmek için kullanılır.
    [HttpPost]
    public ActionResult Post([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("İsim boş olamaz!"); // Hata 400
        }

        _userService.RegisterUser(name);
        return Ok($"'{name}' başarıyla PostgreSQL veritabanına eklendi!"); // Başarılı 200
    }
}
