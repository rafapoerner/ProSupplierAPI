using ProSupplier.Business.Models.Validations.Docs;
using FluentValidation;

namespace ProSupplier.Business.Models.Validations
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {
        public FornecedorValidation()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser forncecido")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres ");

            When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () => 
            {
                RuleFor(f => f.Document.Length).Equal(CpfValidation.TamanhoCpf)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CpfValidation.Validar(f.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
            When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () => 
            {

                RuleFor(f => f.Document.Length).Equal(CnpjValidation.TamanhoCnpj)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CnpjValidation.Validar(f.Document)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });


        }
    }
}
