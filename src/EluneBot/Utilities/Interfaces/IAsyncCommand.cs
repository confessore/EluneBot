using System.Threading.Tasks;
using System.Windows.Input;

namespace EluneBot.Utilities.Interfaces
{
    internal interface IAsyncCommand : ICommand
    {
        bool CanExecute();
        Task Execute();
        void RaiseCanExecuteChanged();
    }

    internal interface IAsyncCommand<T> : ICommand
    {
        bool CanExecute(T parameter);
        Task Execute(T parameter);
        void RaiseCanExecuteChanged();
    }
}
