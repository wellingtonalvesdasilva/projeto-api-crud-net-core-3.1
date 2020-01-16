using ModelData.Model;

namespace Business.Interface
{
    public interface IBaseBusiness<TModel, TViewModel>
        where TModel : BaseEntity
        where TViewModel : class
    {
        void Alterar(long id, TViewModel viewModel);
        TModel Criar(TViewModel viewModel);
    }
}
