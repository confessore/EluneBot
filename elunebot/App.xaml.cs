using elunebot.extensions;
using elunebot.models;
using elunebot.services;
using elunebot.services.interfaces;
using elunebot.statics;
using elunebot.views;
using Fasm;
using GreyMagic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace elunebot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly IServiceProvider _serviceProvider;

        public App()
        {
            _serviceProvider = ConfigureServices();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Launch();
        }

        static InProcessMemoryReader? reader;
        /// <summary>
        /// memory reader instance
        /// </summary>
        internal static InProcessMemoryReader? Reader => 
            reader ??= new InProcessMemoryReader(Process.GetCurrentProcess());

        static Process? process;
        internal static Process? Process =>
            process ??= Process.GetCurrentProcess();

        static ManagedFasm? asm;
        internal static ManagedFasm? Asm =>
            asm ??= new ManagedFasm();

        bool Injected =>
            !Process.ProcessName.Contains(Strings.ExecutingName);
        
        void Launch()
        {
            try
            {
                if (Injected)
                {
                    var defaultWindow = new DefaultWindow();
                    defaultWindow.Closed += (sender, args) => { Environment.Exit(0); };
                    defaultWindow.Show();
                }
                else
                {
                    CheckFilesAndFolders();
                    CheckWoWPath();
                    ExecuteAsAdministrator(out var process);
                    process.WaitForInputIdle();
                    Thread.Sleep(2500);
                    var injection = _serviceProvider.GetRequiredService<InjectionService>();
                    injection.Inject(process.Id);
                    Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                QuitWithMessage(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        void CheckFilesAndFolders()
        {
            Paths.Nethost.CheckFile(resources.binaries.nethost);
            Paths.Bootstrap.CheckFile(resources.binaries.elunebot_bootstrap);
            Paths.Fasm.CheckFile(resources.binaries.fasmdll_managed);
            Paths.FastCall.CheckFile(resources.binaries.elunebot_fastcall);
            Paths.Navigation.CheckFile(resources.binaries.elunebot_navigation);
            Paths.BotBases.CheckDirectory();
            Paths.Logs.CheckDirectory();
            Paths.Plugins.CheckDirectory();
        }

        void CheckWoWPath()
        {
            var localStorage = _serviceProvider.GetRequiredService<ILocalStorageService>();
            var settings = localStorage.ReadSettings();
            if (settings != null)
            {
                if (!string.IsNullOrWhiteSpace(settings.WoWPath))
                    return;
            }
            else
            {
                settings = new Settings();
                var ofd = new OpenFileDialog()
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = "executable (*.exe)|*.exe",
                    FilterIndex = 1,
                    Title = "please locate and open WoW.exe"
                };
                if (ofd.ShowDialog() != DialogResult.OK ||
                    ofd.FileName == Assembly.GetEntryAssembly().Location ||
                    !ofd.FileName.ToLower().Contains(Strings.Process))
                    throw new Exception("the wow executable was not selected. exiting");
                else
                    settings.WoWPath = ofd.FileName;
                localStorage.WriteSettings(settings);
            }
        }

        void ExecuteAsAdministrator(out Process process)
        {
            var localStorage = _serviceProvider.GetRequiredService<ILocalStorageService>();
            var settings = localStorage.ReadSettings();
            if (settings == null)
                throw new Exception("settings file doesn't exist");
            else
            {
                if (string.IsNullOrWhiteSpace(settings.WoWPath))
                    throw new Exception("path was wrong");
                var tmp = new Process();
                process = tmp;
                tmp.StartInfo.FileName = settings.WoWPath;
                tmp.StartInfo.UseShellExecute = true;
                tmp.StartInfo.Verb = "runas";
                tmp.Start();
            }
        }

        void QuitWithMessage(string message)
        {
            MessageBox.Show(new Form() , message);
            Environment.Exit(0);
        }

        IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<ILocalStorageService, LocalStorageService>()
                .AddSingleton<InjectionService>()
                .BuildServiceProvider();
        }
    }
}
