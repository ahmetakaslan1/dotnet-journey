using System;
using lesson5;
using Microsoft.Extensions.DependencyInjection; // Sektör standartı DI kütüphanesi

// ========================================================================
// BÖLÜM 1: BAĞIMLILIK ENJEKSİYONU (DEPENDENCY INJECTION) KUTUSUNU HAZIRLAMA
// (Gerçek hayatta bu kısım Program.cs'nin en üstünde tek bir yerde yapılır)
// ========================================================================

var services = new ServiceCollection();

// İŞTE SES KAYDINDA SÖYLEDİĞİN O MEŞHUR "TEK YERDEN YÖNETİM" BURASI!
// Yarın veritabanını değiştirmek istersen SADECE BURADAKİ SATIRI DEĞİŞTİRECEKSİN.
// if-else YOK, switch-case YOK!

// Şu an Postgres kullanmak istiyoruz diyelim:
services.AddScoped<IUserRepository, PostgresUserRepository>(); 
// MySQL'e mi geçeceksin? Üstü sil, şunu yaz: services.AddScoped<IUserRepository, MySqlUserRepository>();

// UserService'imizi de sisteme tanıtıyoruz ki o da çağrılabilsin.
services.AddScoped<UserService>();

// Kutuyu kapat ve sistemi başlat!
var serviceProvider = services.BuildServiceProvider();


// ========================================================================
// BÖLÜM 2: UYGULAMANIN ÇALIŞMASI
// ========================================================================

Console.WriteLine("=== DEPENDENCY INJECTION (IoC) MİMARİSİ ===");

// "Ey sistem, bana bir UserService ver." diyoruz.
// Sistem UserService'i yaratıyor, onun constructor'ına bakıyor, "Aaa IUserRepository lazımmış" diyor.
// Yukarıdaki ayarımıza bakıp "Heh, PostgresUserRepository istenmiş" diyip onu da yaratıp UserService'in içine koyuyor!
// BÜTÜN BUNLARI BİZİM YERİMİZE ARKA PLANDA YAPIYOR! (new anahtar kelimesi HİÇ KULLANILMADI)

var userService = serviceProvider.GetRequiredService<UserService>();

Console.Write("\nLütfen veritabanına eklenecek kişinin adını girin: ");
string name = Console.ReadLine() ?? "Gizemli Kullanıcı";

// Sadece servisteki metodu çağırıyoruz. Hangi DB'ye gidiyor, nasıl gidiyor ruhumuz bile duymuyor!
userService.RegisterUser(name);

Console.WriteLine("\n--- Sistemdeki Tüm Kullanıcılar ---");
userService.PrintAllUsers();

Console.WriteLine("\n[NOT: Veritabanını değiştirmek istersen Program.cs'deki 17. satırı değiştirmen YETERLİ!]");
