using System.Collections.Generic;
using lesson6.Models;

namespace lesson6.Repositories;

public interface IUserRepository
{
    void Add(User user);
    List<User> GetAll();
}
