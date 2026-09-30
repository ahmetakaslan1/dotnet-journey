namespace Lesson14.Domain;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // BİRİN ÇOĞA (One-to-Many) İLİŞKİSİ
    // Bir kullanıcının birden fazla görevi olabilir. 
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
