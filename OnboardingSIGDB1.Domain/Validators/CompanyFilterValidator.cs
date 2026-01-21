using FluentValidation;
using OnboardingSIGDB1.Domain.Filters;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.Validators

{
    public class CompanyFilterValidator : AbstractValidator<CompanyFilter>
    {
        public CompanyFilterValidator()
        {
            RuleFor(f => f.Name)
                .MaximumLength(150).WithMessage("O nome para filtro não pode ultrapassar 150 caracteres.");
            
            RuleFor(f => f.Cnpj)
                .Must(CnpjValidation.Validate).WithMessage("O CNPJ informado no filtro é inválido.")
                .When(f => !string.IsNullOrEmpty(f.Cnpj));
            
            RuleFor(f => f.StartDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Data de início inválida.")
                .When(f => f.StartDate.HasValue);
            
            RuleFor(f => f.EndDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Data de fim inválida.")
                .When(f => f.EndDate.HasValue);

            RuleFor(f => f.EndDate)
                .GreaterThanOrEqualTo(f => f.StartDate.Value)
                .WithMessage("A data final deve ser maior ou igual a data de início.")
                .When(f => f.StartDate.HasValue && f.EndDate.HasValue);
        }
    }
}