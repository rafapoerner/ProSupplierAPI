﻿using ProSupplier.Business.Interfaces;
using ProSupplier.Business.Models;
using ProSupplier.Business.Models.Validations;

namespace ProSupplier.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IUser _user;

        public ProdutoService(IProdutoRepository produtoRepository,
                              IUser user,
                              INotifier notifier) : base(notifier)
        {
            _produtoRepository = produtoRepository;   
        }


        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            var user = _user.GetUserId();

            await _produtoRepository.Adicionar(produto);    
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Atualizar(produto);
        }

        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);
        }


        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
