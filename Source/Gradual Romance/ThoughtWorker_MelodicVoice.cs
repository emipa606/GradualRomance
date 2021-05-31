using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class ThoughtWorker_MelodicVoice : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
        {
            if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
            {
                return false;
            }

            if (!other.story.traits.HasTrait(TraitDefOfGR.MelodicVoice))
            {
                return false;
            }

            if (pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing) <= 0.15f)
            {
                return false;
            }

            return ThoughtState.ActiveAtStage(0);
        }
    }
}