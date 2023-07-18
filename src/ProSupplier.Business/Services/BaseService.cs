using ProSupplier.Business.Interfaces;
using ProSupplier.Business.Models;
using ProSupplier.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace ProSupplier.Business.Services
{
    public abstract class BaseService
    {

        private readonly INotifier _notifier;

        public BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        // Esse método vai propagar o erro até a camada de apresentação
        protected void Notificar(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        
        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            if (entidade == null)
            {
                throw new ArgumentNullException(nameof(entidade));
            }

            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
    }
}
