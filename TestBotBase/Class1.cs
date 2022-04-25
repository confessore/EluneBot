using elunebot.models.interfaces;
using elunebot.services.interfaces;
using System.ComponentModel.Composition;

namespace TestBotBase
{
    [Export(typeof(IBotBase))]
    public class TestBotBase : IBotBase
    {
        readonly IMemoryService _memory;
        readonly IObjectManagerService _objectManager;
        readonly ISpellService _spell;

        [ImportingConstructor]
        public TestBotBase(
            [Import] IMemoryService memory,
            [Import] IObjectManagerService objectManager,
            [Import] ISpellService spell)
        {
            _memory = memory;
            _objectManager = objectManager;
            _spell = spell;
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
                    //var path = navigation.CalculateLocation(objectManager.LocalPlayer.MapId, objectManager.LocalPlayer.Position, objectManager.NPCs.OrderBy(x => x.Position.DistanceTo(objectManager.LocalPlayer.Position)).FirstOrDefault().Position, true);
                    //await memory.ClickToMoveAsync(path[1]);
                    //await memory.ClickToMoveAsync(objectManager.Units.OrderBy(x => x.Position.DistanceTo(objectManager.LocalPlayer.Position)).FirstOrDefault().Position);
                    _memory.DoString("Jump()");

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(250);
                }
            }, token);
        }

        public void Stop()
        {
            if (cts != null)
                cts.Cancel();
        }

        public void ToggleGUI()
        {

        }
    }
}