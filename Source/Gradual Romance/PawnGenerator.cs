﻿using HarmonyLib;
using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance.Harmony
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateTraits")]
    public static class GRPawnGeneratorPatch
    {
        [LogPerformance]
        [HarmonyPriority(Priority.Low)]
        [HarmonyPostfix]
        public static void GRPawnGenerator_AddBeautyTrait(ref Pawn pawn, PawnGenerationRequest request)
        {
            if (pawn.story.traits.HasTrait(TraitDefOf.Beauty) || !GradualRomanceMod.rerollBeautyTraits)
            {
                return;
            }

            var result = Mathf.Clamp(Mathf.RoundToInt(Rand.Gaussian(0, 1.5f)), -4, 4);
            if (result != 0)
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Beauty, result, true));
            }
        }
    }
}