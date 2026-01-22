using FluentValidation;
using OnboardingSIGDB1.Domain.DTOs;
using OnboardingSIGDB1.Domain.Utils;

namespace OnboardingSIGDB1.Domain.Validators;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("O campo Nome é obrigatório.")
            .Length(2, 150).WithMessage("O campo Nome deve ter entre 2 e 150 caracteres.");

        RuleFor(c => c.Cpf)
            .NotEmpty().WithMessage("O campo CPF é obrigatório.")
            .Must(CpfValidation.Validate).WithMessage("CPF inválido.");

        RuleFor(c => c.HiringDate)
            .GreaterThan(DateTime.MinValue).When(c => c.HiringDate.HasValue)
            .WithMessage("Data de Contratação inválida.");

        RuleFor(c => c.CompanyId)
            .GreaterThan(0).When(c => c.CompanyId.HasValue)
            .WithMessage("O campo CompanyId, se informado, deverá ser maior que 0.");
        
        RuleFor(c => c.Id)
            .Equal(0).When(c => c.Id.HasValue)
            .WithMessage("O Id não deve ser informado.");
    }
}