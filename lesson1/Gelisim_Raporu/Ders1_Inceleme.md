# Ders 1 İnceleme Raporu: Temeller ve Sıkı Bağımlılık (Tight Coupling)

## 1. Bu Derste Neler Yaptık?
Bu derste C# dilinin ve Nesne Yönelimli Programlamanın (OOP) en temel yapıtaşları olan sınıflar, nesneler ve veri tipleri üzerine çalıştık. Bütün kodlarımızı `Program.cs` isimli tek bir dosyanın içerisine yığarak, basit nesne üretme pratikleri yaptık.

### A. Sınıflar (Classes) ve Nesne Üretimi
C#'ta bir nesneyi temsil etmek için `User` ve `Car` gibi sınıflar (Class) tanımladık. Bu sınıflardan `new` anahtar kelimesini kullanarak bellekte canlı nesneler ürettik. Ayrıca nesne üretirken özelliklere tek satırda değer atamamızı sağlayan **Object Initializer** yapısını kullandık.

```csharp
// Geleneksel Yöntem
User user = new User();
user.Id = 10;

// Object Initializer Yöntemi (Daha Temiz)
Car car = new Car { id = 5 };
```

### B. Record Tipleri (Immutable Data)
C# 9.0 ile hayatımıza giren `record` tiplerini inceledik. Sınıflardan farklı olarak `record`'ların sadece veri taşımak için tasarlandığını ve bir kez oluşturulduktan sonra içindeki verilerin değiştirilemediğini (Immutable) öğrendik.

```csharp
// Record ile tanımlanan bir veri yapısı sonradan değiştirilemez.
var person = new Person("ali", 25);
// person.name = "veli"; // HATA VERİR!
```

---

## 2. Mimari Analiz ve Felsefe
Bu dersteki kodlama tarzı, programlamaya yeni başlayan herkesin yaptığı "dümdüz" bir yazım tarzıdır. Nesneler doğrudan `new` anahtar kelimesiyle oluşturulur ve kodun her yeri birbirine göbekten bağlıdır. 

Eğer bu şekilde devam edip bir e-ticaret sitesi yazsaydık, `UserService` isimli bir sınıfın içinde gidip `new PostgresRepository()` yazmak zorunda kalacaktık. Bu durum, yazılım mühendisliğinin en büyük günahlarından biri olan **Sıkı Bağımlılığa (Tight Coupling)** yol açar. Sistemdeki küçük bir değişiklik, bütün dosyaların değiştirilmesini gerektirirdi.

---

## 3. Eksiklikler Nelerdi? (Bizi Ders 2'ye Götüren Sebepler)
1. **Kuralsızlık:** Projede hiçbir standart (kural) yoktu. Yarın 3 farklı kişi veritabanı kayıt sınıfı yazsa, biri `Kaydet()`, diğeri `Ekle()`, diğeri `InsertUser()` adında metotlar yazabilirdi.
2. **Esneklik Yoktu:** Kodumuz sadece anlık işlemleri yapıyordu. Sistemi değiştirmeye (örneğin veritabanını değiştirmeye) kapalıydı.

Bu devasa mimari eksiklikler, bizi **Ders 2**'ye, yani **"Interface (Sözleşme) Kullanımına"** geçmek zorunda bıraktı.
