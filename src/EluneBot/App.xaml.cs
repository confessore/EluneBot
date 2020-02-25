using EluneBot.Extensions;
using EluneBot.Statics;
using EluneBot.Views;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

using DialogResult = System.Windows.Forms.DialogResult;
using Form = System.Windows.Forms.Form;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace EluneBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        [DllImport(Strings.Injector, CallingConvention = CallingConvention.Cdecl)]
        static extern bool Inject(int pid, string dll);

        protected override async void OnStartup(StartupEventArgs e)
        {
            await LaunchAsync();
            base.OnStartup(e);
        }

        Task<bool> InjectedAsync =>
            Task.FromResult(!System.Diagnostics.Process.GetCurrentProcess().ProcessName.Contains(Strings.ExecutingName));

        async Task LaunchAsync()
        {
            try
            {
                if (await InjectedAsync)
                    new MainView().Show();
                else
                {
                    await CheckFilesAndFoldersAsync();
                    await CheckWoWPathAsync();
                    await ExecuteAsAdministratorAsync(out var process);
                    Inject(process.Id, Paths.Loader);
                    Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                await QuitWithMessageAsync(e.InnerException.Message);
            }
        }

        Task CheckFilesAndFoldersAsync()
        {
            Paths.Settings.CheckFile(EluneBot.Properties.Resources.Settings);
            Paths.FastCall.CheckFile(EluneBot.Properties.Resources.EluneBot_FastCall);
            Paths.Injector.CheckFile(EluneBot.Properties.Resources.EluneBot_Injector);
            Paths.Loader.CheckFile(EluneBot.Properties.Resources.EluneBot_Loader);
            Paths.Bases.CheckDirectory();
            Paths.Logs.CheckDirectory();
            Paths.Plugins.CheckDirectory();
            return Task.CompletedTask;
        }

        Task CheckWoWPathAsync()
        {
            if (!string.IsNullOrEmpty(Settings.WoWPath))
                return Task.CompletedTask;
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "executable (*.exe)|*.exe",
                FilterIndex = 1,
                Title = "please locate and open WoW.exe"
            };
            if (ofd.ShowDialog() != DialogResult.OK ||
                ofd.FileName == Assembly.GetEntryAssembly().Location ||
                !ofd.FileName.Contains(Strings.Process))
                return Task.FromException(new Exception("the wow executable was not selected"));
            else
                Settings.WoWPath = ofd.FileName;
            return Task.CompletedTask;
        }

        Task ExecuteAsAdministratorAsync(out System.Diagnostics.Process process)
        {
            var tmp = new System.Diagnostics.Process();
            process = tmp;
            tmp.StartInfo.FileName = Settings.WoWPath;
            tmp.StartInfo.UseShellExecute = true;
            tmp.StartInfo.Verb = "runas";
            tmp.Start();
            return Task.CompletedTask;
        }

        Task QuitWithMessageAsync(string message)
        {
            MessageBox.Show(new Form() { TopMost = true }, message);
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
