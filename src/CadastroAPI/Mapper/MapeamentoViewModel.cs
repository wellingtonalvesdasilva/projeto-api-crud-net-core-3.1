using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ModelData.Model;
using ModelData.ViewModel;

namespace CadastroAPI.Mapper
{
    public class MapeamentoViewModel
    {
        public static void RegistrarMapeamento(IServiceCollection services)
        {
            var configuracaoMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<UsuarioViewModel, Usuario>();
                config.CreateMap<Usuario, UsuarioViewModel>();
            });
            IMapper mapper = configuracaoMapper.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
