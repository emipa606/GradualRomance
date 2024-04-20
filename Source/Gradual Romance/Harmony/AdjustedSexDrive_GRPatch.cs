using HarmonyLib;
using Psychology;
using UnityEngine;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(Pawn_SexualityTracker), nameof(Pawn_SexualityTracker.AdjustedSexDrive), MethodType.Getter)]
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