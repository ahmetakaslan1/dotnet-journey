using AutoMapper;
using Lesson13.Domain;
using Lesson13.Application.DTOs;

namespace Lesson13.Application.Mappings;

// BÖLÜM 2: APPLICATION (İŞ MANTIĞI)
// AutoMapper Profil Sınıfı: Hangi sınıfın hangi sınıfa dönüşebileceğini (Mapping) burada tanımlıyoruz.
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User (Entity) -> UserResponseDto'ya kopyalanabilir.
        // Sektör Gerçeği: İki sınıftaki özellikleri (Id, Name, Email) isimlerine bakarak otomatik eşleştirir.
        CreateMap<User, UserResponseDto>();

        // UserCreateDto (Kullanıcıdan gelen veri) -> User (Entity) dönüşümü
        CreateMap<UserCreateDto, User>();
    }
}
