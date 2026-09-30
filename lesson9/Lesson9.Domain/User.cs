namespace Lesson9.Domain;

// BÖLÜM 1: DOMAIN (ÇEKİRDEK)
public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } // DTO'nun koruyacağı gizli alan
}
