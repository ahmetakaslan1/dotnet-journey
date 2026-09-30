using System.Collections.Generic;
using lesson6.Models;
using lesson6.Repositories;

namespace lesson6.Services;

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
