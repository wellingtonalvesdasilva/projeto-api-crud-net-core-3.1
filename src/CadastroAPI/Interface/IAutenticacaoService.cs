using CadastroAPI.Model;

namespace CadastroAPI.Interface
{
    public interface IAutenticacaoService
    {
        TokenDeAcesso Autenticar(LoginAcesso login);
    }
}
