using Psychology;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Femaleness : AttractionCalculator
    {
        public override bool Check(Pawn observer, Pawn assessed)
        {
            if (assessed.gender != Gender.Female)
            {
                return false;
            }

            if (!PsychologyBase.ActivateKinsey() || !PsycheHelper.PsychologyEnabled(observer))
            {
                return true;
            }

            if (PsycheHelper.Comp(observer).Sexuality.kinseyRating == 3)
            {
                return false;
            }

            return true;
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            if (!AttractionUtility.IsGynephilic(observer))
            {
                return 0f;
            }

            if (AttractionUtility.IsWeaklyGynephilic(observer))
            {
                return 0.5f;
            }

            return 1f;
        }
    }
}