# Ders 9: Entity Framework Core'a Giriş (Manuel Migrations)

Bu proje EF Core'un temellerini anlamak için **Manuel** olarak yapılandırılmıştır.

## Projeyi Çalıştırma Talimatları (ÇOK ÖNEMLİ)

Bu projeyi GitHub'dan yeni indirdiyseniz veya Docker konteynerinizi sıfırdan kurduysanız, sadece `dotnet run` yazmak **YETERLİ OLMAYACAKTIR**. Veritabanı tablolarınız (Users) eksik olduğu için hata alırsınız.

Projeyi ayağa kaldırmak için **sırasıyla** şu komutları çalıştırmalısınız:

1. **Docker Veritabanını Ayağa Kaldırın:**
   ```bash
   docker compose up -d
   ```

2. **Tabloları Veritabanına İnşa Edin (Manuel Update):**
   ```bash
   dotnet ef database update --project Lesson9.Infrastructure --startup-project Lesson9.API
   ```

3. **API'yi Başlatın:**
   ```bash
   cd Lesson9.API
   dotnet run
   ```

*Not: Büyük kurumsal (Enterprise) şirketlerde bu manuel komutlar DevOps robotları (CI/CD) tarafından otomatik çalıştırılır.*
