using FluentValidation;
using Lesson13.Application.DTOs;

namespace Lesson13.Application.Validations;

// UserCreateDto için güvenlik kalkanı (Zırh)
public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("İsim alanı boş bırakılamaz!")
            .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı boş bırakılamaz!")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi girin.");

        RuleFor(x => x.RawPassword)
            .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz!")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
    }
}
