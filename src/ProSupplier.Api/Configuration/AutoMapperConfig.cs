using AutoMapper;
using ProSupplier.Api.ViewModels;
using ProSupplier.Business.Models;

namespace ProSupplier.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, SupplierViewModel>().ReverseMap();
            CreateMap<Endereco, AddressViewModel>().ReverseMap();
            CreateMap<Produto, ProductViewModel>().ReverseMap();

            CreateMap<ProductImageViewModel, Produto>().ReverseMap();

            CreateMap<Produto, ProductViewModel>()
                .ForMember(dest => dest.NameSupplier, opt => opt.MapFrom(src => src.Fornecedor.Name));
        }
    }
}
