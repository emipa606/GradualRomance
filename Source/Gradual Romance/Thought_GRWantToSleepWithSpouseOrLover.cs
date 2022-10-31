using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class Thought_GRWantToSleepWithSpouseOrLover : Thought_Situational
{
    public override string LabelCap
    {
        get
        {
            var directPawnRelation = RelationshipUtility.MostLikedBedSharingRelationship(pawn, false);
            return string.Format(CurStage.label, directPawnRelation.otherPawn.LabelShort).CapitalizeFirst();
        }
    }

    protected override float BaseMoodOffset
    {
        get
        {
            var a = -0.05f *
                    pawn.relations.OpinionOf(RelationshipUtility.MostLikedBedSharingRelationship(pawn, false)
                        .otherPawn);
            return Mathf.Min(a, -1f);
        }
    }
}