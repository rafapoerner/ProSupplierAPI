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
    [Route("api/v{version:apiVersion}/suppliers")]
    public class SuplliersController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public SuplliersController(IFornecedorRepository fornecedorRepository,
                                   IFornecedorService fornecedorService,
                                   IEnderecoRepository enderecoRepository,
                                   INotifier notifier,
                                   IUser user,
                                   IMapper mapper) : base(notifier, user)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> GetAll()
        {
            var fornecedor = _mapper.Map<IEnumerable<SupplierViewModel>>(await _fornecedorRepository.ObterTodos());

            return fornecedor;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> GetById(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [ClaimsAuthorize("Supplier", "Create")]
        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Create(SupplierViewModel supplierViewModel)
        {


            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Update(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id)
            {
                NotificarErro("O Id informado não é o mesmo que foi passado na query.");
                return CustomResponse(supplierViewModel);
            };

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }


        [ClaimsAuthorize("Supplier", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpGet("endereço/{id:guid}")]
        public async Task<AddressViewModel> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<AddressViewModel>(await _enderecoRepository.ObterPorId(id));
        }

        [ClaimsAuthorize("Supplier", "Update")]
        [HttpPut("endereco/{id:guid}")]
        public async Task<ActionResult> AtualizarEndereco(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id)
            {
                NotificarErro("O Id informado não é o mesmo que foi passado na query.");
                return CustomResponse(addressViewModel);
            };

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(addressViewModel));

            return CustomResponse(addressViewModel);
        }



        private async Task<SupplierViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }

        private async Task<SupplierViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

    }

    //Debug.WriteLine("fornecedor: " + JsonConvert.SerializeObject(fornecedor));
}