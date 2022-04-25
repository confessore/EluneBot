using elunebot.models;
using System;
using System.Collections.Generic;

namespace elunebot.services.interfaces
{
    interface IHackService
    {
        void HookWardenMemScan();

        /// <summary>
        /// a list to keep track of all hacks registered
        /// </summary>
        List<Hack> Hacks { get; }

        /// <summary>
        /// add a hack to the list from the outside
        /// hack contains: original bytes, bytes we inject, the address we inject to
        /// </summary>
        public void AddHack(Hack parHack);

        void RemoveHack(string parName);

        void RemoveHack(IntPtr parAddress);

        Hack GetHack(string parName);

        Hack GetHack(IntPtr parAddress);

        void ApplyHack(Hack hack);

        void RemoveHack(Hack hack);

        IntPtr InjectAsm(string[] parInstructions, string parPatchName);

        void InjectAsm(uint parPtr, string parInstructions, string parPatchName);
    }
}
