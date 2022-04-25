using System;
using System.Windows.Input;

namespace elunebot.models
{
    sealed class Command : ICommand
    {
        bool _executing;
        readonly Action _action;
        readonly Func<bool> _canExecute;

        public Command(
            Action action,
            Func<bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter = null) =>
            !_executing && (_canExecute?.Invoke() ?? true);

        public void Execute(object? parameter)
        {
            if (CanExecute())
            {
                try
                {
                    _executing = true;
                    _action();
                }
                finally
                {
                    _executing = false;
                }
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
