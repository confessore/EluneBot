using elunebot.services.interfaces;
using System;
using System.Linq;

namespace elunebot.models
{
    /// <summary>
    /// class for a simple hack (read: changing bytes in memory)
    /// </summary>
    sealed class Hack
    {
        readonly IMemoryService _memory;
        readonly IObjectManagerService _objectManager;

        /// <summary>
        /// constructor: addr and the new bytes
        /// </summary>
        public Hack(
            IMemoryService memory,
            IObjectManagerService objectManager,
            IntPtr parAddress,
            byte[] parCustomBytes,
            string parName)
        {
            _memory = memory;
            _objectManager = objectManager;
            Address = parAddress;
            CustomBytes = parCustomBytes;
            OriginalBytes = App.Reader.ReadBytes(Address, customBytes.Length);
            Name = parName;
        }

        /// <summary>
        /// constructor: addr, new bytes aswell old bytes
        /// </summary>
        public Hack(IMemoryService memory,
            IObjectManagerService objectManager,
            IntPtr parAddress,
            byte[] parCustomBytes,
            byte[] parOriginalBytes,
            string parName)
        {
            _memory = memory;
            _objectManager = objectManager;
            Address = parAddress;
            CustomBytes = parCustomBytes;
            OriginalBytes = parOriginalBytes;
            Name = parName;
        }

        public Hack(IMemoryService memory,
            IObjectManagerService objectManager,
            uint offset,
            byte[] parCustomBytes,
            string parName)
        {
            _memory = memory;
            _objectManager = objectManager;
            Address = (IntPtr)offset;
            CustomBytes = parCustomBytes;
            Name = parName;
        }

        // old bytes
        byte[] originalBytes;
        public byte[] OriginalBytes
        {
            get => originalBytes;
            set => originalBytes = value;
        }

        public bool DynamicHide { get; set; } = false;
        // is the hack applied
        //internal bool IsApplied { get; private set; }

        public bool RelativeToPlayerBase { get; set; } = false;

        // address where the bytes will be changed
        IntPtr address = IntPtr.Zero;
        public IntPtr Address
        {
            get
            {
                return !RelativeToPlayerBase
                    ? address
                    : IntPtr.Add(_objectManager.LocalPlayer.Pointer, (int)address);
            }
            private set { address = value; }
        }

        // new bytes
        byte[] customBytes { get; set; }
        public byte[] CustomBytes
        {
            get => customBytes;
            set => customBytes = value;
        }

        // name of hack
        internal string Name { get; private set; }

        internal bool IsActivated
        {
            get
            {
                if (RelativeToPlayerBase)
                {
                    if (!_memory.IsInGame()) return false;
                    if (_objectManager.LocalPlayer == null) return false;
                }
                var curBytes = App.Reader.ReadBytes(Address, originalBytes.Length);
                return !curBytes.SequenceEqual(originalBytes);
            }
        }

        internal bool IsWithinScan(IntPtr scanStartAddress, int size)
        {
            var scanStart = (int)scanStartAddress;
            var scanEnd = (int)IntPtr.Add(scanStartAddress, size);

            var hackStart = (int)Address;
            var hackEnd = (int)Address + customBytes.Length;

            if (hackStart >= scanStart && hackStart < scanEnd)
                return true;

            if (hackEnd > scanStart && hackEnd <= scanEnd)
                return true;

            return false;
        }

        /// <summary>
        /// apply the new bytes to address
        /// </summary>
        internal void Apply()
        {
            if (RelativeToPlayerBase)
            {
                if (!_memory.IsInGame()) return;
                if (_objectManager.LocalPlayer == null) return;
                if (OriginalBytes == null)
                    OriginalBytes = App.Reader.ReadBytes(Address, CustomBytes.Length);
            }
            App.Reader.WriteBytes(Address, CustomBytes);
        }

        /// <summary>
        /// restore the old bytes to the address
        /// </summary>
        internal void Remove()
        {
            if (RelativeToPlayerBase)
            {
                if (!_memory.IsInGame()) return;
                if (_objectManager.LocalPlayer == null) return;
            }
            if (DynamicHide && IsActivated)
                CustomBytes = App.Reader.ReadBytes(Address, OriginalBytes.Length);
            App.Reader.WriteBytes(Address, OriginalBytes);
        }
    }
}
