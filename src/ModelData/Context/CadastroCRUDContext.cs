using Microsoft.EntityFrameworkCore;
using ModelData.Model;

namespace ModelData.Context
{
    public class CadastroCRUDContext : DbContext
    {
        public CadastroCRUDContext(DbContextOptions<CadastroCRUDContext> options) : base(options)
        { }

        public DbSet<Usuario> Usuario { get; set; }
    }
}
