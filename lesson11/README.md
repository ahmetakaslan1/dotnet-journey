# Ders 10: Asenkron Mimari ve Auto-Migrate (Otomatik Kurulum)

Bu proje, bir yazılımcının GitHub'dan indirip **hiçbir ekstra komut yazmadan** saniyeler içinde ayağa kaldırabilmesi için (Clone & Play) tasarlanmıştır.

## Projeyi Çalıştırma Talimatları

Bu projede amele gibi `dotnet ef database update` yazmanıza **GEREK YOKTUR.** Uygulamanın `Program.cs` dosyasına "Otomatik Migrate" (Auto-Migrate) özelliği eklenmiştir.

Projeyi çalıştırmak için sadece şu iki adımı izleyin:

1. **Docker Veritabanını Ayağa Kaldırın:**
   ```bash
   docker compose up -d
   ```

2. **API'yi Başlatın (Sihir Burada Gerçekleşecek):**
   ```bash
   cd Lesson11.API
   dotnet run
   ```

### Arka Planda Ne Oluyor?
Siz `dotnet run` yazdığınızda, API ayağa kalkmadan saniyeler önce veritabanına bağlanır. Tabloların (Users) eksik olduğunu fark ederse, `Migrations` klasöründeki planlara bakarak **kendi kendine tabloyu oluşturur** ve ondan sonra yayına girer. Bu sayede manuel terminal komutlarından tamamen kurtulmuş olursunuz.
