using System;

namespace EluneBot.Services.Interfaces
{
    internal interface IMainThreadService
    {
        T Invoke<T>(Func<T> @delegate);

        void Invoke(Action @delegate);
    }
}
