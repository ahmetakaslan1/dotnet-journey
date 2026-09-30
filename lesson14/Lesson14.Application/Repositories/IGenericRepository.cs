using System.Linq.Expressions;

namespace Lesson14.Application.Repositories;

// T (Type) - Bu bir "Generic" (Şablon) sınıftır.
// T yerine User verirsek User tablosunda, TaskItem verirsek TaskItem tablosunda çalışır.
// class kısıtlaması: T mutlaka bir sınıf (class) olmalıdır (Örn: int veya string olamaz).
public interface IGenericRepository<T> where T : class
{
    // Veritabanından veriyi okur
    Task<T?> GetByIdAsync(int id);
    
    // Şartlı arama (Örn: Sadece bana ait görevleri getir)
    // Expression<Func<T, bool>> -> C#'taki LINQ "Where(x => x...)" şartlarını temsil eder.
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    
    // Ekleme
    Task AddAsync(T entity);
    
    // Güncelleme
    void Update(T entity);
    
    // Silme
    void Delete(T entity);
    
    // Değişiklikleri kaydet (Unit of Work mantığı için)
    Task SaveChangesAsync();
}
