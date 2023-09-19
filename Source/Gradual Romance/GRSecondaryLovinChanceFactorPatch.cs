using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryLovinChanceFactor))]
public static class GRSecondaryLovinChanceFactorPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(Priority.VeryHigh)]
    public static void GRSecondaryLovinChanceFactor(ref float __result, ref Pawn ___pawn, Pawn otherPawn)
    {
        if ((___pawn?.RaceProps?.Humanlike ?? false) && (otherPawn?.RaceProps?.Humanlike ?? false))
            __result = AttractionUtility.CalculateAttraction(___pawn, otherPawn, true, false);
    }
}