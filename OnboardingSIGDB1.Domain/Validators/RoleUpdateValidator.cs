using FluentValidation;
using OnboardingSIGDB1.Domain.DTOs;

namespace OnboardingSIGDB1.Domain.Validators;

public class RoleUpdateDtoValidator : AbstractValidator<RoleUpdateDto>
{
    public RoleUpdateDtoValidator()
    {
        RuleFor(r => r.Description)
            .NotEmpty().WithMessage("A descrição do cargo é obrigatória.")
            .Length(2, 250).WithMessage("A descrição deve ter entre 2 e 250 caracteres.");
    }
}