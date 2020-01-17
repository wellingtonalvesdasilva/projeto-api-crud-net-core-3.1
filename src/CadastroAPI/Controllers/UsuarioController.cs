using AutoMapper;
using Business.Interface;
using CadastroAPI.Model;
using Core.Arquitetura;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelData.Model;
using ModelData.ViewModel;

namespace CadastroAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseApiController<Usuario, UsuarioViewModel, FiltroUsuario>
    {
        private readonly IUsuarioBusiness _business;

        public UsuarioController(IGenericRepository<Usuario> repository, IMapper mapper, IUsuarioBusiness business) : base(repository, mapper)
        {
            _business = business;
        }

        /// <summary>
        /// Criar um registro
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioViewModel viewModel)
        {
            var validate = ValidarRequest(viewModel);

            if (validate != null)
                return validate;

            var model = _business.Criar(viewModel);

            return Ok(utilMapeamento.PrepararRetorno(model));
        }

        /// <summary>
        /// Atualizar um registro
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] UsuarioViewModel viewModel)
        {
            var validate = ValidarRequest(viewModel, id);

            if (validate != null)
                return validate;

            _business.Alterar(id, viewModel);

            return NoContent();
        }

        
        [HttpPost("{id}/AtualizarSenha")]
        public IActionResult AtualizarSenha(long id, [FromBody] UsuarioSenhaViewModel usuarioSenha)
        {
            var validate = ValidarRequest(usuarioSenha, id);

            if (validate != null)
                return validate;

            _business.TrocarSenha(id, usuarioSenha);

            return Ok();
        }

        private ObjectResult ValidarRequest<T>(T dados, long? id = null)
        {
            if (dados == null)
                return BadRequest("Informação nula");

            if (id.HasValue)
            {
                var data = repository.ObterPorID(id);
                if (data == null)
                    return NotFound("Informação não localizada");
            }

            return null;
        }
    }
}
