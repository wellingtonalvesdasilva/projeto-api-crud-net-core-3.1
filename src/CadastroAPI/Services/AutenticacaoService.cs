using CadastroAPI.Interface;
using CadastroAPI.Model;
using Core.Arquitetura;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelData.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CadastroAPI.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly AppSettings _appSettings;
        private readonly IGenericRepository<Usuario> _repository;

        public AutenticacaoService(IGenericRepository<Usuario> repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }

        public TokenDeAcesso Autenticar(LoginAcesso login)
        {
            var usuario = _repository.ObterTodos().SingleOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);

            if (usuario == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.ChaveSecretaToken);
            var dataDeValidade = DateTime.Now.AddHours(4);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = dataDeValidade,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenDeAcesso
            {
                Token = tokenHandler.WriteToken(token),
                ValidoAte = dataDeValidade
            };
        }
    }
}
