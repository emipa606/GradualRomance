using RimWorld;
using Verse;

namespace Gradual_Romance;

public class AttractionCalculator_Body : AttractionCalculator
{
    public override float Calculate(Pawn observer, Pawn assessed)
    {
        var bodyFactor = 1f;
        if (assessed.story.traits.HasTrait(TraitDefOfGR.Wimp))
        {
            bodyFactor *= 0.8f;
        }

        if (assessed.story.traits.HasTrait(TraitDefOfGR.Nimble))
        {
            bodyFactor *= 1.2f;
        }

        if (assessed.story.traits.HasTrait(TraitDefOf.Tough))
        {
            bodyFactor *= 1.2f;
        }

        if (assessed.story.traits.HasTrait(TraitDefOf.SpeedOffset))
        {
            var x = assessed.story.traits.DegreeOfTrait(TraitDefOf.SpeedOffset);
            switch (x)
            {
                case -1:
                    bodyFactor *= 0.8f;
                    break;
                case 1:
                    bodyFactor *= 1.2f;
                    break;
                case 2:
                    bodyFactor *= 1.4f;
                    break;
            }
        }

        if (assessed.story.traits.HasTrait(TraitDefOfGR.Immunity))
        {
            var x = assessed.story.traits.DegreeOfTrait(TraitDefOfGR.Immunity);
            switch (x)
            {
                case -1:
                    bodyFactor *= 0.8f;
                    break;
                case 1:
                    bodyFactor *= 1.2f;
                    break;
            }
        }

        if (!assessed.story.bodyType.HasModExtension<GRBodyTypeExtension>())
        {
            return bodyFactor;
        }

        var extension = assessed.story.bodyType.GetModExtension<GRBodyTypeExtension>();
        switch (extension.attractiveForGender)
        {
            case Gender.Male:
            {
                if (!GRHelper.ShouldApplyMaleDifference(assessed.gender))
                {
                    bodyFactor *= extension.attractivenessFactor;
                }

                break;
            }
            case Gender.Female:
            {
                if (!GRHelper.ShouldApplyFemaleDifference(assessed.gender))
                {
                    bodyFactor *= extension.attractivenessFactor;
                }

                break;
            }
            default:
                bodyFactor *= extension.attractivenessFactor;
                break;
        }


        return bodyFactor;
    }
}