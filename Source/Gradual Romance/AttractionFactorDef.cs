using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class AttractionFactorDef : Def
    {
        private readonly Type calculatorClass = typeof(AttractionCalculator);
        [Unsaved] private AttractionCalculator calcInt;
        public AttractionFactorCategoryDef category;
        public bool hidden = false;
        public List<PersonalityNodeModifier> highAttractionPersonalityModifiers = new List<PersonalityNodeModifier>();
        public List<TraitModifier> highAttractionTraitModifiers = new List<TraitModifier>();
        public RulePackDef intriguedByText;
        public List<PersonalityNodeModifier> lowAttractionPersonalityModifiers = new List<PersonalityNodeModifier>();
        public List<TraitModifier> lowAttractionTraitModifiers = new List<TraitModifier>();
        public int maxFactor = 300;
        public int minFactor = 0;
        public bool needsHearing = false;
        public bool needsSight = false;
        public List<TraitDef> nullifyingTraits = new List<TraitDef>();
        public List<PersonalityNodeModifier> personalityImportanceModifiers = new List<PersonalityNodeModifier>();
        public RulePackDef reactionNegativeText;
        public RulePackDef reactionPositiveText;
        public string reasonHigh;
        public string reasonLow;
        public string reasonVeryHigh;
        public string reasonVeryLow;
        public List<TraitDef> requiredTraits = new List<TraitDef>();
        public List<TraitDef> reversingTraits = new List<TraitDef>();
        public List<TraitModifier> traitImportanceModifiers = new List<TraitModifier>();

        public AttractionCalculator calculator
        {
            get
            {
                if (calcInt != null)
                {
                    return calcInt;
                }

                calcInt = (AttractionCalculator) Activator.CreateInstance(calculatorClass);
                calcInt.def = this;

                return calcInt;
            }
        }
    }
}