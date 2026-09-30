using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Lesson11.Domain;
using Lesson11.Application.DTOs;

namespace Lesson11.Application;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    // Dependency Injection ile IMapper'ı istiyoruz (Program.cs bunu bize verecek)
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task RegisterUserAsync(UserCreateDto dto)
    {
        // 1. AutoMapper ile Name ve Email'i otomatik olarak kopyala
        var newUser = _mapper.Map<User>(dto);
        
        // 2. Özel iş mantığı: Şifre hashlama (Bu işlemi AutoMapper'a vermeyiz çünkü bu bir "Business Logic"tir)
        newUser.PasswordHash = dto.RawPassword + "_gizli_hash";
        
        await _userRepository.AddAsync(newUser);
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        
        // MAPPING: Entity'den DTO'ya dönüşüm (AutoMapper Sihri!)
        // Kurye hamallığı bitti! 15 satırlık kod, tek satıra indi.
        // _mapper, "users" listesini alır, AutoMapperProfile'daki kurallara bakar, 
        // ve isimleri eşleşen tüm kolonları otomatik olarak UserResponseDto'ya dönüştürür.
        var dtoList = _mapper.Map<List<UserResponseDto>>(users);
        
        return dtoList;
    }
}
