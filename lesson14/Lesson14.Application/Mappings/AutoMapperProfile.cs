using AutoMapper;
using Lesson14.Domain;
using Lesson14.Application.DTOs;

namespace Lesson14.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Kullanıcı Kayıt DTO'sunu -> User Entity'sine dönüştür
        CreateMap<UserRegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Şifre düz metin geçmesin diye Ignore ediyoruz, kendimiz hashleyeceğiz.

        // Görev Oluşturma DTO'sunu -> TaskItem Entity'sine dönüştür
        CreateMap<TaskCreateDto, TaskItem>();

        // TaskItem Entity'sini -> Dışarıya DTO olarak (Veri sızdırmadan) gönder
        CreateMap<TaskItem, TaskResponseDto>();
    }
}
