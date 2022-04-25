using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace elunebot.views
{
    public partial class DefaultWindow
    {
        void Build()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            Title = $"{assemblyName.Name} - {assemblyName.Version}";
            Height = 300;
            Width = 450;
            ResizeMode = ResizeMode.NoResize;
            Content = DefaultStackPanel();
        }
         
        StackPanel DefaultStackPanel()
        {
            var stackPanel = new StackPanel();
            var comboBox = new ComboBox()
            {
                Margin = new Thickness(5, 5, 5, 5),
                IsEditable = false,
                IsSynchronizedWithCurrentItem = true
            };
            comboBox.SetBinding(ComboBox.ItemsSourceProperty, nameof(_wm.AvailableBotBases));
            comboBox.SetBinding(ComboBox.SelectedItemProperty, nameof(_wm.SelectedBotBase));
            var button0 = new Button()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Content = "reload",
                Command = _wm.ReloadBotBasesCommand
            };
            var button1 = new Button()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Content = "start",
                Command = _wm.StartBotBaseCommand
            };
            var button2 = new Button()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Content = "stop",
                Command = _wm.StopBotBaseCommand
            };
            var button3 = new Button()
            {
                Margin = new Thickness(5, 5, 5, 5),
                Content = "toggle gui",
                Command = _wm.ToggleGUICommand
            };
            stackPanel.Children.Add(comboBox);
            stackPanel.Children.Add(button0);
            stackPanel.Children.Add(button1);
            stackPanel.Children.Add(button2);
            stackPanel.Children.Add(button3);
            return stackPanel;
        }
    }
}
