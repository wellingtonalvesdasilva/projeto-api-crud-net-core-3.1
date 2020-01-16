using ModelData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Util;
using static Util.Enumeracao;

namespace ModelData.Context
{
    public class PopularDados
    {
        public void DadosIniciais(CadastroCRUDContext context)
        {
            if (!context.Usuario.Any())
            {
                var usuarios = new List<Usuario>();

                for (int i = 1; i <= 5; i++)
                    usuarios.Add(new Usuario
                    {
                        NomeCompleto = "Usuario " + i,
                        Email = $"usuario{i}@hotmail.com",
                        DataDeCriacao = DateTime.Now,
                        Senha = Constantes.SenhaDefault,
                        Status = (int)ESituacao.Ativo
                    });

                context.AddRange(usuarios);
                context.SaveChanges();
            }
        }
    }
}
