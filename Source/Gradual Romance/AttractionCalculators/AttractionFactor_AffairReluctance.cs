using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class AttractionFactor_AffairReluctance : AttractionCalculator
{
    private Pawn assessedCuckold;
    private Pawn observerCuckold;

    public override bool Check(Pawn observer, Pawn assessed)
    {
        if (!RelationshipUtility.IsAnAffair(observer, assessed, out var cuck1, out var cuck2))
        {
            return false;
        }

        observerCuckold = cuck1;
        assessedCuckold = cuck2;
        return true;
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var affairReluctance = 1f;
        var affairReluctance2 = 1f;
        if (observerCuckold != null)
        {
            affairReluctance =
                RelationshipUtility.AffairReluctance(
                    RelationshipUtility.MostAdvancedRelationshipBetween(observer, observerCuckold));
            affairReluctance *=
                Mathf.Pow(Mathf.InverseLerp(-100f, 5f, observer.relations.OpinionOf(observerCuckold)), -0.33f);
        }

        if (assessedCuckold == null)
        {
            return Mathf.Min(affairReluctance, affairReluctance2);
        }

        affairReluctance2 =
            RelationshipUtility.AffairReluctance(
                RelationshipUtility.MostAdvancedRelationshipBetween(assessed, assessedCuckold));
        affairReluctance2 *=
            Mathf.Pow(Mathf.InverseLerp(-100f, 5f, observer.relations.OpinionOf(assessedCuckold)), -0.33f);

        return Mathf.Min(affairReluctance, affairReluctance2);
    }
}