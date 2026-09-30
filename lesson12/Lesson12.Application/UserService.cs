using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Lesson12.Domain;
using Lesson12.Application.DTOs;

namespace Lesson12.Application;

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
        
        // 2. Özel iş mantığı: Şifre hashlama (GÜVENLİK!)
        // Artık o komik "_gizli_hash" eklemesi yok. Dünyanın en güvenli şifreleme algoritmalarından 
        // biri olan BCrypt ile kullanıcının şifresini tek yönlü (geri döndürülemez) olarak şifreliyoruz.
        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.RawPassword);
        
        await _userRepository.AddAsync(newUser);
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var dtoList = _mapper.Map<List<UserResponseDto>>(users);
        return dtoList;
    }

    public async Task<string?> LoginAsync(UserLoginDto dto)
    {
        // 1. Kullanıcıyı emaile göre bul
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return null; // Kullanıcı yok!
        }

        // 2. Şifreyi doğrula (Gelen düz şifre ile veritabanındaki hashlenmiş şifreyi karşılaştır)
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.RawPassword, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null; // Şifre yanlış!
        }

        // 3. Kullanıcı doğruysa ona bir JWT (VIP Kartı) ver
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        // Token içine koyacağımız bilgiler (Örn: "Bu biletin sahibi şu kişidir")
        var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Name)
        };

        // Biletin sahte olmadığını kanıtlamak için şirketimizin gizli damgası (Secret Key)
        // Normalde bu "appsettings.json" dosyasından gelir ama öğrenmek için şimdilik buraya yazıyoruz.
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("BenimCokGizliVeGuvenliSirketAnahtarim12345!"));
        
        // Damgayı basmak için kullanılacak şifreleme algoritması
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        // Biletin kendisini oluşturuyoruz (1 saat geçerli)
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: "Lesson12API",
            audience: "Lesson12Users",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        // Bileti string (metin) olarak geriye döndürüyoruz
        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
