using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class AttractionFactorDef : Def
{
    private readonly Type calculatorClass = typeof(AttractionCalculator);
    public readonly bool hidden = false;

    public readonly List<PersonalityNodeModifier> highAttractionPersonalityModifiers =
        [];

    public readonly List<TraitModifier> highAttractionTraitModifiers = [];

    public readonly List<PersonalityNodeModifier> lowAttractionPersonalityModifiers =
        [];

    public readonly List<TraitModifier> lowAttractionTraitModifiers = [];
    public readonly int maxFactor = 300;
    public readonly int minFactor = 0;
    public readonly bool needsHearing = false;
    public readonly bool needsSight = false;
    public readonly List<TraitDef> nullifyingTraits = [];
    public readonly List<PersonalityNodeModifier> personalityImportanceModifiers = [];
    public readonly List<TraitDef> requiredTraits = [];
    public readonly List<TraitDef> reversingTraits = [];
    public readonly List<TraitModifier> traitImportanceModifiers = [];
    [Unsaved] private AttractionCalculator calcInt;
    public AttractionFactorCategoryDef category;
    public RulePackDef intriguedByText;
    public RulePackDef reactionNegativeText;
    public RulePackDef reactionPositiveText;
    public string reasonHigh;
    public string reasonLow;
    public string reasonVeryHigh;
    public string reasonVeryLow;

    public AttractionCalculator calculator
    {
        get
        {
            if (calcInt != null)
            {
                return calcInt;
            }

            calcInt = (AttractionCalculator)Activator.CreateInstance(calculatorClass);
            calcInt.def = this;

            return calcInt;
        }
    }
}