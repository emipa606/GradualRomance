using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Hygiene : AttractionCalculator
{
    private const float maxFilthPenalty = 0.75f;
    private const float filthMin = 0.7f;
    private const float filthMax = 0.1f;

    public override bool Check(Pawn observer, Pawn assessed)
    {
        return ModHooks.UsingDubsHygiene() &&
               (assessed?.needs?.AllNeeds?.Any(x => x.def.defName == "Hygiene") ?? false);
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        return Mathf.Clamp01(Mathf.Lerp(maxFilthPenalty, 1f,
            Mathf.Clamp01(Mathf.InverseLerp(filthMax, filthMin, ModHooks.GetHygieneNeed(assessed)))));
    }
}