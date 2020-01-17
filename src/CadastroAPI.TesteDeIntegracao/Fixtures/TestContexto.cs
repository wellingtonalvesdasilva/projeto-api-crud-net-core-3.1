using CadastroAPI.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelData.Context;
using NUnit.Framework;
using System.Net.Http;

namespace CadastroAPI.TesteDeIntegracao.Fixtures
{
    public class TestContexto
    {
        public HttpClient Client { get; set; }
        public CadastroCRUDContext Contexto { get; set; }
        public TestServer Server { get; set; }

        public TestContexto()
        {
            ConfigurarClient();
        }

        private void ConfigurarClient()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(TestHelper.GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory))
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            Server = new TestServer(webHostBuilder);

            Client = Server.CreateClient();

            PopularDadosEmMemoria();
        }

        private void PopularDadosEmMemoria()
        {
            //Popular dados em memória
            Contexto = Server.Host.Services.GetService(typeof(CadastroCRUDContext)) as CadastroCRUDContext;
            new PopularDados().DadosIniciais(Contexto);
        }
    }

    public class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
                .AddEnvironmentVariables()
                .Build();
        }

        public static AppSettings GetApplicationConfiguration(string outputPath)
        {
            var configuration = new AppSettings();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("AppSettings")
                .Bind(configuration);

            return configuration;
        }
    }
}
