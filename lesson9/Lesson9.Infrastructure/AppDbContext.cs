using Microsoft.EntityFrameworkCore;
using Lesson9.Domain;

namespace Lesson9.Infrastructure;

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
}
