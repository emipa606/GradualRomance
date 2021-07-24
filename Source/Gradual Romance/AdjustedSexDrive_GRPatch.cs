using HarmonyLib;
using Psychology;
using UnityEngine;
using Verse;

namespace Gradual_Romance.Harmony
{
    [HarmonyPatch(typeof(Pawn_SexualityTracker))]
    [HarmonyPatch("AdjustedSexDrive", MethodType.Getter)]
    public class AdjustedSexDrive_GRPatch
    {
        [HarmonyPostfix]
        public static void GRXenoSexDrive(ref float __result, ref Pawn_SexualityTracker __instance, ref Pawn ___pawn)
        {
            var curve = GradualRomanceMod.GetSexDriveCurveFor(___pawn);
            if (curve == null)
            {
                return;
            }

            __result = curve.Evaluate(___pawn.ageTracker.AgeBiologicalYearsFloat) *
                       Mathf.InverseLerp(0f, 0.5f, __instance.sexDrive);
            __result = ___pawn.def.GetModExtension<XenoRomanceExtension>().canGoIntoHeat == false
                ? Mathf.Clamp01(__result)
                : Mathf.Min(__result, 0f);
        }
    }
}


/*
 *         
 *         
 *                     if(PsychologyBase.KinseyFormula() == PsychologyBase.KinseyMode.Realistic)
            {
                return Mathf.Clamp((int)Rand.GaussianAsymmetric(0f, 1f, 3.13f), 0, 6);
            }
            else if (PsychologyBase.KinseyFormula() == PsychologyBase.KinseyMode.Invisible)
            {
                return Mathf.Clamp((int)Rand.GaussianAsymmetric(3.5f, 1.7f, 1.7f), 0, 6);
            }
            else if (PsychologyBase.KinseyFormula() == PsychologyBase.KinseyMode.Gaypocalypse)
            {
                return Mathf.Clamp((int)Rand.GaussianAsymmetric(7f, 3.13f, 1f), 0, 6);
            }
 *         
 *         
 *         public float AdjustedSexDrive
        {
            get
            {
                float ageFactor = 1f;
                if (pawn.gender == Gender.Male) {
                    ageFactor = MaleSexDriveCurve.Evaluate(pawn.ageTracker.AgeBiologicalYears);
                }
                else if (pawn.gender == Gender.Female)
                {
                    ageFactor = FemaleSexDriveCurve.Evaluate(pawn.ageTracker.AgeBiologicalYears);
                }
                return Mathf.Clamp01(ageFactor * Mathf.InverseLerp(0f, 0.5f, this.sexDrive));
            }
        }
 * 
 * 
 */