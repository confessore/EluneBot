using System;

namespace EluneBot.Services.Interfaces
{
    public interface IMainThreadService
    {
        T Invoke<T>(Func<T> @delegate);

        void Invoke(Action @delegate);
    }
}
