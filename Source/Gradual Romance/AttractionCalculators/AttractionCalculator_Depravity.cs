using Psychology;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Depravity : AttractionCalculator
{
    public override bool Check(Pawn observer, Pawn assessed)
    {
        return !observer.story.traits.HasTrait(TraitDefOf.Psychopath);
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var depravityFactor = 1f;
        if (assessed.story.traits.HasTrait(TraitDefOf.Cannibal))
        {
            depravityFactor *= 0.8f;
        }

        if (assessed.story.traits.HasTrait(TraitDefOf.Bloodlust))
        {
            depravityFactor *= 0.8f;
        }

        if (assessed.story.traits.HasTrait(TraitDefOfPsychology.Lecher))
        {
            depravityFactor *= 0.8f;
        }

        return depravityFactor;
    }
}