using MyProject.Application.UseCases;

namespace MyProject.Api.Core
{
    public interface ICommand<TData> : IUseCase
    {
        void Execute(TData data);
    }
}
