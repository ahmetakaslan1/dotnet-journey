using System;
using lesson4;

Console.WriteLine("=== SEKTÖR SEVİYESİ DB GEÇİŞ MİMARİSİ ===");
Console.WriteLine("Hangi veritabanını kullanmak istersin?");
Console.WriteLine("1 - SQLite (Yerel Dosya)");
Console.WriteLine("2 - PostgreSQL (Docker Container)");
Console.WriteLine("3 - MySQL (Docker Container)");
Console.Write("Seçimin (1/2/3): ");
string secim = Console.ReadLine() ?? "1";

// ---------------------------------------------------------
// İŞTE INTERFACE'İN BÜYÜSÜ BURADA BAŞLIYOR!
// ---------------------------------------------------------
// Değişkenimizin tipi "IUserRepository". 
// İçine hangi veritabanını koyarsak koyalım kodun KALANI DEĞİŞMEYECEK!
IUserRepository userRepository; 

if (secim == "2")
{
    userRepository = new PostgresUserRepository();
    Console.WriteLine("=> PostgreSQL moduna geçildi.");
}
else if (secim == "3")
{
    userRepository = new MySqlUserRepository();
    Console.WriteLine("=> MySQL moduna geçildi.");
}
else
{
    userRepository = new SqliteUserRepository();
    Console.WriteLine("=> SQLite moduna geçildi.");
}

Console.WriteLine("---------------------------------------------------------");

// Kodun bundan sonraki kısmı hangi DB'de olduğunu ASLA BİLMEZ VE UMURSAMAZ!
Console.Write("\nLütfen veritabanına eklenecek kişinin adını girin: ");
string name = Console.ReadLine() ?? "Gizemli Kullanıcı";

User newUser = new User { Name = name };

// BUM! userRepository o an Postgres mi, MySQL mi, Sqlite mı? Umrumuzda değil. O bir Repository ve "Add" yapmayı biliyor.
userRepository.Add(newUser);

Console.WriteLine("\n--- O Anki Seçili Veritabanındaki Tüm Kullanıcılar ---");
var allUsers = userRepository.GetAll();
foreach (var user in allUsers)
{
    Console.WriteLine($"ID: {user.Id}, İsim: {user.Name}");
}
