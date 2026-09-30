using System.Collections.Generic;
using System.Linq;
using Lesson9.Domain;
using Lesson9.Application;

namespace Lesson9.Infrastructure;

// BÖLÜM 3: INFRASTRUCTURE (ALTYAPI / VERİTABANI)
// Amelelik bitti! Artık manuel NpgsqlConnection veya INSERT INTO yazmıyoruz.
public class PostgresUserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    // Dependency Injection ile DbContext'i alıyoruz.
    public PostgresUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        // Sihir burada: C# nesnesini Add diyorsun, o arka planda INSERT SQL'ini kendi yazıyor.
        _context.Users.Add(user);
        _context.SaveChanges(); // Veritabanına kaydet
    }

    public List<User> GetAll()
    {
        // Sihir burada: C# listesine çevir diyorsun, o SELECT SQL'ini kendi yazıyor.
        return _context.Users.ToList();
    }
}
