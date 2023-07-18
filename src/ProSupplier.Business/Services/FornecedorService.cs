using Microsoft.AspNetCore.Mvc;
using ProSupplier.Business.Interfaces;
using ProSupplier.Business.Models;
using ProSupplier.Business.Models.Validations;

namespace ProSupplier.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {

        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository,
                              IEnderecoRepository enderecoRepository,
                              INotifier notifier) : base(notifier) 
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            //Validar o estado da entidade
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
                || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Address)) return;

            if(_fornecedorRepository.Buscar( f => f.Document == fornecedor.Document).Result.Any())
            {
                Notificar("Já existe um fornecedor com esse documento informado.");
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            if (_fornecedorRepository.Buscar(f => f.Document == fornecedor.Document && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com esse documento informado");
                return;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task Remover(Guid id)
        {
            if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Products.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados.");
                return;
            }

            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);
            
            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }

            await _fornecedorRepository.Remover(id);
        }


        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose(); 
        }

    }
}
