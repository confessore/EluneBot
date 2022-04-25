using elunebot.models;
using elunebot.models.interfaces;
using elunebot.services;
using elunebot.services.interfaces;
using elunebot.statics;
using elunebot.viewmodels.abstractions;
using GreyMagic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace elunebot.viewmodels
{
    sealed class DefaultWindowModel : BaseWindowModel
    {
        readonly IServiceProvider _serviceProvider;
        readonly IMainThreadService _mainThread;
        readonly IMemoryService _memory;
        readonly IObjectManagerService _objectManager;
        readonly IEndSceneService _endScene;
        readonly IHackService _hack;
        readonly ISpellService _spell;

        public DefaultWindowModel()
        {
            _serviceProvider = ConfigureServices();
            _memory = _serviceProvider.GetRequiredService<IMemoryService>();
            _objectManager = _serviceProvider.GetRequiredService<IObjectManagerService>();
            _hack = _serviceProvider.GetRequiredService<IHackService>();
            _spell = _serviceProvider.GetRequiredService<ISpellService>();
            ReloadBotBasesCommand = new Command(ReloadBotBases);
            StartBotBaseCommand = new Command(StartBotBase, CanStartBotBase);
            StopBotBaseCommand = new Command(StartBotBase, CanStartBotBase);
            ToggleGUICommand = new Command(ToggleGUI);
            //_mainThread = _serviceProvider.GetRequiredService<IMainThreadService>();
            //_endScene = _serviceProvider.GetRequiredService<IEndSceneService>();
            Debugger.Launch();
            _hack.HookWardenMemScan();
            Patches();
            ReloadBotBases();
            //_mainThread.Invoke(() => _endScene.ThrottleFPS());
        }

        void Patches()
        {
            var ctmPatch = new Hack(_memory, _objectManager, Offsets.Hacks.CtmPatch,
                new byte[] { 0x00, 0x00, 0x00, 0x00 }, "CtmPatch");
            _hack.AddHack(ctmPatch);
            ctmPatch.Apply();
            // Loot patch
            var LootPatch = new Hack(_memory, _objectManager, Offsets.Hacks.LootPatch, new byte[] { 0xEB }, "LootPatch");
            _hack.AddHack(LootPatch);
            LootPatch.Apply();

            var LootPatch2 = new Hack(_memory, _objectManager, Offsets.Hacks.LootPatch2, new byte[] { 0xEB }, "LootPatch2");
            _hack.AddHack(LootPatch2);
            LootPatch2.Apply();

            // Ctm Hide
            var CtmHide = new Hack(_memory, _objectManager, Offsets.LocalPlayer.CtmState, new byte[] { 0x0, 0x0, 0x0, 0x0 },
                new byte[] { 0x0C, 0x00, 0x00, 0x00 },
                "CtmHideHack")
            { DynamicHide = true };
            _hack.AddHack(CtmHide);
            CtmHide.Apply();

            var CtmHideX = new Hack(_memory, _objectManager, Offsets.LocalPlayer.CtmX, new byte[] { 0x0, 0x0, 0x0, 0x0 },
                new byte[] { 0x00, 0x00, 0x00, 0x00 },
                "CtmHideHackX")
            { DynamicHide = true };
            _hack.AddHack(CtmHideX);
            CtmHideX.Apply();

            var CtmHideY = new Hack(_memory, _objectManager, Offsets.LocalPlayer.CtmY, new byte[] { 0x0, 0x0, 0x0, 0x0 },
                new byte[] { 0x00, 0x00, 0x00, 0x00 },
                "CtmHideHackY")
            { DynamicHide = true };
            _hack.AddHack(CtmHideY);
            CtmHideY.Apply();

            var CtmHideZ = new Hack(_memory, _objectManager, Offsets.LocalPlayer.CtmZ, new byte[] { 0x0, 0x0, 0x0, 0x0 },
                new byte[] { 0x00, 0x00, 0x00, 0x00 },
                "CtmHideHackZ")
            { DynamicHide = true };
            _hack.AddHack(CtmHideZ);
            CtmHideZ.Apply();

            var luaUnlock = new Hack(_memory, _objectManager, Offsets.Hacks.LuaUnlock, new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xc3 },
                "LuaUnlock");
            _hack.AddHack(luaUnlock);
            luaUnlock.Apply();
        }

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

        IBotBase selectedBotBase;
        public IBotBase SelectedBotBase
        {
            get => selectedBotBase;
            set
            {
                selectedBotBase = value;
                OnPropertyChanged();
            }
        }

        IEnumerable<IBotBase> availableBotBases;
        [ImportMany(typeof(IBotBase), AllowRecomposition = true)]
        public IEnumerable<IBotBase> AvailableBotBases
        {
            get => availableBotBases;
            set
            {
                availableBotBases = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReloadBotBasesCommand { get; }

        void ReloadBotBases()
        {
            if (AvailableBotBases != null)
            {
                foreach (var botBase in AvailableBotBases)
                {
                    botBase.Stop();
                    botBase.Dispose();
                }
            }
            var catalog = new AggregateCatalog();
            foreach (var file in Directory.GetFiles(Paths.BotBases))
            {
                if (!file.EndsWith(".dll")) continue;
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.Load(File.ReadAllBytes(file))));
            }
            var container = new CompositionContainer(catalog);
            //container.ComposeExportedValue(logger);
            container.ComposeExportedValue(_memory);
            container.ComposeExportedValue(_mainThread);
            //container.ComposeExportedValue(navigation);
            container.ComposeExportedValue(_objectManager);
            container.ComposeExportedValue(_spell);
            container.ComposeParts(this);
            if (AvailableBotBases.Any())
                SelectedBotBase = AvailableBotBases.FirstOrDefault();
        }

        public ICommand StartBotBaseCommand { get; }
        bool CanStartBotBase() =>
            !Running && SelectedBotBase != null;

        void StartBotBase()
        {
            if (_memory.IsInGame())
            {
                Running = true;
                SelectedBotBase.Start();
            }
            else
                MessageBox.Show("must be in game to start");
        }

        public ICommand StopBotBaseCommand { get; }

        bool CanStopBase() =>
            Running && SelectedBotBase != null;

        void StopBase()
        {
            Running = false;
            SelectedBotBase.Stop();
        }

        public ICommand ToggleGUICommand { get; }

        void ToggleGUI()
        {
            if (SelectedBotBase != null)
                SelectedBotBase.ToggleGUI();
        }

        IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new InProcessMemoryReader(Process.GetCurrentProcess()))
                .AddSingleton<IMainThreadService, MainThreadService>()
                .AddSingleton<IMemoryService, MemoryService>()
                .AddSingleton<IObjectManagerService, ObjectManagerService>()
                .AddSingleton<IHackService, HackService>()
                .AddSingleton<ISpellService, SpellService>()
                //.AddSingleton<IEndSceneService, EndSceneService>()
                .BuildServiceProvider();
        }
    }
}
