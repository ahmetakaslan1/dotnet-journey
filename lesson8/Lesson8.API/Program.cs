using Lesson8.Application;
using Lesson8.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 

// DEPENDENCY INJECTION (DI) AYARLARI
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();
app.MapControllers(); 
app.Run();
