using CadastroAPI.TesteDeIntegracao.Fixtures;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using ModelData.ViewModel;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CadastroAPI.TesteDeIntegracao.Scenarios
{
    public class UsuarioTest
    {
        private readonly TestContexto _testContext;

        public UsuarioTest()
        {
            _testContext = new TestContexto();
        }

        private string GerarTokenDeAcesso()
        {
            var appSettings = TestHelper.GetApplicationConfiguration(NUnit.Framework.TestContext.CurrentContext.TestDirectory);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.ChaveSecretaToken);
            var dataDeValidade = DateTime.Now.AddHours(4);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "123"),
                    new Claim(ClaimTypes.Email, "wellington@teste.com")
                }),
                Expires = dataDeValidade,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [Theory]
        [InlineData("/api/usuario")]
        [InlineData("/api/usuario/1")]
        public async Task GaranteQueTodasAsOperacoesGETPrecisamDeAutorizacao(string operacao)
        {
            var response = await _testContext.Client.GetAsync(operacao);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/api/usuario")]
        //[InlineData("/api/Usuario/1/​AtualizarSenha")]
        public async Task GaranteQueTodasAsOperacoesPOSTPrecisamDeAutorizacao(string operacao)
        {
            var response = await _testContext.Client.PostAsync(operacao, new StringContent(string.Empty));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/api/usuario/1")]
        public async Task GaranteQueTodasAsOperacoesPUTPrecisamDeAutorizacao(string operacao)
        {
            var response = await _testContext.Server.CreateRequest(operacao)
                           .And(request => request.Method = HttpMethod.Put)
                           .SendAsync("PUT");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("/api/usuario/1")]
        public async Task GaranteQueTodasAsOperacoesDELETEPrecisamDeAutorizacao(string operacao)
        {
            var response = await _testContext.Server.CreateRequest(operacao)
                           .And(request => request.Method = HttpMethod.Delete)
                           .SendAsync("DELETE");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void RealizaOperacaoGetComSucessoEValidaAsInformacoesRetornada()
        {
            var qtdeDeClientesCadastrados = _testContext.Contexto.Usuario.Count();

            var token = GerarTokenDeAcesso();

            var response = _testContext.Server.CreateRequest("/api/usuario")
                          .AddHeader("Authorization", "Bearer " + token)
                          .GetAsync().Result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var retornoObj = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, new
            {
                totalDeRegistros = string.Empty,
                dados = new[]
                {
                    new
                    {
                        nomeCompleto = string.Empty
                    }
                }
            });

            retornoObj.totalDeRegistros.Should().Equals(qtdeDeClientesCadastrados);
            retornoObj.dados.Count().Should().Equals(qtdeDeClientesCadastrados);
        }

        [Theory]
        [InlineData("/api/usuario/1", 1)]
        [InlineData("/api/usuario/2", 2)]
        [InlineData("/api/usuario/3", 3)]
        [InlineData("/api/usuario/4", 4)]
        [InlineData("/api/usuario/5", 5)]
        public void RealizaOperacaoGetPorIdComSucessoEValidaAsInformacoesRetornadaComOBancoDeDados(string operacao, long id)
        {
            var cliente = _testContext.Contexto.Usuario.Where(c => c.Id == id).SingleOrDefault();

            var token = GerarTokenDeAcesso();

            var response = _testContext.Server.CreateRequest(operacao)
                          .AddHeader("Authorization", "Bearer " + token)
                          .GetAsync().Result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var retornoObj = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, new
            {
                id = string.Empty,
                nomeCompleto = string.Empty
            });

            retornoObj.id.Should().Equals(cliente.Id);
            retornoObj.nomeCompleto.Count().Should().Equals(cliente.NomeCompleto);
        }

        [Theory]
        [InlineData("/api/usuario/1")]
        public async Task RealizaOperacaoPutPorIdComSucessoEValidaAsInformacoesDosCampos(string operacao)
        {
            var novoValor = new UsuarioViewModel
            {
                NomeCompleto = "Wellington Alves da Silva",
                Email = "wasilva@hotmail.com"
            };

            var token = GerarTokenDeAcesso();

            //Realizar a atualização do usuário
            var response = await _testContext.Server.CreateRequest(operacao)
                           .AddHeader("Authorization", "Bearer " + token)
                           .And(request => {
                               request.Method = HttpMethod.Put;
                               request.Content = new StringContent(JsonConvert.SerializeObject(novoValor), UnicodeEncoding.UTF8, "application/json");
                           })
                           .SendAsync("PUT");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            //Agora verificar se o valor foi realmente atualizado conferindo através do get
            response = _testContext.Server.CreateRequest(operacao)
                          .AddHeader("Authorization", "Bearer " + token)
                          .GetAsync().Result;

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var retornoObj = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, new
            {
                id = string.Empty,
                nomeCompleto = string.Empty,
                email = string.Empty
            });

            retornoObj.nomeCompleto.Count().Should().Equals(novoValor.NomeCompleto);
            retornoObj.email.Count().Should().Equals(novoValor.Email);
        }
    }
}
