# Ders 6 İnceleme Raporu: İnternete Açılan Kapı (Web API) ve Yalancı Güvenlik

## 1. Önceki Dersten Kalan Hangi Sorunu Çözdük?
Ders 5'teki konsol uygulamasından tamamen kurtulduk. Kusursuzlaşan mimarimizi aldık ve bir **ASP.NET Core Web API** projesine dönüştürdük. Uygulamamız artık 7/24 interneti (HTTP isteklerini) dinleyen bir sunucu oldu.

## 2. Bu Derste Neler Yaptık?
Projemizi kurumsal bir klasörleme yapısına (N-Tier başlangıcı) soktuk: `Controllers`, `Services`, `Repositories` ve `Models`. En önemlisi internet isteklerini karşılayacak olan `UserController` sınıfını yazdık.

### A. Controller ve HTTP İstekleri
İnternetten `GET http://localhost:5000/api/users` isteği geldiğinde, bunu karşılayacak olan "Kapıcı" yani Controller'ı yazdık. Kapıcı (Controller), tıpkı Konsol'da yaptığımız gibi, işi `UserService`'e (İş Mantığına) devretti.

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    // Controller, Dependency Injection ile otomatik olarak UserService'i alır.
    public UserController(UserService userService) 
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Get() 
    {
        return Ok(_userService.GetAllUsers());
    }
}
```

### B. Yerleşik (Built-in) Dependency Injection
Konsol uygulamasında `ServiceCollection` kutusunu kendi ellerimizle oluşturmuştuk. Web API projesinde ise `builder.Services` objesinin arkada zaten bu işi yaptığını gördük. Kodumuz kısaldı ve standartlaştı.

---

## 3. Mimari Analiz ve Felsefe
* **Sunucu Mimarisinin Temeli:** Bir internet sitesinin arka planının (Backend) aslında sonsuz bir döngü içinde (Run) gelen HTTP metinlerini bekleyip, onları ilgili C# fonksiyonlarına yönlendirmekten ibaret olduğunu gördük.

*(İşte burası mimarinin çöküş noktasıdır)*
* **C#'ta Klasörlerin (Namespace) Zayıflığı:** Projeyi klasörlere böldük ama hepsi aynı evin (aynı `.csproj`'un) içindeydi. Java dünyasında "Package-Private" özelliği ile farklı klasörlerdeki kodların birbirini görmesini yasaklayabilirsiniz. Ancak C#'ta klasörlerin GÜVENLİK GÜCÜ YOKTUR.
* **Stajyer Yazılımcı Tehlikesi:** Ekibe yeni katılan bir geliştirici, `UserController` klasörünün içindeyken çok rahat bir şekilde `using lesson6.Repositories;` yazıp `PostgresUserRepository` nesnesini Controller içinde kullanabilir. Kötü kod yazmasını engelleyecek fiziksel bir duvar yoktu.

---

## 4. Eksiklikler Nelerdi? (Bizi Ders 7'ye Götüren Sebepler)
* **Derleme Zamanı Güvenliği Yoktu:** Yazılımcının mimariyi delmesini fiziksel olarak engelleyemiyorduk. "Lütfen Controller'a SQL yazmayın" demekle kod güvenliği sağlanmaz.
* **Çözüm İhtiyacı:** Kodu sadece klasörlere bölmek yerine, **Projeleri Fiziksel Binalara (.csproj DLL'lerine)** bölmek zorundaydık. Bu da bizi modern .NET'in zirvesine, **Ders 7'ye (Gerçek Clean Architecture)** mecbur bıraktı.
