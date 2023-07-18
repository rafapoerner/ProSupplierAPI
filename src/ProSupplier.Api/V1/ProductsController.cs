using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProSupplier.Api.Controllers;
using ProSupplier.Api.ViewModels;
using ProSupplier.Business.Interfaces;
using ProSupplier.Business.Models;
using static ProSupplier.Api.Extentions.CustomAuthorization;

namespace ProSupplier.Api.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProductsController(INotifier notifier,
                                  IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IUser user,
                                  IMapper mapper) : base(notifier, user)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetById(Guid id)
        {
            var productViewModel = await ObterProduto(id);
            if (productViewModel == null) return NotFound();

            return productViewModel;
        }

        [ClaimsAuthorize("Produto", "Create")]
        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + productViewModel.Imagem;

            if (!UploadArchive(productViewModel.ImagemUpload, imageName))
            {
                return CustomResponse(productViewModel);
            }

            productViewModel.Imagem = imageName;

            await _produtoService.Adicionar(_mapper.Map<Produto>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [ClaimsAuthorize("Produto", "Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, ProductViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse();
            }

            var produtoAtualizacao = await ObterProduto(id);

            if (string.IsNullOrEmpty(produtoViewModel.Imagem))
                produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
                if (!UploadArchive(produtoViewModel.ImagemUpload, imagemNome))
                {
                    return CustomResponse(ModelState);
                }

                produtoAtualizacao.Imagem = imagemNome;
            }

            produtoAtualizacao.FornecedorId = produtoViewModel.FornecedorId;
            produtoAtualizacao.Name = produtoViewModel.Name;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Atualizar")]
        [HttpPost("Adicionar")]
        public async Task<ActionResult<ProductViewModel>> CreateAddAlternative(ProductImageViewModel productImageViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagePrefix = Guid.NewGuid() + "_";

            if (!await UploadArchiveAlternative(productImageViewModel.ImagemUpload, imagePrefix))
            {
                return CustomResponse(ModelState);
            }

            productImageViewModel.Imagem = imagePrefix + productImageViewModel.ImagemUpload.FileName;

            await _produtoService.Adicionar(_mapper.Map<Produto>(productImageViewModel));

            return CustomResponse(productImageViewModel);
        }




        [ClaimsAuthorize("Produto", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var product = await ObterProduto(id);

            if (product == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(product);
        }

        private bool UploadArchive(string archive, string imgName)
        {
            if (string.IsNullOrEmpty(archive))
            {
                NotificarErro("Forneça uma imagem para este produto.");
                return false;
            }
            var imageDataByteArray = Convert.FromBase64String(archive);


            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imgName);
            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome.");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private async Task<bool> UploadArchiveAlternative(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }


        private async Task<ProductViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
        }

    }
}
