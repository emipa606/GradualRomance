using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Options : AttractionCalculator
{
    public override float Calculate(Pawn observer, Pawn assessed)
    {
        return 1f;
    }
}