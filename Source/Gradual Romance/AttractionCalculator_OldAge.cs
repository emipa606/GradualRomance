using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_OldAge : AttractionCalculator
    {
        public override bool Check(Pawn observer, Pawn assessed)
        {
            return AgeCalculationUtility.GetMaturity(assessed) > AgeCalculationUtility.GetMaturity(observer);
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            return Mathf.Clamp01(AgeCalculationUtility.GetMaturityFactor(observer, assessed));
        }
    }
}