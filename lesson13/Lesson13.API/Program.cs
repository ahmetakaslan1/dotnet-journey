using Lesson13.Application;
using Lesson13.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. .ENV DOSYASINI YÜKLE
// Proje ana dizinindeki (lesson13) .env dosyasını bulup içindeki değişkenleri sisteme entegre eder.
DotNetEnv.Env.Load("../.env");
// Çevre değişkenlerini de IConfiguration'a dahil et (Böylece IConfiguration üzerinden okuyabileceğiz)
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers(); 
builder.Services.AddCors();

// FLUENT VALIDATION AYARI: Projedeki kurallarımızı (AbstractValidator sınıflarını) otomatik bulur ve uygular.
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(Lesson13.Application.Validations.UserCreateDtoValidator).Assembly);

// 2. EF Core'un kalbi olan DbContext'i sisteme tanıtıyoruz. (Burada Postgres kullan diyoruz)
// Değerleri artık güvenli bir şekilde .env'den okuyoruz!
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5443";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "mysecretpassword";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";

var connectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPass};Database={dbName}";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. DEPENDENCY INJECTION (DI) AYARLARI
// Mysql olmasını istiyorsak: builder.Services.AddScoped<IUserRepository, MysqlUserRepository>();
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

// 4. AUTOMAPPER AYARI
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<Lesson13.Application.Mappings.AutoMapperProfile>();
});

// 5. GÜVENLİK (JWT AUTHENTICATION) AYARI
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (string.IsNullOrEmpty(jwtSecret))
{
    throw new Exception("JWT_SECRET .env dosyasında bulunamadı!");
}

var key = System.Text.Encoding.UTF8.GetBytes(jwtSecret);
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
        ValidIssuer = "Lesson13API", // Bileti veren biz miyiz?
        ValidateAudience = true,
        ValidAudience = "Lesson13Users", // Bilet bizim kullanıcılarımız için mi?
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

// CORS AYARI (Ayrık Mimari İçin Çok Önemli!)
// Frontend (arayüz) farklı bir yerden (veya dosyadan) çalıştığı için tarayıcı "Güvenlik İhlali" diyerek engeller.
// Bu ayar sayesinde API'miz, dışarıdan gelen (Frontend) isteklere kapıyı açar.
app.UseCors(policy => 
    policy.AllowAnyOrigin() // Herhangi bir yerden gelebilir
          .AllowAnyHeader() // Herhangi bir başlık (header) gönderebilir
          .AllowAnyMethod() // GET, POST, PUT, DELETE her şeyi yapabilir
);

// 5. GÜVENLİK KAPISI (MIDDLEWARE) VE HATA YAKALAYICI (EXCEPTION HANDLING)
// Projedeki her hata (Exception) önce bizim süzgecimize takılacak.
app.UseMiddleware<Lesson13.API.Middlewares.GlobalExceptionMiddleware>();

// Uygulamaya gelen istekler önce bu kapıdan geçer:
// a) "Bu kişinin VIP kartı (JWT) var mı?" diye sorar (Authentication)
app.UseAuthentication();
// b) "Peki bu kartın bu odaya girmeye yetkisi var mı?" diye sorar (Authorization)
app.UseAuthorization();

app.MapControllers(); 
app.Run();
