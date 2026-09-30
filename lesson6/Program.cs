using lesson6.Repositories;
using lesson6.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// DEPENDENCY INJECTION (IoC) AYARLARI
// Konsolda elle yazdığımız "ServiceCollection" yapısının Web API'deki tam karşılığı burasıdır.
// Web projelerinde Microsoft bunu bizim için hazır getirir (builder.Services).
// =========================================================

builder.Services.AddControllers(); // Controller sınıflarını (UserController) aktif et.

// Kendi yazdığımız Interface, Repository ve Servisleri sisteme tanıtıyoruz:
// Artık kim IUserRepository isterse ona Postgres olanı verilecek!
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

// =========================================================

var app = builder.Build();

app.MapControllers(); // Gelen web isteklerini doğru Controller'lara (api/users gibi) yönlendir.

// Konsol uygulaması değil, 7/24 çalışacak bir Web Sunucusu başlatıyoruz:
app.Run();
