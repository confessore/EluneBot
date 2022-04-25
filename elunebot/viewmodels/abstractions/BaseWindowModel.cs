using elunebot.statics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace elunebot.viewmodels.abstractions
{
    abstract class BaseWindowModel : INotifyPropertyChanged
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

        public ICommand[] Commands = Array.Empty<ICommand>();

        public string WindowTitle =>
            $"{Strings.ExecutingName} - {Strings.ExecutingVersion}";


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //if (Commands != null)
            //{
            //    foreach (var command in Commands)
            //        command.RaiseCanExecuteChanged();
            //}
        }

        #endregion
    }
}
