using FluentValidation;
using GestionAbsences.DTOs.Auth;
using GestionAbsences.DTOs.User;

namespace GestionAbsences.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100).WithMessage("Le prénom est requis");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100).WithMessage("Le nom est requis");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email invalide");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Le mot de passe doit contenir au moins 6 caractères");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Les mots de passe ne correspondent pas");
        }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email invalide");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Le mot de passe est requis");
        }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Role).NotEmpty().Must(r => r == "Admin" || r == "Professeur" || r == "Etudiant")
                .WithMessage("Le rôle doit être Admin, Professeur ou Etudiant");
        }
    }
}
