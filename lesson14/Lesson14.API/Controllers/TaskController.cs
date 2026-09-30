using Microsoft.AspNetCore.Mvc;
using Lesson14.Application.DTOs;
using Lesson14.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Lesson14.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // DİKKAT: Bu sınıfa biletsiz (JWT'siz) kimse giremez!
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }

    // Sisteme giren kullanıcının ID'sini Token'dan okuyan yardımcı bir metod
    private int GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
            throw new Exception("Kullanıcı kimliği bulunamadı, yeniden giriş yapın.");
        return int.Parse(userIdString);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
    {
        var userId = GetCurrentUserId(); // Biletten (JWT) adamın kim olduğunu anladık
        var result = await _taskService.CreateTaskAsync(dto, userId); // Sadece ona ait görev oluşturduk
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyTasks()
    {
        var userId = GetCurrentUserId();
        var tasks = await _taskService.GetMyTasksAsync(userId); // Sadece kendi görevlerini listeledik
        return Ok(tasks);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetCurrentUserId();
        await _taskService.DeleteTaskAsync(id, userId); // Kendi görevi değilse AuthService Exception fırlatacak
        return Ok(new { message = "Görev başarıyla silindi!" });
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> ToggleTask(int id)
    {
        var userId = GetCurrentUserId();
        var result = await _taskService.ToggleTaskCompletionAsync(id, userId);
        return Ok(result);
    }
}
