using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_FemaleRelationshipReluctance : AttractionCalculator
{
    public override bool Check(Pawn observer, Pawn assessed)
    {
        return assessed.gender == Gender.Female;
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        if (AttractionUtility.IsWeaklyGynephilic(observer) || AttractionUtility.IsExclusivelyAndrophilic(observer))
        {
            return 0f;
        }

        return AttractionUtility.IsOccasionallyGynephilic(observer) ? 0.5f : 1f;
    }
}