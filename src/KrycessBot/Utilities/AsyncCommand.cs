using KrycessBot.Utilities.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KrycessBot.Utilities
{
    internal class AsyncCommand : IAsyncCommand
    {
        bool executing;
        readonly Func<bool> canExecute;
        readonly Func<Task> execute;

        public AsyncCommand(
            Func<Task> execute,
            Func<bool> canExecute = null)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute() =>
            !executing && (canExecute?.Invoke() ?? true);

        public async Task Execute()
        {
            if (CanExecute())
            {
                try
                {
                    executing = true;
                    await execute();
                }
                finally
                {
                    executing = false;
                }
                RaiseCanExecuteChanged();
            }
        }

        #region Explicit

        bool ICommand.CanExecute(object parameter) =>
            CanExecute();

        void ICommand.Execute(object parameter) =>
            Execute().GetAwaiter();

        #endregion
    }

    internal class AsyncCommand<T> : IAsyncCommand<T>
    {
        bool executing;
        readonly Func<T, bool> canExecute;
        readonly Func<T, Task> execute;

        public AsyncCommand(
            Func<T, Task> execute,
            Func<T, bool> canExecute = null)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(T parameter) =>
            !executing && (canExecute?.Invoke(parameter) ?? true);

        public async Task Execute(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    executing = true;
                    await execute(parameter);
                }
                finally
                {
                    executing = false;
                }
                RaiseCanExecuteChanged();
            }
        }

        #region Explicit

        bool ICommand.CanExecute(object parameter) =>
            CanExecute((T)parameter);

        void ICommand.Execute(object parameter) =>
            Execute((T)parameter).GetAwaiter();

        #endregion
    }
}
