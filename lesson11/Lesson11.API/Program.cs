using Lesson11.Application;
using Lesson11.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
//Entity Framework Core (ORM) profesyonel bir araçtır ve "Bana şifreyi sınıfların içine gömme!" der.
//Eski sistemimizi (Ders 8) hatırla: Şifreyi ve bağlantı adresini (Connection String) amele gibi gidip PostgresUserRepository.cs dosyasının içine (Infrastructure katmanına) yazmıştık. 
//Orada her seferinde manuel new NpgsqlConnection(şifre) diyorduk.

// 1. EF Core'un kalbi olan DbContext'i sisteme tanıtıyoruz. (Burada Postgres kullan diyoruz)
var connectionString = "Host=localhost;Port=5441;Username=postgres;Password=mysecretpassword;Database=postgres";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. DEPENDENCY INJECTION (DI) AYARLARI
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddScoped<UserService>();

// 3. AUTOMAPPER AYARI
// Yeni AutoMapper versiyonlarında Profil sınıflarımızı manuel olarak yapılandırma içine ekliyoruz:
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<Lesson11.Application.Mappings.AutoMapperProfile>();
});

var app = builder.Build();

// AUTO-MIGRATE: Java (Spring Boot)'daki hibernate.ddl-auto=update mantığı!
// Uygulama her ayağa kalktığında veritabanına bakar, tablo yoksa kendi kendine oluşturur.
// Sektörde küçük/orta ölçekli projelerde o terminal kodunu elle yazmamak için bu kod kullanılır.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers(); 
app.Run();
