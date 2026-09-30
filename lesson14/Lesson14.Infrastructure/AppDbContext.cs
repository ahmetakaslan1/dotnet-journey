using Microsoft.EntityFrameworkCore;
using Lesson14.Domain;

namespace Lesson14.Infrastructure;

public class AppDbContext : DbContext
{
    // TABLOLARIMIZ
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // KURALLAR VE İLİŞKİLER (One-To-Many)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Kural: E-posta adresi benzersiz olsun
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // 2. Kural: İLİŞKİSEL VERİTABANI (Bir Kullanıcının Birden Fazla Görevi Vardır)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)           // User'ın birden fazla Task'i var
            .WithOne(t => t.User)            // Her Task'in bir User'ı var
            .HasForeignKey(t => t.UserId)    // Yabancı Anahtar (Foreign Key) UserId kolonudur
            .OnDelete(DeleteBehavior.Cascade); // Eğer kullanıcı silinirse, ona ait tüm görevler de (Tasks) silinsin!
    }
}
