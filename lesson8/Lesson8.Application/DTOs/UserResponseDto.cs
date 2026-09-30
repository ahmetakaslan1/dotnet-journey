namespace Lesson8.Application.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    // DİKKAT: PasswordHash bilerek buraya eklenmedi (Güvenlik)
}
