using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryLovinChanceFactor))]
public static class Pawn_RelationsTracker_SecondaryLovinChanceFactor
{
    [HarmonyPriority(Priority.VeryHigh)]
    public static void Postfix(ref float __result, ref Pawn ___pawn, Pawn otherPawn)
    {
        __result = AttractionUtility.CalculateAttraction(___pawn, otherPawn, true, false);
    }
}