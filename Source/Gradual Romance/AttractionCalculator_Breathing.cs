using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Breathing : AttractionCalculator
    {
        public override float Calculate(Pawn observer, Pawn assessed)
        {
            var breathFactor = 1f;
            if (assessed.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
            {
                breathFactor *= 0.8f;
            }

            return breathFactor;
        }
    }
}