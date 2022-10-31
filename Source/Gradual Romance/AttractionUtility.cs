using System.Collections.Generic;
using System.Linq;
using System.Text;
using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance;

public static class AttractionUtility
{
    private const float veryHighReasonThreshold = 1.75f;
    private const float highReasonThreshold = 1.2f;
    private const float lowReasonThreshold = 0.8f;
    private const float veryLowReasonThreshold = 0.25f;


    private const float YoungAge = 10f;

    private const float AdultAge = 18f;

    private const float OldAge = 50f;

    private const float ElderAge = 65f;

    private const float AncientAge = 90f;

    private const float MinAge = 18f;

    private const float GenderedAgePreference = 0.6f;
    private const float GreedWealthPreference = 1.5f;
    private const float PressureStartsAtOpinionDifference = 15f;

    private const float SkillAttractivenessDampenerUpper = 0.5f;

    private const float SkillAttractivenessDampenerLower = 0.2f;

    private const float FriendAttractionDampener = 0.4f;

    private const float MaleSkillEvaluationFactor = 0.5f;
    private const float MaleBeautyEvaluationFactor = 1.5f;

    private static readonly SimpleCurve AgeDeviation = new SimpleCurve
    {
        new CurvePoint(14, 1f),
        new CurvePoint(16, 2f),
        new CurvePoint(18, 3f),
        new CurvePoint(20, 4f),
        new CurvePoint(22, 5f),
        new CurvePoint(24, 6f),
        new CurvePoint(28, 7f),
        new CurvePoint(32, 8f),
        new CurvePoint(36, 9f),
        new CurvePoint(40, 10f)
    };

    private static readonly SimpleCurve SkillAttractivenessCurve = new SimpleCurve
    {
        new CurvePoint(0, 0.25f),
        new CurvePoint(2, 1f),
        new CurvePoint(4, 2f),
        new CurvePoint(8, 4f),
        new CurvePoint(12, 8f),
        new CurvePoint(14, 16f),
        new CurvePoint(16, 32f),
        new CurvePoint(18, 64f),
        new CurvePoint(20, 128f)
    };

    // AGE TOOLS //
    public static bool IsAgeAppropriate(Pawn pawn)
    {
        return pawn.ageTracker.AgeBiologicalYearsFloat > 18f;
    }

    public static float GetAgeDeviation(Pawn pawn)
    {
        if (pawn.def.defName != "Human")
        {
            return pawn.RaceProps.lifeExpectancy / 8;
        }

        return AgeDeviation.Evaluate(pawn.ageTracker.AgeBiologicalYears);
    }

    /// KINSEY TOOLS ///
    /// 
    /// The Kinsey system doesn't really reflect attraction, but rather under what circumstances they will pursue homosexual/heterosexual relationships
    /// Exclusively - will never consider relations outside of one gender
    /// Weakly - the pawn will interact informally with dispreffered sex pawns, and will never pursue formal relationships with them.
    /// Occasionally - the pawn interacts with dispreferred pawns freely, but only prefers formal relationships rarely.
    /// Bisexual - no limit to either male or female relationships.

    //Whether a pawn can be attracted to men at all.
    public static bool IsAndrophilic(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn) && PsychologySettings.enableKinsey)
        {
            var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
            switch (pawn.gender)
            {
                case Gender.Female when kinsey < 6:
                case Gender.Male when kinsey > 0:
                    return true;
            }
        }
        else
        {
            if (pawn.gender == Gender.Male && pawn.story.traits.HasTrait(TraitDefOf.Gay))
            {
                return true;
            }

            if (pawn.gender == Gender.Female && pawn.story.traits.HasTrait(TraitDefOf.Gay) == false)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsWeaklyAndrophilic(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        switch (pawn.gender)
        {
            case Gender.Female when kinsey == 5:
            case Gender.Male when kinsey == 1:
                return true;
            default:
                return false;
        }
    }

    public static bool IsOccasionallyAndrophilic(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        switch (pawn.gender)
        {
            case Gender.Female when kinsey == 4:
            case Gender.Male when kinsey == 2:
                return true;
            default:
                return false;
        }
    }

    public static bool IsOccasionallyGynephilic(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        switch (pawn.gender)
        {
            case Gender.Female when kinsey == 2:
            case Gender.Male when kinsey == 4:
                return true;
            default:
                return false;
        }
    }

    public static bool IsWeaklyGynephilic(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        switch (pawn.gender)
        {
            case Gender.Female when kinsey == 1:
            case Gender.Male when kinsey == 5:
                return true;
            default:
                return false;
        }
    }

    public static bool IsExclusivelyAndrophilic(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn) && PsychologySettings.enableKinsey)
        {
            var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
            if (pawn.gender == Gender.Female && kinsey == 0)
            {
                return true;
            }

            if (pawn.gender == Gender.Male && kinsey == 6)
            {
                return true;
            }
        }
        else
        {
            if (pawn.gender == Gender.Male && pawn.story.traits.HasTrait(TraitDefOf.Gay))
            {
                return true;
            }

            if (pawn.gender == Gender.Female && pawn.story.traits.HasTrait(TraitDefOf.Gay) == false)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsGynephilic(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn) && PsychologySettings.enableKinsey)
        {
            var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
            if (pawn.gender == Gender.Male && kinsey < 6)
            {
                return true;
            }

            if (pawn.gender == Gender.Female && kinsey > 0)
            {
                return true;
            }
        }
        else
        {
            if (pawn.gender == Gender.Female && pawn.story.traits.HasTrait(TraitDefOf.Gay))
            {
                return true;
            }

            if (pawn.gender == Gender.Male && pawn.story.traits.HasTrait(TraitDefOf.Gay) == false)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsExclusivelyGynephilic(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn) && PsychologySettings.enableKinsey)
        {
            var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
            if (pawn.gender == Gender.Female && kinsey == 6)
            {
                return true;
            }

            if (pawn.gender == Gender.Male && kinsey == 0)
            {
                return true;
            }
        }
        else
        {
            if (pawn.gender == Gender.Female && pawn.story.traits.HasTrait(TraitDefOf.Gay))
            {
                return true;
            }

            if (pawn.gender == Gender.Male && pawn.story.traits.HasTrait(TraitDefOf.Gay) == false)
            {
                return true;
            }
        }

        return false;
    }

    public static bool WouldConsiderFormalRelationship(Pawn pawn, Pawn other)
    {
        if (PsycheHelper.PsychologyEnabled(pawn) && PsychologySettings.enableKinsey)
        {
            if (other.gender == Gender.Male)
            {
                var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
                if (pawn.gender == Gender.Female && kinsey >= 5)
                {
                    return false;
                }

                if (pawn.gender == Gender.Male && kinsey <= 1)
                {
                    return false;
                }
            }
            else
            {
                var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
                if (pawn.gender == Gender.Female && kinsey <= 1)
                {
                    return false;
                }

                if (pawn.gender == Gender.Male && kinsey >= 5)
                {
                    return false;
                }
            }
        }
        else
        {
            if (other.gender == Gender.Male)
            {
                if (pawn.gender == Gender.Female && pawn.story.traits.HasTrait(TraitDefOf.Gay))
                {
                    return false;
                }

                if (pawn.gender == Gender.Male && pawn.story.traits.HasTrait(TraitDefOf.Gay) == false)
                {
                    return false;
                }
            }
            else
            {
                if (pawn.gender == Gender.Male && pawn.story.traits.HasTrait(TraitDefOf.Gay))
                {
                    return false;
                }

                if (pawn.gender == Gender.Female && pawn.story.traits.HasTrait(TraitDefOf.Gay) == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool IsBisexual(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        return kinsey is > 0 and < 6;
    }

    public static bool IsStronglyBisexual(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        return kinsey is > 1 and < 5;
    }

    public static bool IsWeaklyBisexual(Pawn pawn)
    {
        if (!PsycheHelper.PsychologyEnabled(pawn) || !PsychologySettings.enableKinsey)
        {
            return false;
        }

        var kinsey = PsycheHelper.Comp(pawn).Sexuality.kinseyRating;
        return kinsey == 1 || kinsey == 5;
    }

    public static bool IsAromantic(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn))
        {
            return PsycheHelper.Comp(pawn).Sexuality.AdjustedRomanticDrive < 0.01f;
        }

        return false;
    }

    public static bool IsAsexual(Pawn pawn)
    {
        if (PsycheHelper.PsychologyEnabled(pawn))
        {
            return PsycheHelper.Comp(pawn).Sexuality.AdjustedSexDrive < 0.01f;
        }

        return false;
    }


    private static bool IsSociallyIncompetent(Pawn pawn)
    {
        return pawn.WorkTagIsDisabled(WorkTags.Social) || pawn.skills.GetSkill(SkillDefOf.Social).Level < 2;
    }

    // ATTRACTION FACTOR TOOLS //

    private static float CalculateFactor(Pawn observer, Pawn assessed, AttractionFactorDef attractionFactor)
    {
        var calculator = attractionFactor.calculator;
        if (attractionFactor.needsSight && observer.health.capacities.GetLevel(PawnCapacityDefOf.Sight) <= .1f)
        {
            return 1f;
        }

        if (attractionFactor.needsHearing && observer.health.capacities.GetLevel(PawnCapacityDefOf.Hearing) <= .1f)
        {
            return 1f;
        }

        if (attractionFactor.requiredTraits.Count > 0)
        {
            var hasRequiredTrait = false;
            foreach (var trait in attractionFactor.requiredTraits)
            {
                if (!observer.story.traits.HasTrait(trait))
                {
                    continue;
                }

                hasRequiredTrait = true;
                break;
            }

            if (hasRequiredTrait == false)
            {
                return 1f;
            }
        }

        if (attractionFactor.nullifyingTraits.Any())
        {
            var hasNullifyingTraits = false;
            foreach (var trait in attractionFactor.nullifyingTraits)
            {
                if (!observer.story.traits.HasTrait(trait))
                {
                    continue;
                }

                hasNullifyingTraits = true;
                break;
            }

            if (hasNullifyingTraits)
            {
                return 1f;
            }
        }

        if (!calculator.Check(observer, assessed))
        {
            return 1f;
        }

        var value = calculator.Calculate(observer, assessed);

        if (value == 1f)
        {
            return 1f;
        }

        if (value == 0f)
        {
            return 0f;
        }

        if (attractionFactor.reversingTraits.Any())
        {
            var hasReversingTraits = false;
            foreach (var trait in attractionFactor.reversingTraits)
            {
                if (!observer.story.traits.HasTrait(trait))
                {
                    continue;
                }

                hasReversingTraits = true;
                break;
            }

            if (hasReversingTraits)
            {
                value = value < 1
                    ? Mathf.Max(Mathf.Lerp(2.5f, 1f, value), 0.05f)
                    : Mathf.Pow(value, value - (2 * value));
            }
        }

        if (attractionFactor.traitImportanceModifiers.Any())
        {
            foreach (var traitMod in attractionFactor.traitImportanceModifiers)
            {
                if (!observer.story.traits.HasTrait(traitMod.trait))
                {
                    continue;
                }

                if (traitMod.modifier == 0)
                {
                    return 0f;
                }

                value = Mathf.Pow(value, traitMod.modifier);
            }
        }

        if (attractionFactor.personalityImportanceModifiers.Any() &&
            PsycheHelper.PsychologyEnabled(observer) && GradualRomanceMod.AttractionCalculation ==
            GradualRomanceMod.AttractionCalculationSetting.Complex)
        {
            foreach (var perMod in attractionFactor.personalityImportanceModifiers)
            {
                if (perMod.reverse == false)
                {
                    value = Mathf.Pow(value,
                        Mathf.Lerp(0.5f, 1.5f,
                            PsycheHelper.Comp(observer).Psyche.GetPersonalityRating(perMod.personalityNode)) *
                        perMod.modifier);
                }
                else
                {
                    value = Mathf.Pow(value,
                        Mathf.Lerp(0.5f, 1.5f,
                            1f - PsycheHelper.Comp(observer).Psyche.GetPersonalityRating(perMod.personalityNode)) *
                        perMod.modifier);
                }
            }
        }

        if (value < 1)
        {
            if (attractionFactor.lowAttractionTraitModifiers.Any())
            {
                foreach (var traitMod in attractionFactor.lowAttractionTraitModifiers)
                {
                    if (!observer.story.traits.HasTrait(traitMod.trait))
                    {
                        continue;
                    }

                    if (traitMod.modifier == 0)
                    {
                        return 0f;
                    }

                    value = Mathf.Pow(value, traitMod.modifier);
                }
            }

            if (!attractionFactor.lowAttractionPersonalityModifiers.Any() ||
                !PsycheHelper.PsychologyEnabled(observer) || GradualRomanceMod.AttractionCalculation !=
                GradualRomanceMod.AttractionCalculationSetting.Complex)
            {
                return Mathf.Clamp(value, attractionFactor.minFactor, attractionFactor.maxFactor);
            }

            foreach (var perMod in attractionFactor.lowAttractionPersonalityModifiers)
            {
                if (perMod.reverse == false)
                {
                    value = Mathf.Pow(value,
                        Mathf.Lerp(0.5f, 1.5f,
                            PsycheHelper.Comp(observer).Psyche.GetPersonalityRating(perMod.personalityNode)) *
                        perMod.modifier);
                }
                else
                {
                    value = Mathf.Pow(value,
                        Mathf.Lerp(0.5f, 1.5f,
                            1f - PsycheHelper.Comp(observer).Psyche
                                .GetPersonalityRating(perMod.personalityNode)) * perMod.modifier);
                }
            }
        }
        else
        {
            if (attractionFactor.highAttractionTraitModifiers.Any())
            {
                foreach (var traitMod in attractionFactor.highAttractionTraitModifiers)
                {
                    if (!observer.story.traits.HasTrait(traitMod.trait))
                    {
                        continue;
                    }

                    if (traitMod.modifier == 0)
                    {
                        return 0f;
                    }

                    value = Mathf.Pow(value, traitMod.modifier);
                }
            }

            if (!attractionFactor.highAttractionPersonalityModifiers.Any() ||
                !PsycheHelper.PsychologyEnabled(observer) || GradualRomanceMod.AttractionCalculation !=
                GradualRomanceMod.AttractionCalculationSetting.Complex)
            {
                return Mathf.Clamp(value, attractionFactor.minFactor, attractionFactor.maxFactor);
            }

            foreach (var perMod in attractionFactor.highAttractionPersonalityModifiers)
            {
                if (perMod.reverse == false)
                {
                    value = Mathf.Pow(value,
                        Mathf.Lerp(0.5f, 1.5f,
                            PsycheHelper.Comp(observer).Psyche.GetPersonalityRating(perMod.personalityNode)) *
                        perMod.modifier);
                }
                else
                {
                    value = Mathf.Pow(value,
                        Mathf.Lerp(0.5f, 1.5f,
                            1f - PsycheHelper.Comp(observer).Psyche
                                .GetPersonalityRating(perMod.personalityNode)) * perMod.modifier);
                }
            }
        }

        return Mathf.Clamp(value, attractionFactor.minFactor, attractionFactor.maxFactor);
    }

    public static float CalculateAttractionCategory(AttractionFactorCategoryDef category, Pawn observer,
        Pawn assessed)
    {
        return CalculateAttractionCategory(category, observer, assessed, out _, out _,
            out _, out _, out _);
    }

    public static float CalculateAttractionCategory(AttractionFactorCategoryDef category, Pawn observer,
        Pawn assessed, out List<AttractionFactorDef> veryLowFactors, out List<AttractionFactorDef> lowFactors,
        out List<AttractionFactorDef> highFactors, out List<AttractionFactorDef> veryHighFactors,
        out AttractionFactorDef reasonForInstantFailure)
    {
        //Log.Message("Method start.");
        //Log.Message("Making null values.");
        veryHighFactors = new List<AttractionFactorDef>();
        highFactors = new List<AttractionFactorDef>();
        lowFactors = new List<AttractionFactorDef>();
        veryLowFactors = new List<AttractionFactorDef>();
        reasonForInstantFailure = null;
        //Log.Message("Retrieving factor defs.");
        var allFactors = from def in DefDatabase<AttractionFactorDef>.AllDefsListForReading
            where def.category == category
            select def;

        var attraction = 1f;
        //Log.Message("Starting factor calculations for " + allFactors.Count().ToString() + "factors");

        foreach (var factor in allFactors)
        {
            if (factor.calculator.Check(observer, assessed) == false)
            {
                continue;
            }

            //Log.Message("Doing calculation for " + factor.defName);
            var result = CalculateFactor(observer, assessed, factor);
            switch (result)
            {
                case 1f:
                    continue;
                case 0f:
                {
                    if (GradualRomanceMod.detailedAttractionLogs)
                    {
                        Log.Message($"Instantly failed at {factor.defName}");
                    }

                    veryLowFactors.Add(factor);
                    reasonForInstantFailure = factor;
                    return 0f;
                }
            }

            if (factor.hidden)
            {
                continue;
            }

            switch (result)
            {
                //Log.Message("Sort factor into results.");
                case >= veryHighReasonThreshold:
                    veryHighFactors.Add(factor);
                    break;
                case >= highReasonThreshold:
                    highFactors.Add(factor);
                    break;
                case <= veryLowReasonThreshold:
                    veryLowFactors.Add(factor);
                    break;
                case <= lowReasonThreshold:
                    lowFactors.Add(factor);
                    break;
            }

            //Log.Message("Integrating result.");
            if (GradualRomanceMod.detailedAttractionLogs)
            {
                Log.Message(
                    $"{factor.defName}({observer.Name.ToStringShort},{assessed.Name.ToStringShort})({category.defName}): {result}=>{attraction * result}");
            }

            attraction *= result;
        }

        //Log.Message("Concluding method.");
        return attraction;
    }

    public static float CalculateAttraction(Pawn observer, Pawn assessed, bool attractionOnly,
        bool formalRelationship)
    {
        return CalculateAttraction(observer, assessed, attractionOnly, formalRelationship, out _,
            out _, out _, out _, out _);
    }

    public static float CalculateAttraction(Pawn observer, Pawn assessed, bool attractionOnly,
        bool formalRelationship, out List<AttractionFactorDef> veryLowFactors,
        out List<AttractionFactorDef> lowFactors, out List<AttractionFactorDef> highFactors,
        out List<AttractionFactorDef> veryHighFactors, out AttractionFactorDef reasonForInstantFailure)
    {
        veryLowFactors = new List<AttractionFactorDef>();
        lowFactors = new List<AttractionFactorDef>();
        highFactors = new List<AttractionFactorDef>();
        veryHighFactors = new List<AttractionFactorDef>();
        reasonForInstantFailure = null;
        var result = observer.GetComp<GRPawnComp>().RetrieveAttractionAndFactors(assessed, out veryLowFactors,
            out lowFactors, out highFactors, out veryHighFactors, formalRelationship, !attractionOnly);

        var allCategories = DefDatabase<AttractionFactorCategoryDef>.AllDefsListForReading;

        foreach (var category in allCategories)
        {
            /*
            if (!formalRelationship && category.onlyForRomance)
            {
                continue;
            }
            if (attractionOnly && category.chanceOnly)
            {
                continue;
            }
            */
            if (!category.alwaysRecalculate)
            {
                continue;
            }

            result *= CalculateAttractionCategory(category, observer, assessed, out var newVeryLowFactors,
                out var newLowFactors, out var newHighFactors, out var newVeryHighFactors,
                out var newReasonForInstantFailure);

            veryHighFactors.AddRange(newVeryHighFactors);
            highFactors.AddRange(newHighFactors);
            lowFactors.AddRange(newLowFactors);
            veryLowFactors.AddRange(newVeryLowFactors);
            if (reasonForInstantFailure == null && newReasonForInstantFailure != null)
            {
                reasonForInstantFailure = newReasonForInstantFailure;
            }
        }

        if (!formalRelationship)
        {
            veryHighFactors.RemoveAll(x => x.category.onlyForRomance);
            highFactors.RemoveAll(x => x.category.onlyForRomance);
            lowFactors.RemoveAll(x => x.category.onlyForRomance);
            veryLowFactors.RemoveAll(x => x.category.onlyForRomance);
        }

        if (attractionOnly)
        {
            return result;
        }

        veryHighFactors.RemoveAll(x => x.category.chanceOnly);
        highFactors.RemoveAll(x => x.category.chanceOnly);
        lowFactors.RemoveAll(x => x.category.chanceOnly);
        veryLowFactors.RemoveAll(x => x.category.chanceOnly);

        return result;
    }

    public static bool QuickCheck(Pawn observer, Pawn assessed)
    {
        if (!IsAgeAppropriate(observer) && !IsAgeAppropriate(assessed))
        {
            return false;
        }

        switch (assessed.gender)
        {
            case Gender.Male when IsExclusivelyGynephilic(observer):
            case Gender.Female when IsExclusivelyAndrophilic(observer):
                return false;
        }

        if (observer.def.defName == assessed.def.defName)
        {
            return true;
        }

        switch (GradualRomanceMod.extraspeciesRomance)
        {
            case GradualRomanceMod.ExtraspeciesRomanceSetting.NoXenoRomance:
            case GradualRomanceMod.ExtraspeciesRomanceSetting.OnlyXenophiles when !ModHooks.IsXenophile(observer):
                return false;
        }

        return !ModHooks.IsXenophobe(observer);
    }

    public static string WriteReasonsParagraph(Pawn observer, Pawn assessed,
        List<AttractionFactorDef> veryHighFactors, List<AttractionFactorDef> highFactors,
        List<AttractionFactorDef> lowFactors, List<AttractionFactorDef> veryLowFactors)
    {
        var paragraph = new StringBuilder();
        paragraph.AppendLine();
        paragraph.Append("WhatILike".Translate(observer.Named("PAWN1"), assessed.Named("PAWN2")));
        var positiveFactors = new List<string>();
        var negativeFactors = new List<string>();
        foreach (var factor in veryHighFactors)
        {
            string newString = factor.reasonVeryHigh.Formatted(observer.Named("PAWN1"), assessed.Named("PAWN2"));
            newString.UncapitalizeFirst();
            positiveFactors.Add(newString);
        }

        foreach (var factor in highFactors)
        {
            string newString = factor.reasonHigh.Formatted(observer.Named("PAWN1"), assessed.Named("PAWN2"));
            newString.UncapitalizeFirst();
            positiveFactors.Add(newString);
        }

        foreach (var factor in veryLowFactors)
        {
            string newString = factor.reasonVeryLow.Formatted(observer.Named("PAWN1"), assessed.Named("PAWN2"));
            newString.UncapitalizeFirst();
            negativeFactors.Add(newString);
        }

        foreach (var factor in lowFactors)
        {
            string newString = factor.reasonLow.Formatted(observer.Named("PAWN1"), assessed.Named("PAWN2"));
            newString.UncapitalizeFirst();
            negativeFactors.Add(newString);
        }

        if (!positiveFactors.Any())
        {
            paragraph.Append("NothingInParticular".Translate());
        }
        else
        {
            paragraph.Append(GRGrammarUtility.SayList(positiveFactors));
        }

        if (negativeFactors.Count != 0)
        {
            paragraph.Append(", " + "WhatIDislike".Translate(observer.Named("PAWN1"), assessed.Named("PAWN2")));
            paragraph.Append(GRGrammarUtility.SayList(negativeFactors));
            paragraph.Append(".");
        }
        else
        {
            paragraph.Append(".");
        }

        return paragraph.ToString();
    }


    // RELATION TOOLS //
    public static int GetRelationshipUnmodifiedOpinion(Pawn pawn, Pawn other)
    {
        if (!other.RaceProps.Humanlike || pawn == other)
        {
            return 0;
        }

        if (pawn.Dead)
        {
            return 0;
        }

        var num = 0;
        if (pawn.RaceProps.Humanlike)
        {
            num += pawn.needs.mood.thoughts.TotalOpinionOffset(other);
        }

        if (num != 0)
        {
            var num2 = 1f;
            var hediffs = pawn.health.hediffSet.hediffs;
            foreach (var hediff in hediffs)
            {
                if (hediff.CurStage != null)
                {
                    num2 *= hediff.CurStage.opinionOfOthersFactor;
                }
            }

            num = Mathf.RoundToInt(num * num2);
        }

        if (num > 0 && pawn.HostileTo(other))
        {
            num = 0;
        }

        return Mathf.Clamp(num, -100, 100);
    }

    public static float RelationshipStress(Pawn initiator, Pawn recipient, float modifier = 1f)
    {
        //Psychopaths are cold-hearted
        if (initiator.story.traits.HasTrait(TraitDefOf.Psychopath))
        {
            //But socially incompetent will still embarrass themselves
            if (IsSociallyIncompetent(initiator))
            {
                return 1f;
            }

            return 0.05f;
        }
        //Lechers are used to hitting on people.

        var pressure = modifier;
        if (initiator.story.traits.HasTrait(TraitDefOfPsychology.Lecher))
        {
            pressure *= 0.5f;
        }

        var opinionDifference = initiator.relations.OpinionOf(recipient) - recipient.relations.OpinionOf(initiator);
        pressure *= Mathf.Clamp(Mathf.InverseLerp(0f, PressureStartsAtOpinionDifference, opinionDifference), 0.05f,
            5f);
        //People with low mental break thresholds freak out more and say the wrong thing. 
        pressure *= Mathf.InverseLerp(0f, 0.05f, initiator.GetStatValue(StatDefOf.MentalBreakThreshold));
        //People with high social skill are less likely to pick bad flirts.
        var socialSkillFactor =
            Mathf.Clamp(
                Mathf.InverseLerp(4, 16, Mathf.Abs(initiator.skills.GetSkill(SkillDefOf.Social).Level - 20)), 0.01f,
                2f);
        pressure *= socialSkillFactor;
        return Mathf.Clamp(pressure, 0.2f, 5f);
    }

    public static float PropensityToSeduce(Pawn pawn)
    {
        var propensity = 1f;
        if (pawn.story.traits.HasTrait(TraitDefOfPsychology.Lecher))
        {
            propensity *= 1.5f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfPsychology.Prude))
        {
            propensity *= 0.5f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfPsychology.Codependent))
        {
            propensity *= 0.75f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfGR.Shy))
        {
            propensity *= 0.25f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfGR.Seductive))
        {
            propensity *= 2.5f;
        }

        if (!PsycheHelper.PsychologyEnabled(pawn))
        {
            return propensity;
        }

        var comp = PsycheHelper.Comp(pawn);
        propensity *= comp.Psyche.GetPersonalityRating(PersonalityNodeDefOfGR.Adventurous) + 0.5f;
        propensity *= comp.Psyche.GetPersonalityRating(PersonalityNodeDefOf.Extroverted) + 0.5f;
        propensity *= comp.Psyche.GetPersonalityRating(PersonalityNodeDefOfGR.Confident) + 0.1f;
        propensity *= 1.2f - comp.Psyche.GetPersonalityRating(PersonalityNodeDefOf.Pure);
        propensity *= 1.2f - comp.Psyche.GetPersonalityRating(PersonalityNodeDefOfGR.Moralistic);
        propensity *= comp.Sexuality.AdjustedSexDrive;

        return propensity;
    }

    public static float PropensityToBeSeduced(Pawn pawn)
    {
        var propensity = 1f;
        if (pawn.story.traits.HasTrait(TraitDefOfPsychology.Lecher))
        {
            propensity *= 1.5f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfPsychology.Prude))
        {
            propensity *= 0.5f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfPsychology.Codependent))
        {
            propensity *= 0.75f;
        }

        if (pawn.story.traits.HasTrait(TraitDefOfGR.Shy))
        {
            propensity *= 2f;
        }

        if (!PsycheHelper.PsychologyEnabled(pawn))
        {
            return propensity;
        }

        var comp = PsycheHelper.Comp(pawn);
        propensity *= comp.Psyche.GetPersonalityRating(PersonalityNodeDefOfGR.Adventurous) + 0.5f;
        propensity *= 1.5f - comp.Psyche.GetPersonalityRating(PersonalityNodeDefOfGR.Confident);
        propensity *= 1.5f - comp.Psyche.GetPersonalityRating(PersonalityNodeDefOf.Pure);
        propensity *= comp.Sexuality.AdjustedSexDrive;

        return propensity;
    }

    ///////SOCIAL ATTRACTIVENESS////////

    private static float GetAttractivenessForSkillLevel(int level)
    {
        return SkillAttractivenessCurve.Evaluate(level);
    }

    public static float GetObjectiveSkillAttractiveness(Pawn pawn)
    {
        var value = 0f;
        IEnumerable<SkillDef> allSkillDefs = DefDatabase<SkillDef>.AllDefsListForReading;
        foreach (var skill in allSkillDefs)
        {
            value += GetAttractivenessForSkillLevel(pawn.skills.GetSkill(skill).Level);
        }

        return value;
    }

    private static float EvaluateRoomAttractiveness(Room assessedRoom)
    {
        float roomTypeFactor;
        if (assessedRoom.Role == RoomRoleDefOf.Bedroom)
        {
            roomTypeFactor = 1f;
        }
        else if (assessedRoom.Role == RoomRoleDefOf.Barracks)
        {
            roomTypeFactor = 1f / assessedRoom.ContainedBeds.Count();
        }
        else
        {
            roomTypeFactor = 0.01f;
        }

        return assessedRoom.GetStat(RoomStatDefOf.Wealth) * roomTypeFactor;
    }

    public static float GetObjectiveWealthAttractiveness(Pawn pawn)
    {
        var observerRoom = pawn.ownership.OwnedRoom;
        return observerRoom == null ? 1f : EvaluateRoomAttractiveness(observerRoom);
    }

    /*
    private const float MinScarAttraction = 0.95f;
    private const float UpperScarAttraction = 0.9f;
    private const float LowerScarAttraction = 0.7f;
    */
}