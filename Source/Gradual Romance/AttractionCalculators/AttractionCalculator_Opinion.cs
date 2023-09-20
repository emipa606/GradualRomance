using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Opinion : AttractionCalculator
{
    private const float UniversalOpinionImportance = 1f;

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        float opinion = AttractionUtility.GetRelationshipUnmodifiedOpinion(observer, assessed);
        var romanceFactor = Mathf.InverseLerp(-100, 100, opinion) * 2f;
        return Mathf.Pow(romanceFactor, UniversalOpinionImportance);
    }
}