using RimWorld;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Voice : AttractionCalculator
{
    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var voiceFactor = 1f;
        if (assessed.story.traits.HasTrait(TraitDefOf.AnnoyingVoice))
        {
            voiceFactor *= 0.8f;
        }

        if (assessed.story.traits.HasTrait(TraitDefOfGR.MelodicVoice))
        {
            voiceFactor *= 1.25f;
        }

        return voiceFactor;
    }
}