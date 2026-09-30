using System.Collections.Generic;
using Lesson7.Domain;

namespace Lesson7.Application;

// İŞ MANTIĞI:
// Bu servis, IUserRepository adında bir sözleşme bekler.
// Verilerin SQL ile mi yoksa SQLite ile mi yazıldığını zerre kadar umursamaz.
public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void RegisterUser(string name)
    {
        var newUser = new User { Name = name };
        _userRepository.Add(newUser);
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAll();
    }
}
