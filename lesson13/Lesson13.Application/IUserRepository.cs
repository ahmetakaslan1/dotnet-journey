using System.Collections.Generic;
using System.Threading.Tasks;
using Lesson13.Domain;

namespace Lesson13.Application;

public interface IUserRepository
{
    // Task, asenkron işlemin gelecekte biteceğini temsil eden bir "söz"dür (Promise).
    Task AddAsync(User user);
    
    // Geriye Liste dönecek ama bu Liste bir "Task" (Gelecekteki bir işlem) içinde gelecek.
    Task<List<User>> GetAllAsync();
    
    // Login işlemi için emaile göre kullanıcı getirecek fonksiyon
    Task<User?> GetByEmailAsync(string email);
}
