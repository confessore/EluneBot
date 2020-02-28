using EluneBot.Interfaces;
using EluneBot.Services.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestBase
{
    [Export(typeof(IBase))]
    public class TestBase : IBase
    {
        readonly ILoggerService logger;
        readonly IMemoryService memory;
        readonly INavigationService navigation;
        readonly IObjectManagerService objectManager;

        [ImportingConstructor]
        public TestBase(
            [Import]ILoggerService logger,
            [Import]IMemoryService memory,
            [Import]INavigationService navigation,
            [Import]IObjectManagerService objectManager)
        {
            this.logger = logger;
            this.memory = memory;
            this.navigation = navigation;
            this.objectManager = objectManager;
        }

        CancellationTokenSource cts;
        CancellationToken token;

        public string Name => "TestBase";

        public Version Version => new Version(0, 0, 0, 0);

        public string Author => "krycess";

        public void Dispose()
        {

        }

        public void Start()
        {
            cts = new CancellationTokenSource();
            token = cts.Token;
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    //foreach (var unit in objectManager.Units)
                    //await logger.GeneralLog(unit.Position.DistanceTo(objectManager.LocalPlayer.Position).ToString());
                    var path = navigation.CalculateLocation(objectManager.LocalPlayer.MapId, objectManager.LocalPlayer.Position, objectManager.NPCs.OrderBy(x => x.Position.DistanceTo(objectManager.LocalPlayer.Position)).FirstOrDefault().Position, true);
                    await memory.ClickToMoveAsync(path[1]);
                    //await memory.ClickToMoveAsync(objectManager.Units.OrderBy(x => x.Position.DistanceTo(objectManager.LocalPlayer.Position)).FirstOrDefault().Position);
                    //await memory.DoStringAsync("DoEmote('sit')");

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100);
                }
            }, token);
        }

        public void Stop()
        {
            cts.Cancel();
        }

        public void ToggleGUI()
        {

        }
    }
}
