using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroAPI.Model
{
    public class FiltroUsuario : ParametroDePaginacao
    {
        public string NomeCompleto { get; set; }
    }
}
