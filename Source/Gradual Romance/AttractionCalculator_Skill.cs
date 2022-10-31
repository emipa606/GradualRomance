using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Skill : AttractionCalculator
{
    private const float SkillAttractivenessDampenerUpper = 0.5f;

    private const float SkillAttractivenessDampenerLower = 0.2f;

    public override bool Check(Pawn observer, Pawn assessed)
    {
        return observer.IsColonist && assessed.IsColonist;
    }

    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var observerValue = AttractionUtility.GetObjectiveSkillAttractiveness(observer);
        var assessedValue = AttractionUtility.GetObjectiveSkillAttractiveness(assessed);
        var value = assessedValue / observerValue;
        value = Mathf.Pow(value, value < 1 ? SkillAttractivenessDampenerUpper : SkillAttractivenessDampenerLower);

        return value;
    }
}