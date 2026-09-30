using System.Net;
using System.Text.Json;

namespace Lesson14.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorResponse = new
            {
                Error = "Sistem Hatası Yakalandı",
                Message = ex.Message
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
