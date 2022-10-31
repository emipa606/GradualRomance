using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(ThoughtWorker_Ugly), "CurrentSocialStateInternal", typeof(Pawn), typeof(Pawn))]
public static class GRThoughtWorker_UglyPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(Priority.HigherThanNormal)]
    public static void GRUglyPatch(ref ThoughtState __result, Pawn pawn, Pawn other)
    {
        if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
        {
            __result = false;
        }
        else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
        {
            __result = false;
        }

        else
        {
            var num = other.story.traits.DegreeOfTrait(TraitDefOf.Beauty);
            switch (num)
            {
                case -1:
                    __result = ThoughtState.ActiveAtStage(0);
                    break;
                case -2:
                    __result = ThoughtState.ActiveAtStage(1);
                    break;
                case -3:
                    __result = ThoughtState.ActiveAtStage(2);
                    break;
                case -4:
                    __result = ThoughtState.ActiveAtStage(3);
                    break;
                default:
                    __result = false;
                    break;
            }
        }
    }
}