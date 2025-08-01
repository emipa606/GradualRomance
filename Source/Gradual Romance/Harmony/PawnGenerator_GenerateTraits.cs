using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(PawnGenerator), "GenerateTraits")]
public static class PawnGenerator_GenerateTraits
{
    [HarmonyPriority(Priority.Low)]
    public static void Postfix(ref Pawn pawn)
    {
        if (pawn.story.traits.HasTrait(TraitDefOfGR.Beauty) || !GradualRomanceMod.rerollBeautyTraits)
        {
            return;
        }

        var result = Mathf.Clamp(Mathf.RoundToInt(Rand.Gaussian(0, 1.5f)), -4, 4);
        if (result != 0)
        {
            pawn.story.traits.GainTrait(new Trait(TraitDefOfGR.Beauty, result, true));
        }
    }
}