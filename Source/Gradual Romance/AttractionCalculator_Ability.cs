using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Ability : AttractionCalculator
{
    public override bool Check(Pawn observer, Pawn assessed)
    {
        return !observer.story.traits.HasTrait(TraitDefOfPsychology.OpenMinded);
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var disabilityFactor = 1f;
        disabilityFactor *= Mathf.Lerp(0.2f, 1f, assessed.health.capacities.GetLevel(PawnCapacityDefOf.Talking));
        disabilityFactor *=
            Mathf.Lerp(0.2f, 1f, assessed.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
        disabilityFactor *= Mathf.Lerp(0.2f, 1f, assessed.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
        return disabilityFactor;
    }
}