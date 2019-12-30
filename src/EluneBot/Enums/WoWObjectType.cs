namespace EluneBot.Enums
{
    public enum WoWObjectType : byte
    {
        /// <summary>
        /// the object has no type
        /// </summary>
        OT_NONE = 0,
        /// <summary>
        /// the object is an item
        /// </summary>
        OT_ITEM = 1,
        /// <summary>
        /// the object is a container
        /// </summary>
        OT_CONTAINER = 2,
        /// <summary>
        /// the object is a unit
        /// </summary>
        OT_UNIT = 3,
        /// <summary>
        /// the object is a player
        /// </summary>
        OT_PLAYER = 4,
        /// <summary>
        /// the object is a game object
        /// </summary>
        OT_GAMEOBJ = 5,
        /// <summary>
        /// the object is a dynamic object
        /// </summary>
        OT_DYNOBJ = 6,
        /// <summary>
        /// the object is a corpse
        /// </summary>
        OT_CORPSE = 7
    }
}
