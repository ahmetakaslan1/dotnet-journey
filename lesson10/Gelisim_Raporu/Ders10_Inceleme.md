# Ders 10: Asenkron Programlama (async/await) ile Performans Patlaması

## Ne Öğrendik ve Ne Yaptık?
Bu derste sistemimizi senkron (kilitlenen) bir yapıdan, asenkron (uçan) bir yapıya geçirdik. 
Sektörde mülakatların 1 numaralı konusu olan `async` ve `await` mantığını projemize tamamen entegre ettik.

### Asenkron (Garson) Mantığı Nedir?
- **Eski Sistem (Senkron):** Garson siparişi alıp mutfağa (Veritabanına) gidiyor ve yemek pişene kadar kapıda dikilip bekliyordu. Diğer masalardaki müşterilerle (diğer API istekleriyle) ilgilenmiyordu.
- **Yeni Sistem (Asenkron):** Garson (Sunucu Thread'i) siparişi veritabanına iletir (Örneğin `await _context.SaveChangesAsync()`). Veritabanı veriyi kaydederken garson anında salona döner ve diğer kullanıcıların isteklerini yanıtlamaya başlar. Veritabanı işini bitirdiğinde garsona haber verir, garson da veriyi kullanıcıya iletir.

### Kodda Neler Değişti?
Aşağıdan yukarıya (Soğan Mimarisine uygun olarak) şu değişiklikleri yaptık:

1. **`IUserRepository.cs` (Arayüz):**
   `void Add(User user)` metodunu `Task AddAsync(User user)` olarak değiştirdik. `Task` kelimesi, bu işlemin gelecekte biteceğine dair bir söz (Promise) verir. Metodun sonuna `Async` yazmak sektörde değişmez bir kuraldır.

2. **`PostgresUserRepository.cs` (Altyapı):**
   `_context.SaveChanges()` yerine **`await _context.SaveChangesAsync()`** kullandık. EF Core'un bu efsanevi asenkron metodu sayesinde veritabanı I/O işlemleri sırasında sunucu işlemcisi (CPU) kilitlenmekten kurtuldu. Aynı şekilde listeleme için `await _context.Users.ToListAsync()` yazdık.

3. **`UserService.cs` ve `UserController.cs`:**
   En alttaki katman asenkron olunca, onu çağıran üst katmanlar da asenkron olmak **zorundadır**. (Asenkronluk bulaşıcıdır).
   Bu yüzden Servis ve Controller katmanlarındaki metodlarımızı da `async Task` yaparak, `await` kelimesiyle birbirlerini beklemelerini sağladık.

---

## Eksikliğimiz Ne? (Ufukta Ne Var?)
Sunucumuz artık 10.000 kişiyi aynı anda kaldıracak güce ulaştı. Ama hala büyük mimari eksiklerimiz var:

1. **Güvenlik Eksikliği:** Kullanıcıların JWT ile kimlik doğrulaması (Login sistemi) yapması ve şifrelerin BCrypt ile gizlenmesi gerekiyor.
2. **Doğrulama (Validation):** DTO'ların içindeki verilerin kurallara (Örn: Şifre en az 8 karakter olmalı) uyup uymadığını FluentValidation ile denetlemeliyiz.
3. **AutoMapper:** Entity'den DTO'ya dönüşümleri tek tek `Name = user.Name` yazmak (Kurye Hamallığı) yerine AutoMapper ile tek satırda otomatikleştirmeliyiz.
4. **Hata Yönetimi (Global Exception Handling):** Hataları Global Exception Middleware ile yakalayıp kullanıcıya temiz bir JSON (`{ "error": "Geçersiz E-posta" }`) dönmeliyiz.
