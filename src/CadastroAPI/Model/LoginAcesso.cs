using System.ComponentModel.DataAnnotations;

namespace CadastroAPI.Model
{
    public class LoginAcesso
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
