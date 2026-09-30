# Ders 13: Kurumsal Savunma Hatları ve .env Mimarisi

Bu derste projemizi "Büyük Şirket" (Enterprise) standartlarına yükselttik. Yaptığımız temel değişiklikler şunlardır:

## 1. Ayrık (Decoupled) Mimari ve Arayüz
- Eski usul MVC yerine, Backend'imizi sadece bir **Web API** olarak tuttuk ve Frontend için ayrı bir klasör (`frontend`) açtık.
- `index.html`, `style.css` ve `app.js` ile modern, cam (Glassmorphism) tasarımlı bir arayüz oluşturduk.
- API'de dışarıdan gelen isteklere izin vermek için **CORS** ayarlarını (`builder.Services.AddCors()`) aktif ettik.

## 2. FluentValidation (Veri Doğrulama Zırhı)
- Dışarıdan gelen verilerin (DTO'ların) veritabanına ulaşmadan önce kurallara uyup uymadığını kontrol eden kurallar yazdık.
- `UserCreateDtoValidator`: Şifre en az 6 karakter olmalı, isim boş olamaz gibi.
- Bunlar, `Program.cs` içindeki `AddFluentValidationAutoValidation()` süzgeci sayesinde Controller'a hiç ulaşmadan otomatik kontrol edilir (400 Bad Request döner).

## 3. Global Exception Handling (Merkezi Hata Yönetimi)
- `GlobalExceptionMiddleware.cs` yazarak, sistemdeki tüm kodlama veya iş mantığı (Örn: "Aynı e-posta zaten var") hatalarını tek bir merkezde yakaladık.
- Bu sayede sistem asla çökmez ve kullanıcının görebileceği şık, standart bir JSON hata mesajı döner.

## 4. Benzersiz (Unique) Veritabanı Kuralları
- `AppDbContext` içinde `OnModelCreating` metodunu ezerek (override), Entity Framework'e `Email` kolonunun **Unique** (Benzersiz) olması gerektiğini söyledik.
- `dotnet ef migrations add AddUniqueEmailIndex` komutuyla bunu veritabanına yansıttık. (Race condition hatalarını önlemek için).

## 5. Çevre Değişkenleri (.env) Entegrasyonu
Daha önce şifreler, bağlantı adresleri ve gizli JWT anahtarlarını kodun içine sabit (hardcoded) yazıyorduk. Bu, sektörde büyük bir güvenlik açığıdır! 
- Proje ana dizininde bir `.env` dosyası oluşturduk. (İçinde Postgres portu, şifresi ve JWT Gizli Anahtarı var).
- **Docker için:** `docker-compose.yml` dosyasına `POSTGRES_USER: ${DB_USER}` diyerek bu `.env` dosyasından okumasını sağladık.
- **.NET API için:** `DotNetEnv` kütüphanesini indirdik. `Program.cs` içinde `DotNetEnv.Env.Load()` diyerek aynı `.env` dosyasını API'mize de tanıttık!
- Artık hem Docker hem .NET API, şifreleri aynı ortak `.env` dosyasından okuyor. Yarın şifreyi değiştirirsek tek bir dosyadan her yer değişmiş olacak!
