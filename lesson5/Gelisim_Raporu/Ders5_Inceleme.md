# Ders 5 İnceleme Raporu: Inversion of Control ve Dependency Injection

## 1. Önceki Dersten Kalan Hangi Sorunu Çözdük?
Ders 4'te veritabanlarımız mükemmel çalışıyordu ancak C# kodumuz hala `new PostgresUserRepository()` gibi manuel sınıf oluşturmalarına ve `if-else` karmaşasına bağımlıydı.
Bu derste **Dependency Injection (Bağımlılık Enjeksiyonu)** mantığını sisteme entegre ettik. C#'ın kendi `ServiceCollection` yapısıyla merkezi bir DI Kutusu (Konteyner) yarattık.

## 2. Bu Derste Neler Yaptık?
İş mantığını yöneten `UserService` sınıfını, nesne üretme derdinden kurtardık. `Program.cs`'teki o çirkin `if-else` yığınını silip, sadece sisteme kuralları öğrettik.

### A. Constructor Injection (Yapıcı Metot Enjeksiyonu)
`UserService`'e dedik ki: *"İçeride `new Postgres...` yazma! Bana çalışmak için neye ihtiyacın olduğunu Constructor (Yapıcı Metot) ile söyle, ben sana vereceğim."*

```csharp
public class UserService
{
    private readonly IUserRepository _db; // Sadece sözleşmeyi biliyor

    // Constructor Injection: "Bana IUserRepository kuralına uyan bir şey ver!"
    public UserService(IUserRepository db)
    {
        _db = db;
    }
}
```

### B. Merkezi Kutu (ServiceCollection) Kaydı
`Program.cs` içinde Microsoft'un kütüphanesini kullanarak sisteme kuralı öğrettik.

```csharp
var services = new ServiceCollection();

// Sisteme dedik ki: Biri senden IUserRepository isterse, ona Postgres ver!
services.AddScoped<IUserRepository, PostgresUserRepository>();
services.AddScoped<UserService>(); // Servisi de kutuya kaydet

var provider = services.BuildServiceProvider();
```

### C. Veri Kalıcılığı (Docker Volumes)
Docker konteynerini sildiğimizde verilerin uçmaması için `docker-compose.yml` içine `volumes` ayarını ekledik. ` - ./pgdata:/var/lib/postgresql` satırıyla, konteyner içindeki verileri kendi Windows klasörümüze fiziksel olarak bağladık.

---

## 3. Mimari Analiz ve Felsefe
* **Inversion of Control (Kontrolün Tersine Çevrilmesi):** Daha önce nesneleri `new` ile **BİZ** yönetiyorduk (Kontrol bizdeydi). Şimdi kuralları framework'e (Microsoft kutusuna) verdik, nesne yaratma işini **O** yapıyor (Kontrol tersine döndü). Bu, kurumsal yazılımın temel taşıdır.
* **Service Lifetimes (Yaşam Döngüleri):** Sınıfları kutuya kaydederken `AddScoped` kelimesini kullandık. Bunun sektörel anlamı şudur: "İnternetten bir kullanıcı geldiğinde ona yepyeni bir veritabanı bağlantısı aç (yeni nesne üret), kullanıcı işini bitirip gidince o nesneyi RAM'den çöpe at."

---

## 4. Eksiklikler Nelerdi? (Bizi Ders 6'ya Götüren Sebepler)
* **İlkel Bir Platform (Konsol Uygulaması):** Mimarimiz kusursuz olsa da hala Siyah Ekrana mahkumduk. Kullanıcıdan `Console.ReadLine()` ile isim alıyorduk.
* **Dış Dünyaya Kapalılık:** Sektörde sistemler birbirleriyle HTTP protokolü üzerinden (İnternetten) haberleşir. Bizim harika algoritmamıza bir cep telefonu uygulamasının veya React ile yazılmış bir web sitesinin bağlanma ihtimali SIFIRDI.
Bu durum bizi kodu internete açmaya, yani **Ders 6'ya (ASP.NET Core Web API)** geçmek zorunda bıraktı.
