using System.Linq;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_ColdFeetSharingBed : ThoughtWorker
{
    protected override ThoughtState CurrentStateInternal(Pawn pawn)
    {
        if (pawn.ownership.OwnedBed == null)
        {
            return ThoughtState.Inactive;
        }

        var bedPartners = from partner in pawn.ownership.OwnedBed.OwnersForReading
            where partner != pawn && RelationshipUtility.MostAdvancedRelationshipBetween(pawn, partner) != null &&
                  RelationshipUtility.ShouldShareBed(pawn, partner) == false
            select partner;
        return !bedPartners.Any() ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(0);
    }
}