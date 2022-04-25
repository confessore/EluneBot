using System;

namespace elunebot.services.interfaces
{
    interface IMainThreadService
    {
        T Invoke<T>(Func<T> @delegate);

        void Invoke(Action @delegate);
    }
}
