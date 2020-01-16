using AutoMapper;
using CadastroAPI.Model;
using System.Linq;

namespace CadastroAPI.Helper
{
    public class UtilMapeamento<TViewModel, TModel, TFilter> where TFilter : ParametroDePaginacao
    {
        public readonly IMapper _mapper;

        public UtilMapeamento(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Realiza o mapeamento do modelo para ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TViewModel PrepararRetorno(TModel model)
        {
            return _mapper.Map<TViewModel>(model);
        }

        /// <summary>
        /// Realiza o mapeamento de IQueryable do modelo para lista de ViewModel
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public PaginacaoResultado<TViewModel> PrepararRetorno(IQueryable<TModel> models, TFilter paginacao)
        {
            var dados = PaginacaoAutomatica<TModel, TViewModel>.Paginar(_mapper, models, paginacao.NumeroDaPagina, paginacao.QuantidadePorPagina);
            return new PaginacaoResultado<TViewModel>
            {
                PaginaAtual = dados.PaginaAtual,
                QuantidadePorPagina = dados.QuantidadePorPagina,
                TotalDePaginas = dados.TotalDePaginas,
                TotalDeRegistros = dados.TotalDeRegistros,
                Dados = dados
            };
        }
    }
}
