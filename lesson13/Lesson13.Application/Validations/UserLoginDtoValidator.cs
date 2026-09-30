using FluentValidation;
using Lesson13.Application.DTOs;

namespace Lesson13.Application.Validations;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta alanı boş bırakılamaz!")
            .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi girin.");

        RuleFor(x => x.RawPassword)
            .NotEmpty().WithMessage("Şifre alanı boş bırakılamaz!");
    }
}
