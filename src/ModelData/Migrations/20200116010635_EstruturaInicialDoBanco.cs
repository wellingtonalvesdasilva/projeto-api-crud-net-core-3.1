using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelData.Migrations
{
    public partial class EstruturaInicialDoBanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataDeCriacao = table.Column<DateTime>(nullable: false),
                    DataDeEdicao = table.Column<DateTime>(nullable: true),
                    DataDeRemocao = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    NomeCompleto = table.Column<string>(maxLength: 400, nullable: false),
                    Email = table.Column<string>(maxLength: 500, nullable: false),
                    Senha = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
