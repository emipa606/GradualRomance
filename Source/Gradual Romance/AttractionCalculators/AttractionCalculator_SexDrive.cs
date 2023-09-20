using Psychology;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_SexDrive : AttractionCalculator
{
    public override float Calculate(Pawn observer, Pawn assessed)
    {
        return PsycheHelper.Comp(observer).Sexuality.AdjustedSexDrive;
    }
}