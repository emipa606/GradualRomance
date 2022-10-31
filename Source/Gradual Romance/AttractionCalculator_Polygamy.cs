using Psychology;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Polygamy : AttractionCalculator
{
    public override bool Check(Pawn observer, Pawn assessed)
    {
        return assessed.story.traits.HasTrait(TraitDefOfPsychology.Polygamous);
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var weight = 1f;
        if (RelationshipUtility.IsPolygamist(assessed) && !RelationshipUtility.IsPolygamist(observer))
        {
            weight *= .85f;
        }
        else if (RelationshipUtility.IsPolygamist(assessed) && RelationshipUtility.IsPolygamist(observer))
        {
            weight *= 1.2f;
        }

        return weight;
    }
}