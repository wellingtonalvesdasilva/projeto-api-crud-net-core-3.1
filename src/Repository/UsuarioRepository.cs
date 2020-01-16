using Core.Arquitetura;
using ModelData.Context;
using ModelData.Model;

namespace Repository
{
    public class UsuarioRepository : GenericRepository<CadastroCRUDContext, Usuario>, IGenericRepository<Usuario>
    {
        public UsuarioRepository(CadastroCRUDContext context) : base(context)
        { }
    }
}
