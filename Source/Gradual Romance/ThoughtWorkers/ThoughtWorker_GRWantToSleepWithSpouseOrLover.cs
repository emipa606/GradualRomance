using System.Linq;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class ThoughtWorker_GRWantToSleepWithSpouseOrLover : ThoughtWorker
{
    /// This is a mish-mash of copy-pasted code.
    /// Mostly copy-pasted from Psychology Mod by Word-Mule
    /// 
    /// Does the same function but excludes lovefriends and sweethearts
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        var directPawnRelation = RelationshipUtility.MostLikedBedSharingRelationship(p, false);
        if (directPawnRelation == null)
        {
            return ThoughtState.Inactive;
        }

        var multiplePartners = (from r in p.relations.PotentiallyRelatedPawns
            where RelationshipUtility.ShouldShareBed(p, r)
            select r).Count() > 1;
        bool partnerBedInRoom;
        if (p.ownership.OwnedBed != null)
        {
            partnerBedInRoom = (from t in p.ownership.OwnedBed.GetRoom().ContainedBeds
                where t.OwnersForReading.Contains(directPawnRelation.otherPawn)
                select t).Any();
        }
        else
        {
            partnerBedInRoom = false;
        }

        if (p.ownership.OwnedBed != null && RelationshipUtility.IsPolygamist(p) &&
            multiplePartners && partnerBedInRoom)
        {
            return ThoughtState.Inactive;
        }

        if (p.ownership.OwnedBed != null && p.ownership.OwnedBed == directPawnRelation.otherPawn.ownership.OwnedBed)
        {
            return ThoughtState.Inactive;
        }

        return p.relations.OpinionOf(directPawnRelation.otherPawn) <= 0
            ? ThoughtState.Inactive
            : ThoughtState.ActiveDefault;
    }
}