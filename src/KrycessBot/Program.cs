using System;
using System.Threading;

namespace KrycessBot
{
    static class Program
    {
        static Thread Thread { get; set; }

        [STAThread]
        static int Entry(string args)
        {
            Thread = new Thread(App.Main);
            Thread.SetApartmentState(ApartmentState.STA);
            Thread.Start();
            return 1;
        }
    }
}
