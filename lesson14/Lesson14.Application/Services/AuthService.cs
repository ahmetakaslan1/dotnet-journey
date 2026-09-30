using AutoMapper;
using Lesson14.Domain;
using Lesson14.Application.DTOs;
using Lesson14.Application.Repositories;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lesson14.Application.Services;

public class AuthService
{
    private readonly IGenericRepository<User> _userRepository; // BÜYÜK YENİLİK: Generic Repo kullandık!
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IGenericRepository<User> userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(UserRegisterDto dto)
    {
        // 1. E-posta zaten kayıtlı mı?
        var existingUsers = await _userRepository.GetAllAsync(u => u.Email == dto.Email);
        if (existingUsers.Any())
            throw new Exception("Bu e-posta adresi zaten kullanılıyor!");

        // 2. DTO'yu Entity'ye çevir (Mapper)
        var newUser = _mapper.Map<User>(dto);
        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // 3. Kaydet (Generic Repo)
        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();

        return "Kayıt işlemi başarıyla tamamlandı.";
    }

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        var users = await _userRepository.GetAllAsync(u => u.Email == dto.Email);
        var user = users.FirstOrDefault();

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("E-posta veya şifre hatalı!");

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        // Şifreyi .env üzerinden (IConfiguration ile) çekiyoruz!
        var secret = _configuration["JWT_SECRET"];
        if (string.IsNullOrEmpty(secret)) throw new Exception("JWT Secret bulunamadı!");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2), // Token ömrü 2 saat
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
