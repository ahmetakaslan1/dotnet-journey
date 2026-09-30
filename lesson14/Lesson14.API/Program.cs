using Lesson14.Application.Repositories;
using Lesson14.Infrastructure;
using Lesson14.Infrastructure.Repositories;
using Lesson14.Application.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Lesson14.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. .ENV Dosyasını Bul ve Yükle (Sadece Local geliştirme için, Docker ortam değişkenlerini kendi verir)
if (System.IO.File.Exists("../.env"))
{
    DotNetEnv.Env.Load("../.env");
}
else if (System.IO.File.Exists(".env"))
{
    DotNetEnv.Env.Load(".env");
}
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddCors();

// 2. EF CORE & GENERIC REPOSITORY 
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "mysecretpassword";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "taskifydb";

var connectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPass};Database={dbName}";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Dependency Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();

// 3. AUTOMAPPER & FLUENT VALIDATION
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<Lesson14.Application.Mappings.AutoMapperProfile>();
});
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(Lesson14.Application.Validations.UserRegisterDtoValidator).Assembly);

// 4. JWT AUTHENTICATION
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (string.IsNullOrEmpty(jwtSecret)) throw new Exception("JWT_SECRET .env dosyasında bulunamadı!");

var key = System.Text.Encoding.UTF8.GetBytes(jwtSecret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// OTOMATİK VERİTABANI OLUŞTURMA (Docker için çok kritik!)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    int retries = 5;
    while (retries > 0)
    {
        try
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Veritabanı başarıyla oluşturuldu/güncellendi.");
            break; 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Veritabanına bağlanılamadı. Kalan deneme: {retries}. Hata: {ex.Message}");
            retries--;
            System.Threading.Thread.Sleep(3000); 
        }
    }
}

app.Run();
