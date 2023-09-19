using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryRomanceChanceFactor))]
public static class GRSecondaryRomanceChanceFactorPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(Priority.VeryHigh)]
    public static void GRSecondaryRomanceChanceFactor(ref float __result, ref Pawn ___pawn, Pawn otherPawn)
    {
        if ((___pawn?.RaceProps?.Humanlike ?? false) && (otherPawn?.RaceProps?.Humanlike ?? false))
            __result = AttractionUtility.CalculateAttraction(___pawn, otherPawn, false, true);
    }
}