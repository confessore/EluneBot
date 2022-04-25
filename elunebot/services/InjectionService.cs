using elunebot.services.interfaces;
using elunebot.statics;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace elunebot.services
{
    sealed class InjectionService : IInjectionService
    {
        public void Inject(int pid)
        {
            var process = Process.GetProcesses().FirstOrDefault(x => x.Id == pid);
            process.WaitForInputIdle();
            var procHandle = Imports.OpenProcess(Imports.PROCESS_CREATE_THREAD | Imports.PROCESS_QUERY_INFORMATION | Imports.PROCESS_VM_OPERATION | Imports.PROCESS_VM_WRITE | Imports.PROCESS_VM_READ, false, process.Id);
            var nethost = Path.GetFullPath(".\\nethost.dll");
            var bootstrap = Path.GetFullPath(".\\elunebot.bootstrap.dll");
            var fasm = Path.GetFullPath(".\\fasmdll_managed.dll");
            if (!File.Exists(bootstrap)) throw new Exception();
            if (!File.Exists(nethost)) throw new Exception();
            RemoteLoadLibrary(procHandle, "kernel32.dll", "LoadLibraryW", nethost);
            RemoteLoadLibrary(procHandle, "kernel32.dll", "LoadLibraryW", bootstrap);
            RemoteLoadLibrary(procHandle, bootstrap, "launch", 1);
        }

        static void RemoteLoadLibrary<T>(IntPtr handle, string PathToDll, string functionToCall, T parameter)
        {
            var llHandle = Imports.LoadLibrary(PathToDll);
            IntPtr functionToCallPtr = Imports.GetProcAddress(Imports.GetModuleHandle(Path.GetFileName(PathToDll)), functionToCall);
            UIntPtr bytesWritten;
            uint allocSize;
            byte[] bytes;
            if (typeof(T) == typeof(string))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var strParam = parameter as string;
                bytes = Encoding.Unicode.GetBytes(strParam);
                allocSize = (uint)((strParam.Length + 1) * 2);
            }
            else
            {
                var result = (byte[])typeof(BitConverter).GetMethod("GetBytes", new[] { typeof(T) })
                    .Invoke(null, new[] { parameter as object });
                bytes = result;
                allocSize = (uint)result.Length;
            }
            IntPtr allocMemAddress = Imports.VirtualAllocEx(handle, IntPtr.Zero, allocSize, Imports.MEM_COMMIT | Imports.MEM_RESERVE, Imports.PAGE_READWRITE);
            Imports.WriteProcessMemory(handle, allocMemAddress, bytes, allocSize, out bytesWritten);
            var thrHandle = Imports.CreateRemoteThread(handle, IntPtr.Zero, 0, functionToCallPtr, allocMemAddress, 0, IntPtr.Zero);
            Imports.WaitForSingleObject(thrHandle, UInt32.MaxValue);
            Imports.VirtualFreeEx(handle, allocMemAddress, allocSize,
                Imports.AllocationType.Release);
            Imports.FreeLibrary(llHandle);
        }
    }
}
