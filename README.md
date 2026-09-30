# 🚀 .NET Core Öğrenme Serüveni (Dotnet_learn)

Merhabalar! Bu depo (repository), benim ASP.NET Core Web API dünyasına adım atarken adım adım ilerlediğim, öğrendiklerimi uyguladığım ve pratik yaptığım dersleri içermektedir. 

**Bu arşivi kendimce, acizane bir fikir olarak; hem kendi gelişimimi takip etmek hem de dönüp bakabileceğim bir referans kaynağı oluşturmak amacıyla hazırladım.** Eğer benzer bir yoldan geçiyorsanız, umarım sizin için de faydalı bir kaynak olur.

---

## 📂 Proje Yapısı ve Dersler

Şu an bu repoda toplam **14 adet ders/aşama** bulunmaktadır. Her bir ders klasörü (`lesson1`, `lesson2` ... `lesson14`), bir önceki dersin üzerine yeni konseptler konularak inşa edilmiştir.

Bu serüvende sıfırdan başlayarak yavaş yavaş "Clean Architecture" (Temiz Mimari) prensiplerine geçiş yaptık ve sonunda "Taskify" tarzı bir Görev Yönetim Sistemi (Task Management) uygulaması geliştirdik.

### 🛠️ Öğrenilen ve Uygulanan Temel Teknolojiler
- **C# & .NET 8/10 Web API:** Temel API mimarisi ve Controller yapıları.
- **Entity Framework Core (Code-First):** Veritabanı modellemesi ve LINQ sorguları.
- **PostgreSQL:** İlişkisel veritabanı yönetimi.
- **Katmanlı Mimari (Clean Architecture):** `Domain`, `Application`, `Infrastructure`, ve `API` katmanlarının ayrıştırılması.
- **Tasarım Kalıpları (Design Patterns):** Generic Repository Pattern, Unit of Work Pattern.
- **Docker & Docker Compose:** Projenin (özellikle veritabanının) konteynerize edilerek ayağa kaldırılması (Örn: `lesson9` ve sonrasında).
- **Servis Katmanı:** Controller'ı iş mantığından arındırıp `TaskService` gibi servis sınıfları kullanma.

---

## 📝 Derslerin Genel Akışı

*   **İlk Dersler (Basit Başlangıçlar):** Proje yapısını tanıma, basit endpoint'ler oluşturma, HTTP metotları (GET, POST, PUT, DELETE).
*   **Orta Seviye (Veritabanı ve ORM):** Entity Framework Core kurulumu, Code-First yaklaşımı ile PostgreSQL'e bağlanma, Repository Pattern entegrasyonu.
*   **İleri Seviye (Mimari ve Docker):** Clean Architecture prensiplerine göre projeyi katmanlara bölme, Docker Compose ile bağımlılıkları yönetme, Unit of Work ile transaction yönetimi.
*   **Son Dersler (Lesson 14):** Artık oturmuş bir mimaride `TaskItem` entitisi üzerinden `TaskService` ve `TaskController` ile tam teşekküllü bir CRUD operasyonu.

---

## 💡 Son Söz

Yazılım, sürekli öğrenmeyi ve pratik yapmayı gerektiren bir yolculuk. Buradaki kodlar belki en mükemmel kodlar değil ama adım adım gelişimin en güzel kanıtı. Kendimce, nacizane bir fikir olarak oluşturduğum bu arşivin, benim gibi öğrenme hevesi olan herkese ilham vermesi dileğiyle...

İyi kodlamalar! 💻
