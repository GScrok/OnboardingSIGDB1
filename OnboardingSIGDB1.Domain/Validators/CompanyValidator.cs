using FluentValidation;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.Validators
{
    public class CompanyDtoValidator : AbstractValidator<CompanyDto>
    {
        public CompanyDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O campo Nome é obrigatório.")
                .Length(2, 150).WithMessage("O campo Nome deve ter entre 2 e 150 caracteres.");

            RuleFor(c => c.Cnpj)
                .NotEmpty().WithMessage("O campo CNPJ é obrigatório.")
                .Must(CnpjValidation.Validate).WithMessage("CNPJ inválido.");

            RuleFor(c => c.FoundationDate)
                .GreaterThan(System.DateTime.MinValue).When(c => c.FoundationDate.HasValue)
                .WithMessage("Data de Fundação inválida.");
        }
    }
}