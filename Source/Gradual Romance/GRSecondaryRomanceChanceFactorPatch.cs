using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony
{
    [HarmonyPatch(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryRomanceChanceFactor))]
    public static class GRSecondaryRomanceChanceFactorPatch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.VeryHigh)]
        public static void GRSecondaryRomanceChanceFactor(Pawn_RelationsTracker __instance, ref float __result,
            ref Pawn ___pawn, Pawn otherPawn)
        {
            __result = AttractionUtility.CalculateAttraction(___pawn, otherPawn, false, true);
        }
    }
}