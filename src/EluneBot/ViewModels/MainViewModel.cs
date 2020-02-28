using EluneBot.Interfaces;
using EluneBot.Services;
using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using EluneBot.Utilities;
using EluneBot.Utilities.Interfaces;
using EluneBot.ViewModels.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using MessageBox = System.Windows.Forms.MessageBox;

namespace EluneBot.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        readonly ILoggerService logger;
        readonly IMemoryService memory;
        readonly INavigationService navigation; 
        readonly IObjectManagerService objectManager;
        readonly IEndSceneService endScene;
        readonly IPatchService patch;

        public MainViewModel()
        {
            Services = ConfigureServicesAsync().GetAwaiter().GetResult();
            logger = Services.GetRequiredService<ILoggerService>();
            memory = Services.GetRequiredService<IMemoryService>();
            navigation = Services.GetRequiredService<INavigationService>();
            objectManager = Services.GetRequiredService<IObjectManagerService>();
            endScene = Services.GetRequiredService<IEndSceneService>();
            patch = Services.GetRequiredService<IPatchService>();
            ReloadBasesAsync();
            ReloadBasesAsyncCommand = new AsyncCommand(ReloadBasesAsync);
            StartBaseAsyncCommand = new AsyncCommand(StartBaseAsync, CanStartBase);
            StopBaseAsyncCommand = new AsyncCommand(StopBaseAsync, CanStopBase);
            ToggleGUIAsyncCommand = new AsyncCommand(ToggleGUIAsync);
            AsyncCommands = new IAsyncCommand[]
            {
                ReloadBasesAsyncCommand,
                StartBaseAsyncCommand,
                StopBaseAsyncCommand,
                ToggleGUIAsyncCommand
            };
        }

        IServiceProvider Services { get; }

        public IAsyncCommand ReloadBasesAsyncCommand { get; }
        public IAsyncCommand StartBaseAsyncCommand { get; }
        public IAsyncCommand StopBaseAsyncCommand { get; }
        public IAsyncCommand ToggleGUIAsyncCommand { get; }

        bool running;
        public bool Running
        {
            get => running;
            set
            {
                running = value;
                OnPropertyChanged();
            }
        }

        IBase selectedBase;
        public IBase SelectedBase
        {
            get => selectedBase;
            set
            {
                selectedBase = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<IBase> availableBases;
        [ImportMany(typeof(IBase), AllowRecomposition = true)]
        public ObservableCollection<IBase> AvailableBases
        {
            get => availableBases;
            set
            {
                availableBases = value;
                OnPropertyChanged();
            }
        }

        Task ReloadBasesAsync()
        {
            if (AvailableBases != null)
            {
                foreach (var @base in AvailableBases)
                {
                    @base.Stop();
                    @base.Dispose();
                }
            }
            var catalog = new AggregateCatalog();
            foreach (var file in Directory.GetFiles(Paths.Bases))
            {
                if (!file.EndsWith(".dll")) continue;
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.Load(File.ReadAllBytes(file))));
            }
            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue(logger);
            container.ComposeExportedValue(memory);
            container.ComposeExportedValue(navigation);
            container.ComposeExportedValue(objectManager);
            container.ComposeParts(this);
            if (AvailableBases.Count > 0)
                SelectedBase = AvailableBases[0];
            return Task.CompletedTask;
        }

        bool CanStartBase() =>
            !Running && SelectedBase != null;

        async Task StartBaseAsync()
        {
            if (await memory.IsInGameAsync())
            {
                Running = true;
                SelectedBase.Start();
            }
            else
                MessageBox.Show("must be in game to start");
        }

        bool CanStopBase() =>
            Running && SelectedBase != null;

        Task StopBaseAsync()
        {
            Running = false;
            SelectedBase.Stop();
            return Task.CompletedTask;
        }

        Task ToggleGUIAsync()
        {
            if (SelectedBase != null)
                SelectedBase.ToggleGUI();
            return Task.CompletedTask;
        }

        Task<IServiceProvider> ConfigureServicesAsync()
        {
            return Task.FromResult<IServiceProvider>(
                new ServiceCollection()
                .AddSingleton<ILoggerService, LoggerService>()
                .AddSingleton<IMemoryService, MemoryService>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<IObjectManagerService, ObjectManagerService>()
                .AddSingleton<IEndSceneService, EndSceneService>()
                .AddSingleton<IPatchService, PatchService>()
                .BuildServiceProvider());
        }
    }
}
