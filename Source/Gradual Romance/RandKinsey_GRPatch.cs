using HarmonyLib;
using Psychology;
using UnityEngine;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(Pawn_SexualityTracker))]
[HarmonyPatch("RandKinsey")]
public class RandKinsey_GRPatch
{
    [HarmonyPrefix]
    public static bool GRRandKinsey(ref int __result, ref Pawn_SexualityTracker __instance, ref Pawn ___pawn)
    {
        int averageKinsey;
        var pawnGender = ___pawn.gender;
        var extension = ___pawn.def.GetModExtension<XenoRomanceExtension>();
        if (extension.averageKinseyFemale >= 0 && pawnGender == Gender.Female)
        {
            averageKinsey = extension.averageKinseyFemale;
        }
        else if (extension.averageKinseyMale >= 0 && pawnGender == Gender.Male)
        {
            averageKinsey = extension.averageKinseyMale;
        }
        else if (pawnGender == Gender.Female)
        {
            switch (GradualRomanceMod.averageKinseyFemale)
            {
                case GradualRomanceMod.KinseyDescriptor.ExclusivelyHeterosexual:
                    averageKinsey = 0;
                    break;
                case GradualRomanceMod.KinseyDescriptor.MostlyHeterosexual:
                    averageKinsey = 1;
                    break;
                case GradualRomanceMod.KinseyDescriptor.LeansHeterosexual:
                    averageKinsey = 2;
                    break;
                case GradualRomanceMod.KinseyDescriptor.Bisexual:
                    averageKinsey = 3;
                    break;
                case GradualRomanceMod.KinseyDescriptor.LeansHomosexual:
                    averageKinsey = 4;
                    break;
                case GradualRomanceMod.KinseyDescriptor.MostlyHomosexual:
                    averageKinsey = 5;
                    break;
                case GradualRomanceMod.KinseyDescriptor.ExclusivelyHomosexual:
                    averageKinsey = 6;
                    break;
                default:
                    averageKinsey = 0;
                    break;
            }
        }
        else if (pawnGender == Gender.Male)
        {
            switch (GradualRomanceMod.averageKinseyMale)
            {
                case GradualRomanceMod.KinseyDescriptor.ExclusivelyHeterosexual:
                    averageKinsey = 0;
                    break;
                case GradualRomanceMod.KinseyDescriptor.MostlyHeterosexual:
                    averageKinsey = 1;
                    break;
                case GradualRomanceMod.KinseyDescriptor.LeansHeterosexual:
                    averageKinsey = 2;
                    break;
                case GradualRomanceMod.KinseyDescriptor.Bisexual:
                    averageKinsey = 3;
                    break;
                case GradualRomanceMod.KinseyDescriptor.LeansHomosexual:
                    averageKinsey = 4;
                    break;
                case GradualRomanceMod.KinseyDescriptor.MostlyHomosexual:
                    averageKinsey = 5;
                    break;
                case GradualRomanceMod.KinseyDescriptor.ExclusivelyHomosexual:
                    averageKinsey = 6;
                    break;
                default:
                    averageKinsey = 0;
                    break;
            }
        }
        else
        {
            averageKinsey = 0;
        }

        float fAverageKinsey = averageKinsey;
        __result = Mathf.Clamp(
            (int)Rand.GaussianAsymmetric(fAverageKinsey, (fAverageKinsey + 1f) / 2.5f,
                Mathf.Abs(fAverageKinsey - 7f) / 2.5f), 0, 6);

        return false;
    }
}