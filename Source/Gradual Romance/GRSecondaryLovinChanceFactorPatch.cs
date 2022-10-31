using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryLovinChanceFactor))]
public static class GRSecondaryLovinChanceFactorPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(Priority.VeryHigh)]
    public static void GRSecondaryLovinChanceFactor(Pawn_RelationsTracker __instance, ref float __result,
        ref Pawn ___pawn, Pawn otherPawn)
    {
        __result = AttractionUtility.CalculateAttraction(___pawn, otherPawn, true, false);
    }
}