using elunebot.viewmodels;
using System.Windows;

namespace elunebot.views
{
    public partial class DefaultWindow : Window
    {
        DefaultWindowModel _wm;

        public DefaultWindow()
        {
            DataContext = _wm = new DefaultWindowModel();
            Build();
        }
    }
}
