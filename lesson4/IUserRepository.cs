using System.Collections.Generic;

namespace lesson4;

public interface IUserRepository
{
    void Add(User user);
    List<User> GetAll();
}
