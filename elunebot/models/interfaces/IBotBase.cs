using System;

namespace elunebot.models.interfaces
{
    public interface IBotBase
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
