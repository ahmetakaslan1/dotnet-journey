# Ders 9: Entity Framework Core (ORM) ve Büyülü Geçiş

## Ne Öğrendik ve Ne Yaptık?
Bu derste .NET ekosisteminin en büyük ve en çok kullanılan veritabanı silahını (Entity Framework Core) projemize entegre ettik. Ameleliğe kalıcı olarak son verdik.

1. **Amele SQL'in Çöpe Atılması:**
   Eskiden `PostgresUserRepository` içinde `NpgsqlConnection`, `CREATE TABLE`, `INSERT INTO` gibi amele komutları yazıyorduk. Bunları sildik.
   Yerine sadece şunu yazdık:
   ```csharp
   _context.Users.Add(user);
   _context.SaveChanges();
   ```

2. **AppDbContext (Veritabanı Yöneticisi):**
   `Infrastructure` katmanına `AppDbContext` adında bir sınıf ekledik. Bu sınıf EF Core'un kalbidir. 
   `public DbSet<User> Users { get; set; }` diyerek C#'taki `User` sınıfımızın veritabanında `Users` adında bir tabloya karşılık geleceğini EF Core'a fısıldadık.

3. **Program.cs (Fişin Takılması - Composition Root):**
   Bağlantı şifresini (Connection String) ve motor türünü (Postgres) `Program.cs` içinden verdik.
   *Java (Spring Boot) kullananların application.properties dosyasında yaptığı işi, C#'ta şeffaflık adına Program.cs içinde kodla (`AddDbContext`) açıkça yazıyoruz.*

4. **Migrations (Göçler):**
   Visual Studio'da `Add-Migration` (biz terminalde `dotnet ef migrations add` kullandık) diyerek EF Core'un tabloları kendi kendine oluşturmasını sağladık. "Tabloyu Entity'den alma" hedefimizi %100 oranında başardık.

---

## Eksikliğimiz Ne? (Ufukta Ne Var?)
Mimari oturdu, veritabanı oturdu. Peki gerçek bir "Enterprise" projeden neyimiz eksik?

1. **Asenkron Eksikliği (En büyük darboğaz):** Metodlarımız şu an senkron (`void`). Milyonlarca isteği kaldırabilmek için `async Task` ve `await` yapısına geçmeliyiz.
2. **Güvenlik Eksikliği:** Kullanıcıların JWT ile kimlik doğrulaması yapması ve şifrelerin BCrypt ile gizlenmesi gerekiyor.
3. **Doğrulama (Validation):** DTO'ların içindeki verilerin (Email boş mu, hatalı mı?) kurala uygun olup olmadığını FluentValidation ile denetlemeliyiz.
4. **AutoMapper:** Entity'den DTO'ya dönüşümleri elle yazmak yerine (Kurye Hamallığı) AutoMapper ile tek satırda bağlamalıyız.
5. **Hata Yönetimi (Middleware):** Hataları Global Exception Handler ile yakalayıp kullanıcıya temiz bir JSON dönmeliyiz.
