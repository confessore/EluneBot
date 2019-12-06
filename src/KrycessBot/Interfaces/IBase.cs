using System;

namespace KrycessBot.Interfaces
{
    public interface IBase
    {
        string Name { get; }
        Version Version { get; }
        string Author { get; }
        void Dispose();
        void Start();
        void Stop();
        void ToggleGUI();
    }
}
