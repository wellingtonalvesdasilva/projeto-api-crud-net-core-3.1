using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroAPI.Model
{
    public class PaginacaoResultado<TViewModel>
    {
        public int PaginaAtual { get; set; }
        public int TotalDePaginas { get; set; }
        public int QuantidadePorPagina { get; set; }
        public int TotalDeRegistros { get; set; }

        public List<TViewModel> Dados { get; set; }
    }
}
