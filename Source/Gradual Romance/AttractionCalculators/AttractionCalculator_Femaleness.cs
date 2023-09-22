using Psychology;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Femaleness : AttractionCalculator
{
    public override bool Check(Pawn observer, Pawn assessed)
    {
        if (assessed.gender != Gender.Female)
        {
            return false;
        }

        if (!PsychologySettings.enableKinsey || !PsycheHelper.PsychologyEnabled(observer))
        {
            return true;
        }

        return PsycheHelper.Comp(observer).Sexuality.kinseyRating != 3;
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        if (!AttractionUtility.IsGynephilic(observer))
        {
            return 0f;
        }

        return AttractionUtility.IsWeaklyGynephilic(observer) ? 0.5f : 1f;
    }
}