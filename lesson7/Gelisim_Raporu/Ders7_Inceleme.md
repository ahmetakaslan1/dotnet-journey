# Ders 7 İnceleme Raporu: Mimari Zirve (Clean Architecture)

## 1. Önceki Dersten Kalan Hangi Sorunu Çözdük?
Ders 6'da her şeyi aynı projenin içine, sadece klasörlere bölerek koyduğumuz için güvenlik (erişim) sorunları yaşamıştık.
Bunu çözmek için odaları yıktık ve yerine 4 farklı FİZİKSEL BİNA (Class Library Projesi) diktik ve tek bir Çatı (Solution - `.sln`) altında birleştirdik:
`Domain`, `Application`, `Infrastructure`, `API`.

## 2. Bu Derste Neler Yaptık?
Fiziksel projelerimizi kurup aralarındaki yolları (Referansları) `<ProjectReference>` ile elle biz inşa ettik.

### A. Proje Referanslarının (.csproj) Gücü
C# dilinde projelerin birbirini görebilmesi için `ProjectReference` verilmesinin zorunlu olduğunu öğrendik.

```xml
<!-- API projesinin .csproj dosyası -->
<ItemGroup>
  <ProjectReference Include="..\Lesson7.Application\Lesson7.Application.csproj" />
  <ProjectReference Include="..\Lesson7.Infrastructure\Lesson7.Infrastructure.csproj" />
</ItemGroup>
```
Application projesinden Infrastructure'a referans VERMEDİĞİMİZ için, iş mantığı katmanımızdan (Application) bir daha asla SQL veya Postgres koduna ulaşılamayacağını fiziksel olarak garanti altına aldık.

---

## 3. Mimari Analiz ve Felsefe
Bu derste seninle konuştuğumuz mimari felsefeler, bir Senior mühendisin seviyesindedir:

* **Java (Maven) vs .NET:** Java dünyasında devasa projelerde Maven/Gradle ile **Multi-Module (pom.xml)** yapısının kullanıldığını gördük. Java'daki `pom.xml` içindeki `<dependency>` etiketlerinin, .NET'teki `.csproj` içindeki `<ProjectReference>` ile birebir aynı mimari amaca hizmet ettiğini anladık.
* **Derleme Zamanı Güvenliği (Compile-Time Security):** Bir geliştirici `UserService` (Application) içinde `using Lesson7.Infrastructure` yazmaya kalkarsa IDE anında altını çizer ve projeyi Derlemez (Build Error). İnsana güvenmedik, sistemi kurallara bağladık.
* **Geçişli Referans (Transitive Dependency):** `API -> Application -> Domain` zincirinde API projesinin, Domain projesini doğrudan referans almasa bile geçişli olarak nasıl görebildiğini kanıtladık.
* **Bağımlılığı Tersine Çevirme (Dependency Inversion):** Sözleşmeyi (`IUserRepository`) Infrastructure (Veritabanı) projesinden koparıp, Application projesinin kalbine koyduk. Böylece "Kuralları koyan iş mantığıdır, amele olan veritabanı o kurala uymak için bizim ayağımıza (Application'a) referans vermek zorundadır" dedik.

> **Kusursuz Mimari (IoC - Bootstrapper):** Şablonumuzda `API` projesi, DI (Program.cs) ayarı yapabilmek için `Infrastructure`'ı mecbur görüyor. Ancak %100 "Purist (Kusursuz)" kurumsal mimarilerde bu bile istenmez. Çözüm olarak **5. bir Proje (IoC / Bootstrapper)** yaratılır, tüm projeler bu 5. projeye bağlanır ve API'nin veritabanıyla olan o ince bağı tamamen koparılır.

---

## 4. Eksiklikler Nelerdi? (Bizi Ders 8'e Götüren Sebepler)
Mimarimiz (Fiziksel Duvarlar) artık KUSURSUZ! Ancak odaların içindeki kodlarda 2 büyük günahımız kaldı:
1. **Manuel SQL Yazımı (Amelelik):** `Infrastructure` projemizin içinde hala `INSERT INTO Users...` şeklinde ilkel string SQL'ler yazıyoruz. Sektörde bu işler için **Entity Framework Core (ORM)** kullanılır. C# kodları otomatik SQL'e çevrilir.
2. **Veritabanını İnternete Çıplak Çıkarmak:** Controller'larımız `User` modelini doğrudan tarayıcıya yolluyor. Güvenlik için araya sahte kuryeler yani **DTO (Data Transfer Object)** konulması şarttır.

**Hedef:** Ders 8'de bu çelik omurganın üzerine EF Core (ORM) ve DTO mantığını inşa edeceğiz.
