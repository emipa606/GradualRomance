using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_PrettySuperficial : ThoughtWorker
{
    protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
    {
        if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
        {
            return false;
        }

        if (RelationsUtility.IsDisfigured(other))
        {
            return false;
        }

        if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
        {
            return false;
        }

        var num = other.story.traits.DegreeOfTrait(TraitDefOf.Beauty);
        switch (num)
        {
            case 1:
                return ThoughtState.ActiveAtStage(0);
            case 2:
                return ThoughtState.ActiveAtStage(1);
            case 3:
                return ThoughtState.ActiveAtStage(2);
            case 4:
                return ThoughtState.ActiveAtStage(3);
            default:
                return false;
        }
    }
}