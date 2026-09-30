namespace Lesson7.Domain;

// BÖLÜM 1: DOMAIN (ÇEKİRDEK)
// Bu proje (ev) SADECE ve SADECE varlıkları (Entity/Model) tutar.
// Başka hiçbir projeye referans vermez (Dependency yoktur).
// En saf katmandır.
public class User
{
    public int Id { get; init; }
    public string Name { get; set; }
}
