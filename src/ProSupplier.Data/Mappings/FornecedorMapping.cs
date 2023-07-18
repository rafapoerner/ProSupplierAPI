using ProSupplier.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProSupplier.Data.Mappings
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Fornecedor : Endereco
            builder.HasOne(f => f.Address)
                .WithOne(e => e.Fornecedor);
            // 1 : N => Fornecedor : Produtos
            builder.HasMany(f => f.Products)
                .WithOne(p => p.Fornecedor)
                .HasForeignKey( p => p.FornecedorId );

            builder.ToTable("Fornecedores");

        }
    }

}
