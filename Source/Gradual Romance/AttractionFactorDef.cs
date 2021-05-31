using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.Grammar;

namespace Gradual_Romance
{


    public class AttractionFactorCategoryDef : Def
    {
        //when always recalculate is on, GR never caches the result.
        public bool alwaysRecalculate = false;
        //when chance only is on, the result of the category is never reflected in the total attraction. For example, if you have an attraction factor based on the observer's
        //current mood, they aren't less attracted to the other pawn, just not in the mood.
        public bool chanceOnly = false;
        //when true, this category is only calculated for romance chance.
        public bool onlyForRomance = false;
    }

    public class AttractionFactorDef : Def
    {
        public AttractionCalculator calculator
        {
            get
            {
                if (this.calcInt == null)
                {
                    this.calcInt = (AttractionCalculator)Activator.CreateInstance(this.calculatorClass);
                    this.calcInt.def = this;
                }
                return this.calcInt;
            }
        }
        public AttractionFactorCategoryDef category;
        public List<PersonalityNodeModifier> personalityImportanceModifiers = new List<PersonalityNodeModifier> { };
        public List<TraitModifier> traitImportanceModifiers = new List<TraitModifier> { };
        public List<PersonalityNodeModifier> lowAttractionPersonalityModifiers = new List<PersonalityNodeModifier> { };
        public List<PersonalityNodeModifier> highAttractionPersonalityModifiers = new List<PersonalityNodeModifier> { };
        public List<TraitModifier> lowAttractionTraitModifiers = new List<TraitModifier> { };
        public List<TraitModifier> highAttractionTraitModifiers = new List<TraitModifier> { };
        public List<TraitDef> requiredTraits = new List<TraitDef> { };
        public List<TraitDef> nullifyingTraits = new List<TraitDef> { };
        public List<TraitDef> reversingTraits = new List<TraitDef> { };
        public RulePackDef intriguedByText;
        public RulePackDef reactionNegativeText;
        public RulePackDef reactionPositiveText;
        public int maxFactor = 300;
        public int minFactor = 0;
        public string reasonVeryHigh;
        public string reasonHigh;
        public string reasonLow;
        public string reasonVeryLow;
        public bool hidden = false;
        public bool needsSight = false;
        public bool needsHearing = false;
        private Type calculatorClass = typeof(AttractionCalculator);
        [Unsaved]
        private AttractionCalculator calcInt;

    }

    public class GenderModifier
    {
        Gender gender;
        float modifier;
    }



}






