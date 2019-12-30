using EluneBot.Statics;
using EluneBot.Utilities.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace EluneBot.ViewModels.Abstractions
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        DialogResult? result;
        public DialogResult? Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        bool enabled = true;
        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                OnPropertyChanged();
            }
        }

        public IAsyncCommand[] AsyncCommands = new IAsyncCommand[] { };

        public string WindowTitle =>
            $"{Strings.ExecutingName} - {Strings.ExecutingVersion}";


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (AsyncCommands != null)
            {
                foreach (var asyncCommand in AsyncCommands)
                    asyncCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
