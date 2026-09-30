# Ders 2 İnceleme Raporu: Sözleşmelerin Gücü (Interfaces)

## 1. Önceki Dersten Kalan Hangi Sorunu Çözdük?
Ders 1'de kodumuzda bir "Kural (Sözleşme)" yoktu. Her sınıf kafasına göre metot isimleri yazabilirdi. 
Bu derste sisteme **Interface (`IUserRepository`)** mantığını entegre ederek anarşiye son verdik.

## 2. Bu Derste Neler Yaptık?
Sisteme üç farklı veritabanı "mış gibi" (sadece `Console.WriteLine` ile) çalışan sınıf ekledik: `PostgresUserRepository`, `MySqlUserRepository` ve `SqliteUserRepository`. Ancak en önemlisi, bu sınıfları başıboş bırakmadık; hepsine bir **Sözleşme (Interface)** imzalattık.

### A. Sözleşme (Interface) Tanımlamak
Bir Interface oluşturarak sistemin anayasasını yazdık. Dedik ki: "Veritabanı işlemi yapacak her sınıf bu kurallara uymak ZORUNDADIR."

```csharp
public interface IUserRepository
{
    void Add();
    void GetAll();
}
```

### B. Polimorfizm (Çok Biçimlilik) ile Şalter Mantığı
Bu kuralı koyduktan sonra `Program.cs` içinde muazzam bir esneklik kazandık. Artık tipi `PostgresUserRepository` olan bir değişken değil, tipi `IUserRepository` olan bir değişken tanımlayıp, içine istediğimiz veritabanını koyabildik.

```csharp
IUserRepository db;
string secim = "postgres"; // Bu bir şalterdir

if (secim == "postgres")
    db = new PostgresUserRepository(); // Kurala uyduğu için kabul edilir.
else
    db = new SqliteUserRepository();

// Kodun geri kalanı hangi db'nin seçildiğini umursamaz!
db.Add(); 
```

---

## 3. Mimari Analiz ve Felsefe
* **Open/Closed Principle (Açık/Kapalı Prensibi):** Projeye MySQL eklemek istediğimizde var olan kodları (Add metotunu çağıran yerleri) hiç bozmadık. Sadece yeni bir `MySqlUserRepository` sınıfı ekledik. Sistem gelişime AÇIK, değişime KAPALI hale geldi.
* **Gevşek Bağlılık (Loose Coupling):** Kodun ilerleyen kısımları "Postgres" ile çalıştığını bilmiyor. Sadece `db.Add()` diyor. İş yapan kısımlar, aletlerden (Postgres/MySQL) bağımsızlaştırıldı.

---

## 4. Eksiklikler Nelerdi? (Bizi Ders 3'e Götüren Sebepler)
* **Her Şey Hayal Ürünüydü:** Kodumuz harika bir tasarıma ulaştı ama ekrana sadece `Console.WriteLine("Postgres'e eklendi")` yazıyordu. Gerçek dünyada bir SQL tablosuna veri gitmiyordu.
* **SQL ve Veritabanı Bilgisi Eksikliği:** Bir C# programı, dışarıdaki gerçek bir veritabanı motoruyla (Engine) nasıl konuşur? Araya hangi tüneller açılır? Microsoft'un bu iş için kullandığı temel yapı olan **ADO.NET** nedir?
Bu "Gerçek Dünya" eksikliği, bizi hemen **Ders 3**'e itti.
