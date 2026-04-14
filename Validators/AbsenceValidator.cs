using FluentValidation;
using GestionAbsences.DTOs.Absence;

namespace GestionAbsences.Validators
{
    public class MarkAbsenceDtoValidator : AbstractValidator<MarkAbsenceDto>
    {
        public MarkAbsenceDtoValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0).WithMessage("L'identifiant de l'étudiant est requis");
            RuleFor(x => x.ClassId).GreaterThan(0).WithMessage("L'identifiant de la classe est requis");
            RuleFor(x => x.Date).NotEmpty().WithMessage("La date est requise");
        }
    }

    public class BulkMarkAbsenceDtoValidator : AbstractValidator<BulkMarkAbsenceDto>
    {
        public BulkMarkAbsenceDtoValidator()
        {
            RuleFor(x => x.ClassId).GreaterThan(0).WithMessage("L'identifiant de la classe est requis");
            RuleFor(x => x.Date).NotEmpty().WithMessage("La date est requise");
            RuleFor(x => x.StudentIds).NotEmpty().WithMessage("La liste des étudiants est requise");
        }
    }
}
