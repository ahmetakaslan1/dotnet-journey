using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lesson10.Domain;
using Lesson10.Application.DTOs;

namespace Lesson10.Application;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Servisimiz de "async Task" olmak zorunda çünkü repository'yi "await" ile bekliyor.
    public async Task RegisterUserAsync(UserCreateDto dto)
    {
        var newUser = new User 
        { 
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = dto.RawPassword + "_gizli_hash" 
        };
        
        await _userRepository.AddAsync(newUser);
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        
        // MAPPING: Entity'den DTO'ya dönüşüm
        // TODO (SEKTÖR GERÇEĞİ - Kurye Hamallığı): 
        // Burada 3 tane özellik olduğu için elle (manuel) eşleştirmek kolay. 
        // Ama gerçek projelerde User tablosunda 50 tane kolon olabilir! Hepsini tek tek yazmak ameleliktir.
        // Sektörde bu manuel eşleştirme yerine "AutoMapper" veya "Mapster" adlı kütüphaneler kullanılır.
        // İlerleyen derslerde bu amelelikten AutoMapper ile kurtulacağız.
        var dtoList = new List<UserResponseDto>();
        foreach (var user in users)
        {
            dtoList.Add(new UserResponseDto 
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            });
        }
        return dtoList;
    }
}
