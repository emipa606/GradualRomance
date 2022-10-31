using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_AgeDifference : AttractionCalculator
{
    public const float ageDeviationDampener = 0.5f;

    public override bool Check(Pawn observer, Pawn assessed)
    {
        return false;
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        return 1f;
    }
}