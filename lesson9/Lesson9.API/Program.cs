using Lesson9.Application;
using Lesson9.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
//Entity Framework Core (ORM) profesyonel bir araçtır ve "Bana şifreyi sınıfların içine gömme!" der.
//Eski sistemimizi (Ders 8) hatırla: Şifreyi ve bağlantı adresini (Connection String) amele gibi gidip PostgresUserRepository.cs dosyasının içine (Infrastructure katmanına) yazmıştık. Orada her seferinde manuel new NpgsqlConnection(şifre) diyorduk.

// 1. EF Core'un kalbi olan DbContext'i sisteme tanıtıyoruz. (Burada Postgres kullan diyoruz)
var connectionString = "Host=localhost;Port=5439;Username=postgres;Password=mysecretpassword;Database=postgres";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. DEPENDENCY INJECTION (DI) AYARLARI
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();
app.MapControllers(); 
app.Run();
