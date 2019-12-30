using KrycessBot.Game.Interfaces;
using KrycessBot.Interfaces;
using KrycessBot.Services.Interfaces;
using KrycessBot.Statics;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace TestBase
{
    [Export(typeof(IBase))]
    public class TestBase : IBase
    {
        readonly ILoggingService loggingService;
        readonly IObjectManager objectManager;
        readonly IMemoryService memoryService;

        [ImportingConstructor]
        public TestBase(
            [Import]ILoggingService loggingService,
            [Import]IObjectManager objectManager,
            [Import]IMemoryService memoryService)
        {
            this.loggingService = loggingService;
            this.objectManager = objectManager;
            this.memoryService = memoryService;
        }

        public string Name => "TestBase";

        public Version Version => new Version(0, 0, 0, 0);

        public string Author => "krycess";

        public void Dispose()
        {

        }

        public void Start()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await memoryService.EnumerateVisibleObjectsAsync();
                    foreach (var obj in objectManager.FinalObjects)
                        await loggingService.Log(Paths.GeneralLog, obj.Guid.ToString());
                }
            });
        }

        public void Stop()
        {

        }

        public void ToggleGUI()
        {

        }
    }
}
