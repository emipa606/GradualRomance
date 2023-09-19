using UnityEngine;
using Verse;

namespace Gradual_Romance;

public static class AgeCalculationUtility
{
    // A value that determines how much (0 = none, 1 = all) of the observed pawn's maturity is ignored/excused when
    // determining how "close" the two pawns maturities are.
    public const float pctAgeDeviation = 0.05f;
    // A value proportional to how much the maturity factor of an assessor is decreased/penalized when the assessor is
    // a child and younger than the observed.
    public const float youthPenalty = 2f;
    // A value proportional to how much the maturity factor of an assessor is decreased/penalized when the assessor is
    // an adult and older than the observed.
    public const float oldAgePenalty = 1.5f;

    // TODO: This is assuming certain properties about the maturity curve, such as a value of 3f being the max. This
    // should not be done, as the maturity curve is defined in XML.
    public static readonly SimpleCurve recoveryTimeByMaturity = new()
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

    // Computes a float value that represents the maturity of the provided pawn, which largely depends on the pawn's
    // gender and age. Returns a value of 1 if a value can not be computed (e.g., for animal pawns).
    public static float GetMaturity(Pawn pawn)
    {
        SimpleCurve maturityCurve = GradualRomanceMod.GetMaturityCurveFor(pawn);
        // This is "present" for all ThingDef that inherit from defName="human". But attempts to get the maturity of,
        // say, animals would encounter a null SimpleCurve object.
        //
        // TODO: Is there a better "default" value to return. Based on how the curve is defined in CoreDefPatches.xml,
        // the curve ranges from 0.01 at birth to 3 at "end of expected life expectancy". A value of 1 is used for
        // "reproductive age" (18yo). That seems like as good a default to use as any. This is, however, making an
        // assumption about what the maturity value for "reproductive age" is. Possibly an ever better solution would
        // be to return an error state to the calling function (e.g., change this into a "TryGetMaturity"), and the
        // caller can decide what to do.
        if (maturityCurve is null)
            return 1f;

        return maturityCurve.Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
    }

    // Calculates a float value in the range [0, 1], whose value is larger when the maturity (re: biological age)
    // values of the two provided pawns are closer, tailored to the assessor's preferences. It is adjusted for when
    // the assessor is a child and younger than observer (and vice versa), and boosts the value when the two pawns
    // have approximately close maturity values.
    public static float GetMaturityFactor(Pawn observer, Pawn assessor)
    {
        // TODO: The names "observer" and "assessor" for the two pawn arguments to this function are ambiguous. Find
        // better names (based on how this function is being called), and rename them to be more clear. If anything,
        // "observer" should be something like "observed".

        var observerMaturity = GetMaturity(observer);
        var assessorMaturity = GetMaturity(assessor);
        var maturityDeviation = MaturityDeviation(observerMaturity);
        var maturityDifference = Mathf.Abs(observerMaturity - assessorMaturity);

        // TODO: The hard-coded value of 3f here represents the maximum maturity possible (at age 80+). It should
        // instead be computed dynamically, as the curve used is defined in XML.
        //
        // The InverseLerp here is computing how far maturityDifference is between 3f (the largest possible maturity
        // difference, with observer and assessor being on the two ends of the age spectrum), and maturityDeviation.
        // That latter value is used instead of 0, presumably, to ignore "small" differences in maturity between the
        // observer and assessor. That is, based on the constant pctAgeDeviation defined in this file, a difference of
        // maturity between the observer and assessor that is <= 5% of the observer's maturity is ignored (treated as
        // if the two pawns' maturities were equal).
        //
        // As a result, factor here is a lower value (larger penalty) the larger the difference is between the two
        // pawns, but ignores small differences.
        var factor = Mathf.InverseLerp(3f, maturityDeviation, maturityDifference);

        // TODO: This is assuming a value of 1 is used for the maturity tier "reproductive age" stored in
        // assessorMaturity, and is assuming this is the case for both male and female genders. This value should be
        // computed dynamically, as the curves used are defined in XML.
        //
        // This switch statement makes the value of factor smaller when the assessor is a child and younger than the
        // observer, or the assessor is an adult and older than the observer. Note that, based on the current values
        // for youthPenalty and oldAgePenalty in this file, the former case is a more severe penalty than the latter
        // (younger pawns have a larger penalty to their attraction towards older pawns than older pawns have towards
        // younger pawns).
        switch (assessorMaturity)
        {
            // TODO: The test for "factor != 1f" (a) is likely very rarely false, due to float values approximation,
            // and (b) has no functional effect on the result, as 1 raised to any power is still 1. It should probably
            // be removed.
            // TODO: What about the other cases? E.g., assessorMaturity < 1 when assessorMaturity >= observerMaturity?
            // Also, simply when assessorMaturity == 1? (Though, as just stated, because of float approximation, this
            // would likely only very infrequently be encountered.)

            // When the observer is below "reproductive age" (18yo) and is younger than the observer, make the maturity
            // factor more severe (smaller, as it is always <= 1). Note, however, that, even if the assessor has a
            // lower maturity than the observer, if they are close enough, factor will be very close to 1, making this
            // operation have a very small effect.
            case < 1 when assessorMaturity < observerMaturity && factor != 1f:
                factor = Mathf.Pow(factor, youthPenalty);
                break;
            // As above, but the assessor is above "reporductive age" (18yo) and is older than the observer.
            case > 1 when assessorMaturity > observerMaturity && factor != 1f:
                factor = Mathf.Pow(factor, oldAgePenalty);
                break;
        }

        return factor;
    }
}