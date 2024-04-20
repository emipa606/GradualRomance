using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_SuperficialSelfLoathing : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        if (RelationsUtility.IsDisfigured(pawn))
        {
            return ThoughtState.ActiveAtStage(0);
        }

        return pawn.story.traits.DegreeOfTrait(TraitDefOfGR.Beauty) < -1 ? ThoughtState.ActiveAtStage(0) : false;
    }
}