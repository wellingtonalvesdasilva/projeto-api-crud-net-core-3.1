using CadastroAPI.Interface;
using CadastroAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace CadastroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody]LoginAcesso login)
        {
            var tokenDeAcesso = _autenticacaoService.Autenticar(login);

            if (tokenDeAcesso == null)
                return BadRequest("Usuário ou senha errada");

            return Ok(tokenDeAcesso);
        }

    }
}