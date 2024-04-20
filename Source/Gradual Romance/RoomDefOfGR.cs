using RimWorld;
using Verse;

namespace Gradual_Romance;

[DefOf]
public static class RoomDefOfGR
{
    public static RoomRoleDef RecRoom;
    public static RoomRoleDef DiningRoom;

    static RoomDefOfGR()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RoomDefOfGR));
    }
}