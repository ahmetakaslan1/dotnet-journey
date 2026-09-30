using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lesson10.Domain;
using Lesson10.Application;

namespace Lesson10.Infrastructure;

public class PostgresUserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public PostgresUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        // Add metodu senkron çalışabilir çünkü sadece EF Core belleğine (RAM) ekler.
        _context.Users.Add(user);
        
        // ASIL ASENKRON OLAY BURADA: Veritabanına yazarken thread'i serbest bırakıyoruz (I/O işlemi)
        await _context.SaveChangesAsync(); 
    }

    public async Task<List<User>> GetAllAsync()
    {
        // ToListAsync() ile veriler çekilene kadar ana thread (Garson) serbest kalır.
        return await _context.Users.ToListAsync();
    }
}
