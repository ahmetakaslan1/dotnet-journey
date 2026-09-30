# Ders 3 İnceleme Raporu: Gerçek Dünya ve Veritabanı Bağlantıları

## 1. Önceki Dersten Kalan Hangi Sorunu Çözdük?
Ders 2'de kodlarımız sadece "mış gibi" yapıyordu.
Bu derste `Npgsql`, `MySqlConnector` ve `Microsoft.Data.Sqlite` gibi gerçek sektör kütüphanelerini kurarak C# üzerinden veritabanlarına canlı bağlantılar (tüneller) açtık ve SQL komutları gönderdik.

## 2. Bu Derste Neler Yaptık?
`PostgresUserRepository` ve `SqliteUserRepository` sınıflarımızın içini boş `Console.WriteLine`'lardan temizleyip, yerine gerçek ADO.NET bağlantı kodlarını yazdık.

### A. IDisposable (`using`) ve Bellek Yönetimi
Veritabanı bağlantısı açmak çok maliyetli ve tehlikelidir. Kapatılmazsa sistemi çökertir (Memory Leak). C#'ın bunu otomatik kapatmasını sağlamak için `using` kelimesini kullandık.

```csharp
// using kelimesi, süslü parantez bitince bağlantıyı RAM'den KESİN OLARAK siler.
using var connection = new NpgsqlConnection("Host=localhost;Database=mydb;...");
connection.Open();
// ... işlemler ...
```

### B. Güvenlik: Parametreli Sorgular
Dışarıdan gelen veriyi doğrudan SQL string'i içine yapıştırmadık. Bunun yerine `@name` gibi parametreler kullandık.

```csharp
// YANLIŞ VE TEHLİKELİ (SQL Injection saldırısına açık)
var cmd = new NpgsqlCommand($"INSERT INTO Users (Name) VALUES ('{kullaniciAdi}')");

// DOĞRU VE SEKTÖR STANDARDI
var cmd = new NpgsqlCommand("INSERT INTO Users (Name) VALUES (@name)");
cmd.Parameters.AddWithValue("name", kullaniciAdi);
```

---

## 3. Mimari Analiz ve Felsefe
* **Amelelik Katmanı:** ADO.NET (Npgsql vs) ile kod yazmanın ne kadar zor ve uzun olduğunu gördük. Sadece bir satır eklemek için `Connection` aç, `Command` yaz, parametre ekle, `ExecuteNonQuery` çalıştır gibi bir sürü işlem yaptık. Sektörde ORM'lerin (Entity Framework) neden doğduğunu anlamanın en iyi yolu, bu acıyı çekmektir.

---

## 4. Eksiklikler Nelerdi? (Bizi Ders 4'e Götüren Sebepler)
* **Çevresel Bağımlılık Krizi ("Bende Çalışıyordu!"):** Bu kodu sana veya bir başkasına gönderdiğimde kod anında ÇÖKECEKTİ. Neden? Çünkü kodun çalışması için bilgisayarda Postgres ve MySQL programlarının **fiziksel olarak kurulu olması**, şifrelerinin koddakiyle aynı olması gerekiyordu.
* **Geliştirme Ortamı Cehennemi:** Her yazılımcının bilgisayarına bu veritabanlarını kurmasını beklemek modern yazılımda imkansızdır.
Bu devasa altyapı sorunu, bizi sistemleri "kodla beraber taşınabilir" hale getiren **Ders 4'e (Docker)** girmeye mecbur bıraktı.
