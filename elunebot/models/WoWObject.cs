using elunebot.models.enums;
using elunebot.services;
using elunebot.statics;
using System;

namespace elunebot.models
{
    public class WoWObject
    {
        public WoWObject(ulong guid, IntPtr pointer, WoWObjectType type)
        {
            Guid = guid;
            Pointer = pointer;
            Type = type;
        }

        public ulong Guid { get; internal  set; }
        public IntPtr Pointer { get; internal set; }
        public WoWObjectType Type { get; internal set; }
        public bool CanRemove { get; internal set; }
        public virtual string Name { get; internal set; }
        public virtual Location Position { get; internal set; }

        internal T GetDescriptor<T>(int descriptor) where T : struct
        {
            var pointer = App.Reader.Read<uint>(IntPtr.Add(Pointer, Offsets.ObjectManager.DescriptorOffset));
            return App.Reader.Read<T>(new IntPtr(pointer + descriptor));
        }

        internal T ReadRelative<T>(int offset) where T : struct =>
            App.Reader.Read<T>(IntPtr.Add(Pointer, offset));
    }
}
