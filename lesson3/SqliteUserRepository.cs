using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite; // İşte GERÇEK veritabanı kütüphanesi!

namespace lesson3;

// Bu sınıf sadece IUserRepository kurallarına uyar VE GERÇEKTEN SQLite veritabanına bağlanır.
public class SqliteUserRepository : IUserRepository
{
    // Veritabanı bağlantı cümlemiz (Connection String). "app.db" adında GERÇEK bir dosyaya bağlanacağız.
    private readonly string _connectionString = "Data Source=app.db";

    public SqliteUserRepository()
    {
        // Constructor (yapıcı metot): Sınıf ilk çağrıldığında çalışır.
        // Eğer veritabanında tablo yoksa hemen gerçek bir SQL sorgusuyla tabloyu oluşturuyoruz!
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open(); // Veritabanı dosyasını aç
            var command = connection.CreateCommand();
            
            // IF NOT EXISTS: Tablo yoksa oluştur, varsa dokunma.
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                )";
            command.ExecuteNonQuery(); // SQL komutunu çalıştır!
        }
    }

    public void Add(User user)
    {
        // 1. Veritabanı ile bir tünel/bağlantı aç. (using kullanırız ki işimiz bitince ram'den silinsin)
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open(); // Tüneli açtık.

            // 2. Bir SQL komutu oluştur.
            var command = connection.CreateCommand();
            
            // $name bir parametredir. Dışarıdan gelen veriyi direkt SQL'e yazmak GÜVENLİK AÇIĞIDIR (SQL Injection).
            command.CommandText = "INSERT INTO Users (Name) VALUES ($name)";
            command.Parameters.AddWithValue("$name", user.Name); 

            // 3. Komutu çalıştır (ExecuteNonQuery: Insert, Update, Delete gibi veri döndürmeyen komutlar içindir)
            command.ExecuteNonQuery();
            
            Console.WriteLine($"[GERÇEK İŞLEM]: {user.Name} veritabanına eklendi!");
        }
    }

    public List<User> GetAll()
    {
        var users = new List<User>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name FROM Users"; // Bütün kullanıcıları getir!

            // ExecuteReader, bize veritabanından satır satır veri okumamızı sağlar.
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read()) // Okunacak satır olduğu sürece dön...
                {
                    // Okuduğumuz her satırı, C# içindeki User nesnesine çeviriyoruz.
                    var user = new User
                    {
                        Id = reader.GetInt32(0),   // 0. sütun (Id)
                        Name = reader.GetString(1) // 1. sütun (Name)
                    };
                    users.Add(user);
                }
            }
        }
        return users;
    }
}
