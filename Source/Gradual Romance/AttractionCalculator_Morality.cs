using Psychology;
using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Morality : AttractionCalculator
    {
        public override float Calculate(Pawn observer, Pawn assessed)
        {
            var moralityFactor = 1f;
            if (assessed.story.traits.HasTrait(TraitDefOf.Kind))
            {
                moralityFactor *= 1.2f;
            }

            if (assessed.story.traits.HasTrait(TraitDefOfPsychology.BleedingHeart))
            {
                moralityFactor *= 1.2f;
            }

            return moralityFactor;
        }
    }
}