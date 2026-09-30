using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Lesson13.API.Middlewares;

// Bütün uygulamanın etrafını saran bir güvenlik süzgeci (Middleware)
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // İsteği sonraki aşamalara (Controller'a) gönder
            await _next(context);
        }
        catch (Exception ex)
        {
            // Eğer içeride (Service, Repository vs.) herhangi bir hata fırlatılırsa (throw new Exception),
            // proje çökmek yerine buraya düşer! Biz de hatayı yakalayıp düzgün bir JSON olarak geri döneriz.
            _logger.LogError($"Bir hata oluştu: {ex.Message}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        // Şimdilik bütün genel hataları 400 Bad Request olarak dönelim. 
        // İleride özel "NotFoundException", "ValidationException" sınıfları yapıp durum kodunu (404, 500) değiştirebiliriz.
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var result = JsonSerializer.Serialize(new
        {
            StatusCode = context.Response.StatusCode,
            ErrorMessage = exception.Message,
            Detail = "Bu mesaj Global Exception Middleware tarafından yakalandı!"
        });

        return context.Response.WriteAsync(result);
    }
}
