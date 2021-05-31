using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class ThoughtWorker_SuperficialSelfLoathing : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            if (RelationsUtility.IsDisfigured(pawn))
            {
                return ThoughtState.ActiveAtStage(0);
            }

            if (pawn.story.traits.DegreeOfTrait(TraitDefOf.Beauty) < -1)
            {
                return ThoughtState.ActiveAtStage(0);
            }

            return false;
        }
    }
}