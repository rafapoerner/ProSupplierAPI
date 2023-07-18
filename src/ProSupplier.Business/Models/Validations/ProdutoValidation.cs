using FluentValidation;
using FluentValidation.Validators;

namespace ProSupplier.Business.Models.Validations
{
    public class ProdutoValidation : AbstractValidator<Produto>
    {
        public ProdutoValidation()
        {
            RuleFor( p => p.Name )
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 200).WithMessage("O campo {ProertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
            
            RuleFor( p => p.Descricao )
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 1000).WithMessage("O campo {ProertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(p => p.Valor)
                .GreaterThan("0")
                .WithMessage("The 'Valor' must be greater than zero.");
        }
    }
}
