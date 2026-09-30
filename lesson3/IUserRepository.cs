using System.Collections.Generic;

namespace lesson3;

// Backend dünyasında veritabanı işlemlerini yaptığımız arayüzlere (Interface)
// genellikle "Repository" (Depo) deriz. Bu yüzden adını değiştirdik!
public interface IUserRepository
{
    void Add(User user);
    List<User> GetAll();
}
