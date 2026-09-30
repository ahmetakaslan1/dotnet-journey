using System.Collections.Generic;
using Lesson9.Domain;
using Lesson9.Application.DTOs;

namespace Lesson9.Application;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void RegisterUser(UserCreateDto dto)
    {
        // MAPPING: DTO'dan Entity'ye dönüşüm
        var newUser = new User 
        { 
            Name = dto.Name,
            Email = dto.Email,
            // Gerçekte burada kütüphane ile şifreleme (Bcrypt vb) yapılır
            PasswordHash = dto.RawPassword + "_gizli_hash" 
        };
        
        _userRepository.Add(newUser);
    }

    public List<UserResponseDto> GetAllUsers()
    {
        var users = _userRepository.GetAll();
        
        // MAPPING: Entity'den DTO'ya dönüşüm
        var dtoList = new List<UserResponseDto>();
        foreach (var user in users)
        {
            dtoList.Add(new UserResponseDto 
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
                // PasswordHash DTO'da olmadığı için internete SIZMIYOR!
            });
        }
        
        return dtoList;
    }
}
