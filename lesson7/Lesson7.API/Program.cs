using Lesson7.Application;
using Lesson7.Infrastructure; // Sadece ve Sadece DI (Dependency Injection) ayarını yapabilmek için burası görebilir.
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 

// TÜM SİSTEMİN BAĞLANDIĞI YER
// Sistem burada der ki: "Kim benden IUserRepository isterse ona Postgres ver, 
// kim benden UserService isterse ona UserService ver."
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

app.MapControllers(); 

app.Run();
