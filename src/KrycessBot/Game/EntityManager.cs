using Process.NET;

namespace KrycessBot.Game
{
    public sealed class EntityManager
    {
        readonly ProcessSharp processSharp;

        public EntityManager(ProcessSharp processSharp)
        {
            this.processSharp = processSharp;
        }
    }
}
