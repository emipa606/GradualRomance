using Psychology;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_HomophobicSelfLoathing : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn) && PsychologySettings.enableKinsey)
        {
            return PsycheHelper.Comp(pawn).Sexuality.kinseyRating >= 2 ? ThoughtState.ActiveAtStage(0) : false;
        }

        return pawn.story.traits.HasTrait(TraitDefOf.Gay) ? ThoughtState.ActiveAtStage(0) : false;
    }
}