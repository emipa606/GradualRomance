using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_FeelingNeglected : ThoughtWorker
{
    protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
    {
        if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
        {
            return false;
        }

        var pawnRelationDef = RelationshipUtility.MostAdvancedRelationshipBetween(pawn, other);
        if (pawnRelationDef == null)
        {
            return false;
        }

        if (!pawnRelationDef.GetModExtension<RomanticRelationExtension>().isFormalRelationship)
        {
            return false;
        }

        return RelationshipUtility.LevelOfTension(pawn, other) == 0 ? ThoughtState.ActiveDefault : false;
    }
}