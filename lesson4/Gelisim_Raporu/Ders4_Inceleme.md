# Ders 4 İnceleme Raporu: Docker ve Çevresel Bağımsızlık

## 1. Önceki Dersten Kalan Hangi Sorunu Çözdük?
Ders 3'teki "herkes kendi bilgisayarına veritabanı kurmak zorunda" krizini, **Docker ve `docker-compose.yml`** kullanarak kökünden çözdük. Projemiz artık tamamen izole ve kendi veritabanlarını yanında taşıyan bir yapıya dönüştü.

## 2. Bu Derste Neler Yaptık?
Proje ana dizinine bir `docker-compose.yml` dosyası oluşturduk. Bu dosya sayesinde bilgisayarımıza fiziksel olarak Postgres veya MySQL kurmadan, sadece `docker compose up -d` komutuyla sanal konteynerler içinde bu veritabanlarını saniyeler içinde ayağa kaldırdık.

### A. docker-compose.yml Mantığı ve Çevre Değişkenleri
Docker'a veritabanı için gerekli şifre ve kullanıcı adlarını `environment` değişkenleri üzerinden geçtik. Kodun içine şifre gömmek yerine, dışarıdan (ortamdan) ayar almasını sağladık.

```yaml
services:
  postgres-db:
    image: postgres:latest
    environment:
      POSTGRES_PASSWORD: mysecretpassword # Güvenli yapılandırma
    ports:
      - "5432:5432" # İçerideki 5432'yi, dışarıdaki kendi makinemizin 5432'sine bağladık
```

---

## 3. Mimari Analiz ve Felsefe
* **Çevresel Eşitlik (Environment Parity):** Sektörün altın kuralını uyguladık: *"Geliştiricinin bilgisayarı, test sunucusu ve canlı sunucu birbirinin TIPATIP aynısı olmalıdır."* Docker sayesinde işletim sistemimiz Windows, Mac veya Linux olsun fark etmeksizin kodumuz aynı Ubuntu/Alpine tabanlı konteynerde çalıştı.
* **İzolasyon:** Bilgisayarımızda artık çöp dosyalar yok. Docker'ı kapattığımızda veya `docker rm` yaptığımızda veritabanı sistemden tamamen ve kalıntısız bir şekilde siliniyor.

---

## 4. Eksiklikler Nelerdi? (Bizi Ders 5'e Götüren Sebepler)
* **Manuel Nesne Üretimi (Factory Çöplüğü):** `Program.cs`'in içinde hala çirkin bir `if-else` yapısı vardı. Hangi veritabanının kullanılacağını kendi ellerimizle kontrol edip `new` anahtar kelimesiyle objeleri BİZ üretiyorduk.
* **Ölçeklenemezlik:** Yarın bu projede 100 tane servis, 50 tane repository olduğunda, her seferinde *"Buna Postgres nesnesini gönder, şuna Email nesnesini gönder"* diye elle bağlamak yazılımcıyı delirtir.
* **Çözüm İhtiyacı:** Bize her şeyi yönetecek, "Kime ne lazımsa o bulup otomatik versin" diyeceğimiz merkezi bir Beyin / Kutu lazımdı. İşte bu acil ihtiyaç, bizi modern C#'ın kalbi olan **Ders 5'e (Dependency Injection)** itti.
