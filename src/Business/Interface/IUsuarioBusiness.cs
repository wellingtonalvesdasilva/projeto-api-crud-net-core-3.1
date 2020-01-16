using ModelData.Model;
using ModelData.ViewModel;

namespace Business.Interface
{
    public interface IUsuarioBusiness : IBaseBusiness<Usuario, UsuarioViewModel>
    {
        void TrocarSenha(long id, UsuarioSenhaViewModel viewModel);
    }
}
