
using Microsoft.EntityFrameworkCore;
using ProSupplier.Business.Models;

namespace ProSupplier.Data.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Endereco> Enderecos{ get; set; }
        public DbSet<Fornecedor> Fornecedores{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Para evitar que, ao esquecer de mapear algum varchar, ele faça.
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                     .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)") ;

            //Faz o mapeamento de todas as entidades do DbSet de uma vez só.
            modelBuilder.ApplyConfigurationsFromAssembly((typeof(MyDbContext).Assembly));

            // Para evitar fazer o delete nos filhos também.
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;



            base.OnModelCreating(modelBuilder);
        }
    }
}
