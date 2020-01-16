using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Core.Arquitetura
{
    /// <summary>
    /// Classe genérica que usa o padrão repositório para os modelos de dados, para fazer a camada de persistência e conexão com o banco de dados
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TEntity>
    where TContext : DbContext
    where TEntity : class
    {
        private readonly TContext _context;

        public GenericRepository(TContext context)
        {
            _context = context;
        }

        public TContext Contexto
        {
            get
            {
                return _context;
            }
        }

        public void Salvar(bool saveChanges = true)
        {
            if (saveChanges)
                Contexto.SaveChanges();
        }

        private DbSet<TEntity> ObterDbSet()
        {
            return Contexto.Set<TEntity>();
        }

        public virtual void Criar(TEntity entity, bool saveChanges = true)
        {
            if (entity == null)
                throw new ArgumentException("Não pode adicionar entidade nula");

            ObterDbSet().Add(entity);

            Salvar(saveChanges);
        }

        public virtual void Inserir(TEntity entity)
        {
            Criar(entity, true);
        }

        public virtual void Editar(TEntity entity, bool saveChanges = true)
        {
            ObterDbSet().Update(entity);
            Salvar(saveChanges);
        }

        public IQueryable<TEntity> ObterTodos()
        {
            return ObterDbSet();
        }

        public TEntity ObterPorID(params object[] IDs)
        {
            if (IDs[0] == null)
                return null;

            return ObterDbSet().Find(IDs);
        }
    }
}
