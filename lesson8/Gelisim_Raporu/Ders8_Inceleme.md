# Ders 8: DTO (Data Transfer Object) Kuryeleri ve [FromBody] Standardı

## Ne Öğrendik ve Ne Yaptık?
Bu derste Clean Architecture yapımızı zerre kadar bozmadan, API (Dış Dünya) ile Domain (Kalenin İçi) arasına devasa bir güvenlik duvarı ördük. 

1. **DTO Kuryeleri Devrede:**
Eskiden veritabanına kaydettiğimiz `User` sınıfını, olduğu gibi API'den dışarı (internete) yolluyorduk. Ya da kayıt olurken internetteki adam bize doğrudan `User` sınıfını yolluyordu. 
Bu sektörde affedilmez bir hatadır! Çünkü `User` sınıfının içine `Email` ve `PasswordHash` (Şifre) gibi aşırı gizli alanlar ekledik. Tapuyu (User) internete açarsan herkesin şifresi sızar.
Bunu çözmek için araya "Kuryeler" soktuk:
- **`UserCreateDto`:** İçeriye sadece İsim, Email ve Şifre (RawPassword) alabilen kayıt kuryesi.
- **`UserResponseDto`:** Dışarıya veri çıkarken şifreyi gizleyen, sadece İsim ve Email barındıran çıkış kuryesi.

2. **[FromBody] Standardı:**
Controller içinde dışarıdan veri alırken `public IActionResult Create([FromBody] UserCreateDto newUserDto)` yapısını kullandık. Sektörde .NET yazılımcıları API'nin gövdesinden (HTTP Body) gelen JSON verilerini modelle eşleştirmek için bunu açıkça yazarlar. (Bazı durumlarda .NET Core otomatik yapsa da, okunabilirlik ve kesinlik için `[FromBody]` yazmak en temiz sektörel standarttır).

---

## Eksikliğimiz Ne? (Amelelik Neden Devam Ediyor?)
DTO'ları kurduk ama `Infrastructure` (Altyapı) katmanında hala **"İlkel Çağlarda"** yaşıyoruz. 
`PostgresUserRepository.cs` dosyasının içine bakarsanız şunları görürsünüz:
- `CREATE TABLE IF NOT EXISTS Users...`
- `INSERT INTO Users...`

**Bu kodların en büyük tehlikesi şudur:**
Biz `User` sınıfına yeni bir özellik eklediğimizde (örneğin Yaş veya Email), bu SQL string'leri otomatik güncellenmez! Ya tablo eski halinde kalır hata fırlatır (Terminalde aldığınız `column "email" does not exist` hatası tam olarak buydu), ya da SQL string'lerini tek tek elle güncellemek zorunda kalırsınız. Bu manuel süreç tamamen "ameleliktir" ve büyük projelerde sistemi kesinlikle çökertir.

---

## Sonraki Derste (Ders 9) Ne Yapacağız?
Bu ameleliği bitirmeye, yani SQL yazmayı tamamen çöpe atmaya gidiyoruz.
**Entity Framework Core (ORM)** teknolojisine geçeceğiz.

Veritabanına manuel komutlar göndermek yerine, C#'a sadece şunu diyeceğiz:
*"Al sana benim `User.cs` sınıfım. Buna bak ve veritabanı tablolarını kendi kendine (Migrations ile) yarat! Sınıfa yeni özellik mi ekledim? Sen arka planda SQL'ini yazıp tabloyu kendin güncelle!"*

İşte bu yüzden, bir sonraki derste ORM'i (Tabloyu otomatik olarak Entity'den alma büyüsünü) kuracağız.
