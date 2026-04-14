using FluentValidation;
using GestionAbsences.DTOs.Class;

namespace GestionAbsences.Validators
{
    public class CreateClassDtoValidator : AbstractValidator<CreateClassDto>
    {
        public CreateClassDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Le nom de la classe est requis");
        }
    }
}
