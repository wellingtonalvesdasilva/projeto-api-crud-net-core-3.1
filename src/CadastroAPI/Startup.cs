using Business;
using Business.Interface;
using CadastroAPI.Helper;
using CadastroAPI.Interface;
using CadastroAPI.Mapper;
using CadastroAPI.Model;
using CadastroAPI.Services;
using Core.Arquitetura;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModelData.Context;
using ModelData.Model;
using Repository;
using System;
using System.Text;

namespace CadastroAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //configurar objetos fortemente tipados
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //Banco em Memória para poder publicar no Azure ou em qualquer servidor sem banco
            //services.AddDbContext<CadastroCRUDContext>(options => options.UseInMemoryDatabase(databaseName: "CrudCadastroDatabase"));

            services.AddDbContext<CadastroCRUDContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Registra a injeção de dependência dos repositorios
            services.AddScoped<IGenericRepository<Usuario>, UsuarioRepository>();

            //Registra a injeção de dependência das regras de negócios
            services.AddScoped<IUsuarioBusiness, UsuarioBusiness>();

            //Registra o mapeamento da view model
            MapeamentoViewModel.RegistrarMapeamento(services);

            //Preparando o Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "API - CRUD básico",
                    Description = "API em Asp .Net Core 3.1 para CRUD básico de qualquer tabela",
                    Contact = new OpenApiContact
                    {
                        Name = "Wellington Alves da Silva",
                        Email = "wellington.alvesdasilva@hotmail.com",
                        Url = new Uri("https://github.com/wellingtonalvesdasilva")
                    }
                });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Cabecalho de autorização no header. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                c.AddSecurityRequirement(securityRequirement);
            });

            //Configura o JWT
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.ChaveSecretaToken);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Configurar injeção dos serviços
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();

            //Customizar as exceções lançadas
            services.AddControllersWithViews(options => options.Filters.Add(typeof(ExceptionCustomizadaFilterAttribute)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseHttpsRedirection();

            //Preparando o Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "API v1 - CRUD básico");
            });

            app.UseRouting();

            //Política CORS
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }    
}
