using EluneBot.Interfaces;
using EluneBot.Services.Interfaces;
using EluneBot.Statics;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace TestBase
{
    [Export(typeof(IBase))]
    public class TestBase : IBase
    {
        readonly ILoggerService loggingService;
        readonly IObjectManagerService objectManager;

        [ImportingConstructor]
        public TestBase(
            [Import]ILoggerService loggingService,
            [Import]IObjectManagerService objectManager)
        {
            this.loggingService = loggingService;
            this.objectManager = objectManager;
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
