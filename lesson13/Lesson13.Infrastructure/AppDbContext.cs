using Microsoft.EntityFrameworkCore;
using Lesson13.Domain;

namespace Lesson13.Infrastructure;

// BÖLÜM 3: INFRASTRUCTURE (ALTYAPI / VERİTABANI)
// İşte EF Core'un kalbi! DbContext sınıfından miras alarak bunun bir "Veritabanı Yöneticisi" olduğunu belirtiyoruz.
public class AppDbContext : DbContext
{
    // C#'taki User sınıfımızın (Entity), veritabanında "Users" adlı bir tabloya denk geleceğini söylüyoruz.
    public DbSet<User> Users { get; set; }

    // constructor: Dışarıdan bağlantı ayarlarını (Options) alıp ana sınıfa (base) gönderir.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Veritabanı tabloları oluşurken araya girip ekstra kurallar koyduğumuz yer (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Veritabanı Kuralları (Database Constraints):
        // E-posta adresi veritabanında "Benzersiz (Unique)" olmalıdır. 
        // Böylece aynı e-posta ile ikinci bir kişi kayıt olmaya kalkarsa veritabanı hata fırlatır.
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
