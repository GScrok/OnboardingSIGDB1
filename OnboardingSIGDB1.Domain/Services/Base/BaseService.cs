using FluentValidation;
using FluentValidation.Results;
using OnboardingSIGDB1.Domain.Entities.Base;
using OnboardingSIGDB1.Domain.Interfaces;
using OnboardingSIGDB1.Domain.Notifications;

namespace OnboardingSIGDB1.Domain.Services.Base;

public abstract class BaseService
{
    private readonly INotificator _notificator;

    protected BaseService(INotificator notificator)
    {
        _notificator = notificator;
    }

    protected void Notify(string message)
    {
        _notificator.Handle(new Notification(message));
    }

    protected void Notify(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }
    
    protected bool ExecuteValidation<TV, TE>(TV validation, TE entity) 
        where TV : AbstractValidator<TE>
    {
        ValidationResult? validator = validation.Validate(entity);

        if (validator.IsValid) return true;

        Notify(validator);
        return false;
    }
}