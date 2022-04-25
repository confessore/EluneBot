using elunebot.extensions;
using elunebot.models;
using elunebot.services.interfaces;
using elunebot.statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace elunebot.services
{
    sealed class HackService : IHackService
    {
        readonly IMemoryService _memory;
        readonly IObjectManagerService _objectManager;

        public HackService(
            IMemoryService memory,
            IObjectManagerService objectManager)
        {
            _memory = memory;
            _objectManager = objectManager;
        }

        /// <summary>
        /// delegate to our c# function we will jmp to
        /// </summary>
        WardenMemCpyDelegate WardenMemCpy;

        /// <summary>
        /// delegate to our c# function we will jmp to
        /// </summary>
        WardenPageScanDelegate WardenPageScan;

        /// <summary>
        /// first 5 bytes of warden's memscan function
        /// </summary>
        readonly byte[] MemScanOriginalBytes = { 0x56, 0x57, 0xFC, 0x8B, 0x54 };

        readonly byte[] PageScanOriginalBytes = { 0x8B, 0x45, 0x08, 0x8A, 0x04 };

        /// <summary>
        /// is warden's memscan modified?
        /// </summary>
        IntPtr WardensMemScanFuncPtr = IntPtr.Zero;

        IntPtr WardensPageScanFuncPtr = IntPtr.Zero;

        IntPtr WardenMemCpyDetourPtr = IntPtr.Zero;
        IntPtr WardenPageScanDetourPtr = IntPtr.Zero;

        IntPtr AddrToWardenMemCpy = IntPtr.Zero;
        IntPtr AddrToWardenPageScan = IntPtr.Zero;

        /// <summary>
        /// delegate to our C# function
        /// </summary>
        ModifyWardenDetour? ModifyWarden;

        public void HookWardenMemScan()
        {
            ModifyWarden = DisableWarden;
            // get PTR for our c# function
            var addrToDetour = Marshal.GetFunctionPointerForDelegate(ModifyWarden);
            string[] asmCode =
            {
                SendOvers.WardenLoadDetour[0],
                SendOvers.WardenLoadDetour[1],
                SendOvers.WardenLoadDetour[2],
                SendOvers.WardenLoadDetour[3],
                SendOvers.WardenLoadDetour[4].Replace("[|addr|]", ((uint)addrToDetour).ToString()),
                SendOvers.WardenLoadDetour[5],
                SendOvers.WardenLoadDetour[6],
                SendOvers.WardenLoadDetour[7],
            };
            var wardenDetour = InjectAsm(asmCode, "WardenLoadDetour");
            InjectAsm(0x006CA22E, "jmp " + wardenDetour, "WardenLoadDetourJmp");
            System.Diagnostics.Debug.WriteLine("HookWardenMemScan created");
        }

        /// <summary>
        /// init the hack
        /// </summary>
        [Obfuscation(Feature = "virtualization", Exclude = false)]
        private void DisableWarden(IntPtr parWardenPtr1)
        {
            //var second = Memory.Reader.Read<IntPtr>(parWardenPtr1);
            var wardenModuleStart = parWardenPtr1.ReadAs<IntPtr>();
            var memScanPtr = IntPtr.Add(wardenModuleStart, (int)Offsets.Warden.WardenMemScanStart);
            var pageScanPtr = IntPtr.Add(wardenModuleStart, (int)Offsets.Warden.WardenPageScan);

            Console.WriteLine(pageScanPtr.ToString("X"));

            if (pageScanPtr != WardensPageScanFuncPtr)
            {
                var CurrentBytes = App.Reader.ReadBytes(pageScanPtr, 5);
                //var CurrrentBytes = (tmpPtr).ReadAs<Byte>(); //How do I read 5 bytes?
                var isEqual = CurrentBytes.SequenceEqual(PageScanOriginalBytes);
                if (!isEqual) return;

                if (AddrToWardenPageScan == IntPtr.Zero)
                {
                    WardenPageScan = WardenPageScanHook;
                    AddrToWardenPageScan = Marshal.GetFunctionPointerForDelegate(WardenPageScan);
                    if (WardenPageScanDetourPtr == IntPtr.Zero)
                    {
                        // IntPtr readBase, int readOffset, IntPtr writeTo
                        string[] asmCode =
                        {
                            SendOvers.WardenPageScanDetour[0],
                            SendOvers.WardenPageScanDetour[1],
                            SendOvers.WardenPageScanDetour[2],
                            SendOvers.WardenPageScanDetour[3],
                            SendOvers.WardenPageScanDetour[4],
                            SendOvers.WardenPageScanDetour[5],
                            SendOvers.WardenPageScanDetour[6],
                            SendOvers.WardenPageScanDetour[7],
                            SendOvers.WardenPageScanDetour[8],
                            SendOvers.WardenPageScanDetour[9].Replace("[|addr|]", ((uint)AddrToWardenPageScan).ToString()),
                            SendOvers.WardenPageScanDetour[10],
                            SendOvers.WardenPageScanDetour[11],
                            SendOvers.WardenPageScanDetour[12],
                            SendOvers.WardenPageScanDetour[13].Replace("[|addr|]", ((uint)wardenModuleStart + 0x2B2C).ToString())
                        };
                        WardenPageScanDetourPtr = InjectAsm(asmCode, "WardenPageScanDetour");
                    }
                }

                InjectAsm((uint)pageScanPtr,
                    "jmp 0x" + WardenPageScanDetourPtr.ToString("X"),
                    "WardenPageScanJmp");
                WardensPageScanFuncPtr = pageScanPtr;
            }

            if (memScanPtr != WardensMemScanFuncPtr)
            {
                var CurrentBytes = App.Reader.ReadBytes(memScanPtr, 5);
                //var CurrrentBytes = (tmpPtr).ReadAs<Byte>(); //How do I read 5 bytes?
                var isEqual = CurrentBytes.SequenceEqual(MemScanOriginalBytes);
                if (!isEqual) return;

                if (AddrToWardenMemCpy == IntPtr.Zero)
                {
                    WardenMemCpy = WardenMemCpyHook;
                    AddrToWardenMemCpy = Marshal.GetFunctionPointerForDelegate(WardenMemCpy);

                    if (WardenMemCpyDetourPtr == IntPtr.Zero)
                    {
                        string[] asmCodeOnline =
                        {
                            SendOvers.WardenMemCpyDetour[0],
                            SendOvers.WardenMemCpyDetour[1],
                            SendOvers.WardenMemCpyDetour[2],
                            SendOvers.WardenMemCpyDetour[3],
                            SendOvers.WardenMemCpyDetour[4],
                            SendOvers.WardenMemCpyDetour[5],
                            SendOvers.WardenMemCpyDetour[6],
                            SendOvers.WardenMemCpyDetour[7],
                            SendOvers.WardenMemCpyDetour[8],
                            SendOvers.WardenMemCpyDetour[9],
                            SendOvers.WardenMemCpyDetour[10],
                            SendOvers.WardenMemCpyDetour[11],
                            SendOvers.WardenMemCpyDetour[12],
                            SendOvers.WardenMemCpyDetour[13].Replace("[|addr|]", "0x" + ((uint) AddrToWardenMemCpy).ToString("X")),
                            SendOvers.WardenMemCpyDetour[14],
                            SendOvers.WardenMemCpyDetour[15],
                            SendOvers.WardenMemCpyDetour[16],
                            SendOvers.WardenMemCpyDetour[17],
                            SendOvers.WardenMemCpyDetour[18].Replace("[|addr|]", "0x" + ((uint) (memScanPtr + 0x24)).ToString("X"))
                        };
                        WardenMemCpyDetourPtr = InjectAsm(asmCodeOnline, "WardenMemCpyDetour");
                    }
                }

                InjectAsm((uint)memScanPtr, "jmp 0x" + WardenMemCpyDetourPtr.ToString("X"), "WardenMemCpyJmp");
                WardensMemScanFuncPtr = memScanPtr;
            }
        }

        private void WardenPageScanHook(IntPtr readBase, int readOffset, IntPtr writeTo)
        {
            var readByteFrom = readBase + readOffset;

            var activeHacks = Hacks.Where(x => x.IsActivated && x.IsWithinScan(readByteFrom, 1)).ToList();
            activeHacks.ForEach(x =>
            {
                x.Remove();
                Console.WriteLine($@"[PageScan] Disabling {x.Name} at {x.Address.ToString("X")}");
            });
            var myByte = App.Reader.Read<byte>(readByteFrom);
            App.Reader.Write(writeTo, myByte);

            activeHacks.ForEach(x => x.Apply());
        }

        /// <summary>
        /// will be called from our ASM stub
        /// will check if the scanned addr range contains any registered hack
        /// if yes: restore original byte for the hack
        /// do the scan
        /// restore back to the "hacked" state
        /// </summary>
        private void WardenMemCpyHook(IntPtr addr, int size, IntPtr bufferStart)
        {
            if (size != 0)
            {
                // LINQ to get all affected hacks
                var match = Hacks
                    .Where(i => i.Address.ToInt32() <= IntPtr.Add(addr, size).ToInt32()
                                && i.Address.ToInt32() >= addr.ToInt32())
                    .ToList();

                var ActiveHacks = new List<Hack>();
                foreach (var x in match)
                {
                    if (!x.IsActivated) continue;
                    ActiveHacks.Add(x);
                    x.Remove();
                }
                // Do the memscan
                App.Reader.WriteBytes(bufferStart, App.Reader.ReadBytes(addr, size));
                // reapply
                ActiveHacks.ForEach(i => i.Apply());
            }
        }

        /// <summary>
        /// a list to keep track of all hacks registered
        /// </summary>
        public List<Hack> Hacks { get; } = new List<Hack>();

        /// <summary>
        ///     add a hack to the list from the outside
        ///     hack contains: original bytes, bytes we inject, the address we inject to
        /// </summary>
        public void AddHack(Hack parHack)
        {
            if (Hacks.All(i => i.Address != parHack.Address))
            {
                RemoveHack(parHack.Name);
                Hacks.Add(parHack);
            }
        }

        public void RemoveHack(string parName)
        {
            var hack = Hacks.Where(i => i.Name == parName).ToList();
            foreach (var x in hack)
                x.Remove();
            Hacks.RemoveAll(i => i.Name == parName);
        }

        public void RemoveHack(IntPtr parAddress)
        {
            var hack = Hacks.Where(i => i.Address == parAddress).ToList();
            foreach (var x in hack)
                x.Remove();
            Hacks.RemoveAll(i => i.Address == parAddress);
        }

        public Hack GetHack(string parName)
        {
            return Hacks.FirstOrDefault(i => i.Name == parName);
        }

        public Hack GetHack(IntPtr parAddress)
        {
            return Hacks.FirstOrDefault(i => i.Address == parAddress);
        }

        public void ApplyHack(Hack hack)
        {
            if (hack.RelativeToPlayerBase)
            {
                if (!_memory.IsInGame()) return;
                if (_objectManager.LocalPlayer == null) return;
                if (hack.OriginalBytes == null)
                    hack.OriginalBytes = App.Reader.ReadBytes(hack.Address, hack.CustomBytes.Length);
            }
            App.Reader.WriteBytes(hack.Address, hack.CustomBytes);
        }

        public void RemoveHack(Hack hack)
        {
            if (hack.RelativeToPlayerBase)
            {
                if (!_memory.IsInGame()) return;
                if (_objectManager.LocalPlayer == null) return;
                if (hack.OriginalBytes == null)
                    hack.OriginalBytes = App.Reader.ReadBytes(hack.Address, hack.CustomBytes.Length);
                App.Reader.WriteBytes(hack.Address, hack.OriginalBytes);
            }
        }

        public IntPtr InjectAsm(string[] parInstructions, string parPatchName)
        {
            App.Asm.Clear();
            App.Asm.AddLine("use32");
            foreach (var x in parInstructions)
                App.Asm.AddLine(x);

            var byteCode = new byte[0];
            try
            {
                byteCode = App.Asm.Assemble();
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"asm gone wrong in memory service");
            }

            var start = App.Reader.Alloc(byteCode.Length);
            App.Asm.Clear();
            App.Asm.AddLine("use32");
            foreach (var x in parInstructions)
                App.Asm.AddLine(x);
            byteCode = App.Asm.Assemble(start);

            RemoveHack(start);
            RemoveHack(parPatchName);
            var originalBytes = App.Reader.ReadBytes(start, byteCode.Length);
            if (parPatchName != "")
            {
                var parHack = new Hack(
                    _memory,
                    _objectManager,
                    start,
                    byteCode,
                    originalBytes, parPatchName);
                AddHack(parHack);
                parHack.Apply();
            }
            else
                App.Reader.WriteBytes(start, byteCode);
            return start;
        }

        public void InjectAsm(uint parPtr, string parInstructions, string parPatchName)
        {
            App.Asm.Clear();
            App.Asm.AddLine("use32");
            App.Asm.AddLine(parInstructions);
            var start = new IntPtr(parPtr);

            byte[] byteCode;
            try
            {
                byteCode = App.Asm.Assemble(start);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"asm gone wrong in memory service");
                return;
            }

            RemoveHack(start);
            RemoveHack(parPatchName);
            var originalBytes = App.Reader.ReadBytes(start, byteCode.Length);
            if (parPatchName != "")
            {
                var parHack = new Hack(
                    _memory,
                    _objectManager,
                    start,
                    byteCode,
                    originalBytes, parPatchName);
                AddHack(parHack);
                parHack.Apply();
            }
            else
                App.Reader.WriteBytes(start, byteCode);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void ModifyWardenDetour(IntPtr parWardenPtr);

        /// <summary>
        ///     Delegate for our c# function
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void WardenMemCpyDelegate(IntPtr addr, int size, IntPtr bufferStart);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate void WardenPageScanDelegate(IntPtr readBase, int readOffset, IntPtr writeTo);
    }
}
