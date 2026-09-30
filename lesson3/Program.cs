using System;
using lesson3;

// GERÇEK BİR SENARYO
// Programımız HANGİ veritabanı olduğunu yine umursamıyor, IUserRepository'e bağlı!
// Ama bu sefer Console.WriteLine yerine arkada GERÇEK bir SQL çalışıyor.

IUserRepository userRepository = new SqliteUserRepository();

Console.Write("Lütfen veritabanına eklenecek kişinin adını girin: ");
string name = Console.ReadLine() ?? "Gizemli Kullanıcı";

// 1. Yeni bir kullanıcı nesnesi oluştur.
User newUser = new User { Name = name };

// 2. Bunu veritabanına (gerçek SQLite dosyasına) kaydet!
userRepository.Add(newUser);

Console.WriteLine("\n--- Veritabanındaki Tüm Kullanıcılar (GERÇEK VERİ) ---");

// 3. Veritabanından (SQL ile) tüm kullanıcıları çek ve ekrana yaz!
var allUsers = userRepository.GetAll();
foreach (var user in allUsers)
{
    Console.WriteLine($"ID: {user.Id}, İsim: {user.Name}");
}

Console.WriteLine("\n[Not: İşlem bitti. İnanmıyorsan klasördeki 'app.db' dosyasına bak, GERÇEK veritabanın oluştu!]");
