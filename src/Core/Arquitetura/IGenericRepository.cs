using System.Linq;

namespace Core.Arquitetura
{
    /// <summary>
    /// Interface que contém as operações que serão realizadas pelo padrão repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Criar(TEntity entity, bool saveChanges = true);

        void Editar(TEntity entity, bool saveChanges = true);

        void Salvar(bool saveChanges = true);

        IQueryable<TEntity> ObterTodos();

        TEntity ObterPorID(params object[] IDs);
    }
}
