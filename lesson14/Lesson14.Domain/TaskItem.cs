namespace Lesson14.Domain;

// Görev Sınıfımız (Task)
// .NET'te kendi Task sınıfı olduğu için karışmasın diye adını "TaskItem" koyuyoruz.
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;

    // FOREIGN KEY (Yabancı Anahtar) - Görevin kime ait olduğunu tutar
    public int UserId { get; set; }
    
    // NAVIGATION PROPERTY - EF Core'un tabloları birleştirmesi (Join) için nesne referansı
    public User? User { get; set; }
}
