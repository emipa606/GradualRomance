using Psychology;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_Homophobic : ThoughtWorker
{
    protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
    {
        if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
        {
            return false;
        }

        if (PsycheHelper.PsychologyEnabled(other) && PsychologySettings.enableKinsey)
        {
            if (PsycheHelper.Comp(other).Sexuality.kinseyRating >= 2)
            {
                return ThoughtState.ActiveAtStage(0);
            }
        }
        else if (other.story.traits.HasTrait(TraitDefOf.Gay))
        {
            return ThoughtState.ActiveAtStage(0);
        }

        return false;
    }
}