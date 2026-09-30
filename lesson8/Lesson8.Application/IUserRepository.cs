using System.Collections.Generic;
using Lesson8.Domain;

namespace Lesson8.Application;

// BÖLÜM 2: APPLICATION (İŞ MANTIĞI)
// Bu proje (ev) SADECE "Sözleşmeleri" (Interface) ve "İş Kurallarını" (Servisler) içerir.
// Hangi veritabanının kullanılacağını ASLA bilmez (O yüzden PostgresUserRepository burada YOKTUR).
public interface IUserRepository
{
    void Add(User user);
    List<User> GetAll();
}
