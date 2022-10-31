using UnityEngine;
using Verse;

namespace Gradual_Romance;

public static class AgeCalculationUtility
{
    public const float pctAgeDeviation = 0.05f;
    public const float youthPenalty = 2f;
    public const float oldAgePenalty = 1.5f;

    public static readonly SimpleCurve recoveryTimeByMaturity = new SimpleCurve
    {
        new CurvePoint(0.8f, 1.5f),
        new CurvePoint(1f, 2f),
        new CurvePoint(1.5f, 4f),
        new CurvePoint(2f, 12f),
        new CurvePoint(3f, 36f)
    };

    public static float MaturityDeviation(float maturity)
    {
        return maturity * pctAgeDeviation;
    }

    public static float GetMaturity(Pawn pawn)
    {
        return GradualRomanceMod.GetMaturityCurveFor(pawn).Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
    }

    public static float GetMaturityFactor(Pawn observer, Pawn assessor)
    {
        var observerMaturity = GetMaturity(observer);
        var assessorMaturity = GetMaturity(assessor);
        var maturityDeviation = MaturityDeviation(observerMaturity);
        var maturityDifference = Mathf.Abs(observerMaturity - assessorMaturity);
        var factor = Mathf.InverseLerp(3f, maturityDeviation, maturityDifference);
        switch (assessorMaturity)
        {
            case < 1 when assessorMaturity < observerMaturity && factor != 1f:
                factor = Mathf.Pow(factor, youthPenalty);
                break;
            case > 1 when assessorMaturity > observerMaturity && factor != 1f:
                factor = Mathf.Pow(factor, oldAgePenalty);
                break;
        }

        return factor;
    }
}