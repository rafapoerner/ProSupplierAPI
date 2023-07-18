using ProSupplier.Business.Interfaces;
using ProSupplier.Business.Models;
using ProSupplier.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSupplier.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(MyDbContext db) : base(db) { }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking().Include(c => c.Address)
                                        .FirstOrDefaultAsync( c => c.Id == id); 
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await Db.Fornecedores.AsNoTracking()
                                        .Include(c => c.Products)
                                        .Include(c => c.Address)
                                        .FirstOrDefaultAsync( c => c.Id == id);
        }
    }
}
