# projeto-api-crud-net-core-3.1
Projeto de uma API com CRUD Automático, aplicado designer patterns, testes unitários, documentação da API via Swagger, AutoMapper e Migrations

## Orientação

Para rodar utilizando Migrations e o banco SQL Server abrir o VS2019, marcar "CadastroApi" como "Set as StartUp Project" e depois ir até Tools -> Nugget Package Manage -> Package Manager Console -> Selectionar como Default project: ModelData, e aplicar o comando abaixo:

``` csharp
Update-Database
```

Com esse comando a aplicação irá criar as estruturas do banco de dados.

Em sequência apertar F5 o projeto será compilado e executado, e apresentará a tela do Swagger, caso não apareça digite: https://localhost:suaporta/swagger

Se por ventura optar por querer rodar em memória, segue as orientações:

Ir até o projeto CadastroAPI -> Startup.cs e descomentar o código da linha 46 do arquivo ficando assim:

``` csharp
services.AddDbContext<CadastroCRUDContext>(options => options.UseInMemoryDatabase(databaseName: "CrudCadastroDatabase"));
```
Em seguida comentar o código da linha 48, ficando assim:
``` csharp
//services.AddDbContext<CadastroCRUDContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```

Basicamente é isso, e para demonstração de uso da API, utilizar o Swagger, pois esse projeto contempla apenas o back-end.
