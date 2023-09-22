using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Gradual_Romance;

internal static class LovinUtility
{
    private static bool IsCellPrivateFor(IntVec3 cell, Pawn pawn, Pawn other)
    {
        var disturbingPawns =
            pawn.Map.mapPawns.AllPawnsSpawned.Where(x => x != pawn && x != other && !x.NonHumanlikeOrWildMan());
        foreach (var disturber in disturbingPawns)
        {
            if (GenSight.LineOfSight(disturber.Position, cell, disturber.Map))
            {
                return false;
            }
        }

        return true;
    }

    public static IEnumerable<IntVec3> FindPrivateCellsFor(Pawn pawn, Pawn other, int dist)
    {
        var map = pawn.Map;
        return CellQuery.GetRandomCellSampleAround(pawn, dist, dist).Where(x =>
            x.Standable(map) && x.GetDangerFor(pawn, map) == Danger.None && x.InAllowedArea(pawn) &&
            x.InAllowedArea(other) && IsCellPrivateFor(x, pawn, other));
    }


    public static bool RoomIsPrivateFor(Room room, Pawn pawn, Pawn other)
    {
        if (room.Role == RoomRoleDefOf.RecRoom || room.Role == RoomRoleDefOf.DiningRoom)
        {
            return false;
        }

        if (room.Role == RoomRoleDefOf.Bedroom && room.Owners.Any())
        {
            if (!room.Owners.Contains(pawn) && !room.Owners.Contains(other))
            {
                return false;
            }
        }

        var roomPawns = (from thing in room.ContainedAndAdjacentThings
                         where thing is Pawn pawn1 && thing != pawn && thing != other && pawn1.NonHumanlikeOrWildMan()
                         select thing) as IEnumerable<Pawn>;
        return !roomPawns!.Any();
    }
}