using HarmonyLib;
using Psychology;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), "LovinMtbSinglePawnFactor")]
public static class GRLovinMtbSinglePawnFactor
{
    [HarmonyPrefix]
    public static bool NewGRFormula(ref float __result, Pawn pawn)
    {
        var num = 1f;
        num /= 1f - pawn.health.hediffSet.PainTotal;
        var level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
        if (level < 0.5f)
        {
            num /= level * 2f;
        }

        var sexDriveFactor = PsycheHelper.PsychologyEnabled(pawn)
            ? PsycheHelper.Comp(pawn).Sexuality.AdjustedSexDrive
            : GenMath.FlatHill(0f, 0.75f, 1f, 2f, 3f, 0.2f, AgeCalculationUtility.GetMaturity(pawn));

        __result = num / sexDriveFactor;

        return false;
    }
}