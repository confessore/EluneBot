using EluneBot.Services.Interfaces;
using Process.NET;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EluneBot.Services
{
    public sealed class MainThreadService : IMainThreadService
    {
        public MainThreadService()
        {
            Task.Delay(10000).Wait();
            ProcessSharp = new ProcessSharp(System.Diagnostics.Process.GetCurrentProcess(), Process.NET.Memory.MemoryType.Local);
            mtId = System.Diagnostics.Process.GetCurrentProcess().Threads[0].Id;
            EnumWindows(WindowProcess, IntPtr.Zero);
            newCallback = WndProc; // Pins WndProc - will not be garbage collected.
            oldCallback = SetWindowLong((IntPtr)hWnd, GWL_WNDPROC,
                Marshal.GetFunctionPointerForDelegate(newCallback));
        }

        readonly ProcessSharp ProcessSharp;

        [DllImport("user32.dll")]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        internal static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        delegate int WindowProc(IntPtr hWnd, int Msg, int wParam, int lParam);

        readonly int mtId;
        int hWnd;
        const int GWL_WNDPROC = -4;
        const int WM_USER = 0x0400;
        readonly WindowProc newCallback;
        readonly IntPtr oldCallback;
        private readonly ConcurrentQueue<Action> InvokeQueue = new ConcurrentQueue<Action>();
        private readonly ConcurrentQueue<Delegate> InvokeReturnFunction = new ConcurrentQueue<Delegate>();
        private readonly ConcurrentQueue<object> InvokeReturnValue = new ConcurrentQueue<object>();

        bool WindowProcess(IntPtr hWnd, IntPtr lParam)
        {
            GetWindowThreadProcessId(hWnd, out int procId);
            if (procId != App.ProcessSharp.Native.Id) return true;
            if (!IsWindowVisible(hWnd)) return true;
            var length = GetWindowTextLength(hWnd);
            if (length == 0) return true;
            var builder = new StringBuilder(length + 1);
            GetWindowText(hWnd, builder, builder.Capacity);
            if (builder.ToString() == "World of Warcraft")
                this.hWnd = (int)hWnd;
            return true;
        }

        void SendUserMessage(UserMessage message)
        {
            SendMessage(hWnd, WM_USER, (int)message, 0);
        }

        public T Invoke<T>(Func<T> @delegate)
        {
            if (GetCurrentThreadId() == mtId)
                return @delegate();
            InvokeReturnFunction.Enqueue(@delegate);
            SendUserMessage(UserMessage.RunDelegateReturn);
            object ret;
            if (InvokeReturnValue.TryDequeue(out ret))
            {
                return (T)ret;
            }
            return default;
        }

        public void Invoke(Action @delegate)
        {
            if (GetCurrentThreadId() == mtId)
            {
                @delegate();
                return;
            }
            InvokeQueue.Enqueue(@delegate);
            SendUserMessage(UserMessage.RunDelegate);
        }

        int WndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            if (Msg != WM_USER) return CallWindowProc(oldCallback, hWnd, Msg, wParam, lParam);
            var tmpMsg = (UserMessage)wParam;
            switch (tmpMsg)
            {
                case UserMessage.RunDelegateReturn:
                    Delegate result;
                    if (InvokeReturnFunction.TryDequeue(out result))
                    {
                        var invokeTarget = result;
                        InvokeReturnValue.Enqueue(invokeTarget.DynamicInvoke());
                    }
                    return 0;

                case UserMessage.RunDelegate:
                    Action action;
                    if (InvokeQueue.TryDequeue(out action))
                    {
                        action.Invoke();
                    }
                    return 0;
            }
            return CallWindowProc(oldCallback, hWnd, Msg, wParam, lParam);
        }

        enum UserMessage
        {
            RunDelegateReturn,
            RunDelegate
        }
    }
}
