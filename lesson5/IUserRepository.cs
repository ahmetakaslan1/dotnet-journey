using System.Collections.Generic;

namespace lesson5;

public interface IUserRepository
{
    void Add(User user);
    List<User> GetAll();
}
