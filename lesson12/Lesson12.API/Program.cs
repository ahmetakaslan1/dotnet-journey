using Lesson12.Application;
using Lesson12.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
//Entity Framework Core (ORM) profesyonel bir araçtır ve "Bana şifreyi sınıfların içine gömme!" der.
//Eski sistemimizi (Ders 8) hatırla: Şifreyi ve bağlantı adresini (Connection String) amele gibi gidip PostgresUserRepository.cs dosyasının içine (Infrastructure katmanına) yazmıştık. 
//Orada her seferinde manuel new NpgsqlConnection(şifre) diyorduk.

// 1. EF Core'un kalbi olan DbContext'i sisteme tanıtıyoruz. (Burada Postgres kullan diyoruz)
var connectionString = "Host=localhost;Port=5442;Username=postgres;Password=mysecretpassword;Database=postgres";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. DEPENDENCY INJECTION (DI) AYARLARI
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

// 3. AUTOMAPPER AYARI
// Yeni AutoMapper versiyonlarında Profil sınıflarımızı manuel olarak yapılandırma içine ekliyoruz:
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<Lesson12.Application.Mappings.AutoMapperProfile>();
});

// 4. GÜVENLİK (JWT AUTHENTICATION) AYARI
// Sisteme "Benim güvenliğim JWT ile sağlanıyor" diyoruz.
var key = System.Text.Encoding.UTF8.GetBytes("BenimCokGizliVeGuvenliSirketAnahtarim12345!");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // İmza doğru mu?
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "Lesson12API", // Bileti veren biz miyiz?
        ValidateAudience = true,
        ValidAudience = "Lesson12Users", // Bilet bizim kullanıcılarımız için mi?
        ValidateLifetime = true // Süresi dolmuş mu?
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// AUTO-MIGRATE: Java (Spring Boot)'daki hibernate.ddl-auto=update mantığı!
// Uygulama her ayağa kalktığında veritabanına bakar, tablo yoksa kendi kendine oluşturur.
// Sektörde küçük/orta ölçekli projelerde o terminal kodunu elle yazmamak için bu kod kullanılır.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// 5. GÜVENLİK KAPISI (MIDDLEWARE)
// Uygulamaya gelen istekler önce bu kapıdan geçer:
// a) "Bu kişinin VIP kartı (JWT) var mı?" diye sorar (Authentication)
app.UseAuthentication();
// b) "Peki bu kartın bu odaya girmeye yetkisi var mı?" diye sorar (Authorization)
app.UseAuthorization();

app.MapControllers(); 
app.Run();
