using System;
using System.Diagnostics;
using System.Threading;

namespace elunebot
{
    public static class Program
    {
        public delegate void Main();

        static Thread Thread { get; set; }

        [STAThread]
        public static void Entry()
        {
            Thread = new Thread(App.Main);
            Thread.SetApartmentState(ApartmentState.STA);
            Thread.Start();
        }
    }
}