using AutoMapper;
using Lesson11.Domain;
using Lesson11.Application.DTOs;

namespace Lesson11.Application.Mappings;

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
