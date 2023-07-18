using System;
using System.Collections.Generic;

namespace ProSupplier.Business.Models
{
    public class Fornecedor : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public Endereco Address { get; set; }
        public bool Ativo { get; set; }
        
        /* EF Relations */
        public IEnumerable<Produto> Products { get; set; }

    }
}
