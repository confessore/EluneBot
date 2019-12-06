using KrycessBot.Interfaces;
using KrycessBot.Services.Interfaces;
using System;
using System.ComponentModel.Composition;
using System.IO;

namespace TestBase
{
    [Export(typeof(IBase))]
    public class TestBase : IBase
    {
        readonly IMemoryService memoryService;

        [ImportingConstructor]
        public TestBase([Import]IMemoryService memoryService)
        {
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
            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                var tmp = await memoryService.ClassAsync();
                File.WriteAllText("lol", tmp.ToString());
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
