using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(JobDriver_Lovin), "GenerateRandomMinTicksToNextLovin")]
public static class GRGenerateRandomMinTicksToNextLovin
{
    [HarmonyPrefix]
    public static bool GRNextLovinFormula(Pawn pawn, ref int __result)
    {
        if (DebugSettings.alwaysDoLovin)
        {
            __result = 100;
        }
        else
        {
            var num = AgeCalculationUtility.recoveryTimeByMaturity.Evaluate(
                AgeCalculationUtility.GetMaturity(pawn));
            num = Rand.Gaussian(num, 0.3f);
            if (num < 0.5f)
            {
                num = 0.5f;
            }

            __result = (int)(num * 2500f);
        }

        return false;
    }
}