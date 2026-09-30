using System;
using System.Collections.Generic;

namespace lesson5;

// İŞTE SEKTÖRDEKİ GERÇEK MİMARİ!
// Bu sınıf bir "Servis". Görevi kullanıcıları sisteme eklemek veya getirmek.
// DİKKAT: Bu sınıfın içinde ASLA "new PostgresUserRepository()" veya "new Sqlite..." YAZMAZ!


public class UserService
{
    private readonly IUserRepository _userRepository;

    // CONSTRUCTOR INJECTION (Yapıcı Metot Enjeksiyonu)
    // Sınıf yaratılırken "Bana kurala (IUserRepository) uyan herhangi bir veritabanı verin" der.
    public UserService(IUserRepository userRepository)
    {
         
        _userRepository = userRepository;
    }

    public void RegisterUser(string name)
    {
        // Burada isim kontrolü, şifre kontrolü gibi "Business Logic" (İş Kuralları) yapılabilir.
        // Biz direkt veritabanına atıyoruz. HANGİ VERİTABANI OLDUĞUNU UserService BİLMEZ BİLE!
        var newUser = new User { Name = name };
        _userRepository.Add(newUser);
    }

    public void PrintAllUsers()
    {
        var users = _userRepository.GetAll();
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, İsim: {user.Name}");
        }
    }
}
