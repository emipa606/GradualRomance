using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_AgeDifference : AttractionCalculator
    {
        public const float ageDeviationDampener = 0.5f;

        public override bool Check(Pawn observer, Pawn assessed)
        {
            return false;
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            /*
            XenoRomanceExtension observerExtension = observer.def.GetModExtension<XenoRomanceExtension>();
            XenoRomanceExtension assessedExtension = assessed.def.GetModExtension<XenoRomanceExtension>();
            float assessedMaturityPct = Mathf.Clamp01(Mathf.InverseLerp(assessedExtension.youngAdultAge, assessed.RaceProps.lifeExpectancy,assessed.ageTracker.AgeBiologicalYearsFloat));
            float observerMaturityPct = Mathf.Clamp01(Mathf.InverseLerp(observerExtension.youngAdultAge, observer.RaceProps.lifeExpectancy, observer.ageTracker.AgeBiologicalYearsFloat));
            float observerAgeTolerance = GenMath.LerpDoubleClamped(observerExtension.youngAdultAge, observerExtension.midlifeAge, observerExtension.minimumAgeDeviation, observerExtension.maximumAgeDeviation, observer.ageTracker.AgeBiologicalYearsFloat);
            float ageTolerancePct = ( observerAgeTolerance / observer.RaceProps.lifeExpectancy);
            float ageDifference = Mathf.Abs(observerMaturityPct - assessedMaturityPct);
            return Mathf.Clamp01(Mathf.Pow(ageTolerancePct / ageDifference, ageDeviationDampener));
            */


            //People are generally attracted to members of their own age group.
            //For young pawns, this is very significant.
            //As pawns get older, age is less of a factor.
            /*
            float observerAge = observer.ageTracker.AgeBiologicalYearsFloat;
            float assessedAge = assessed.ageTracker.AgeBiologicalYearsFloat;
            float ageDifference = Math.Abs(observerAge - assessedAge);
            float acceptableAgeDifference = AttractionUtility.GetAgeDeviation(observer);
            if (ageDifference - acceptableAgeDifference <= 0f)
            {
                return 1f;
            }

            float ageFactor = acceptableAgeDifference / ageDifference;
            
            //Makes females pawns more tolerant of old age difference.
            //Some people really don't like this, which is why it can be toggled off in the settings.
            if (GRHelper.ShouldApplyFemaleDifference(observer.gender))
            {
                if (observerAge < assessedAge)
                {
                    ageFactor = Mathf.Pow(ageFactor, GenderedAgePreference);
                }
            }
            */
            return 1f;
        }
    }
}