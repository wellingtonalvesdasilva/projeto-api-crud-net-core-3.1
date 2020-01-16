using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CadastroAPI.Helper
{
    public class PaginacaoAutomatica<TModel, TViewModel> : List<TViewModel>
    {
        const int qtdeDeRegistrosPorPaginaDefault = 20;

        public int PaginaAtual { get; private set; }
        public int TotalDePaginas { get; private set; }
        public int QuantidadePorPagina { get; private set; }
        public int TotalDeRegistros { get; private set; }

        public PaginacaoAutomatica(List<TViewModel> items, int totalDeRegistros, int numeroDaPagina, int quantidadePorPagina)
        {
            TotalDeRegistros = totalDeRegistros;
            QuantidadePorPagina = quantidadePorPagina;
            PaginaAtual = numeroDaPagina;
            TotalDePaginas = (int)Math.Ceiling(totalDeRegistros / (double)quantidadePorPagina);
            AddRange(items);
        }

        public static PaginacaoAutomatica<TModel, TViewModel> Paginar(IMapper mapper, IQueryable<TModel> dados, int? numeroDaPaginaParameter, int? quantidadePorPaginaParameter)
        {
            var numeroDaPagina = numeroDaPaginaParameter ?? 1;
            var quantidadePorPagina = quantidadePorPaginaParameter ?? qtdeDeRegistrosPorPaginaDefault;

            var count = dados.Count();
            var items = dados.Skip((numeroDaPagina - 1) * quantidadePorPagina).Take(quantidadePorPagina).ToList();

            var lista = new List<TViewModel>();
            foreach (var model in items)
                lista.Add(mapper.Map<TViewModel>(model));

            return new PaginacaoAutomatica<TModel, TViewModel>(lista, count, numeroDaPagina, quantidadePorPagina);
        }
    }
}
