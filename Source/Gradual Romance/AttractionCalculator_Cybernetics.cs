using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Cybernetics : AttractionCalculator
    {
        private const float ValueDampener = 0.1f;

        public override bool Check(Pawn observer, Pawn assessed)
        {
            if (GradualRomanceMod.AttractionCalculation != GradualRomanceMod.AttractionCalculationSetting.Complex)
            {
                return false;
            }
            /*

            if (!observer.story.traits.HasTrait(TraitDefOf.BodyPurist) || !assessed.story.traits.HasTrait(TraitDefOf.Transhumanist))
            {
                return false;
            }
            */

            if (assessed.health.hediffSet.CountAddedAndImplantedParts() <= 0)
            {
                return false;
            }

            return true;
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            var listOfAddedParts = new List<Hediff_AddedPart>();
            foreach (var hediff in assessed.health.hediffSet.hediffs)
            {
                if (hediff is Hediff_AddedPart)
                {
                    listOfAddedParts.Add(hediff as Hediff_AddedPart);
                }
            }

            var valueOfParts = 0f;
            foreach (var hediff in listOfAddedParts)
            {
                valueOfParts += hediff.def.spawnThingOnRemoved.BaseMarketValue;
            }

            valueOfParts = Mathf.Max(valueOfParts, 1f);
            var cyberFactor = Mathf.Pow(valueOfParts, ValueDampener);
            if (observer.story.traits.HasTrait(TraitDefOf.BodyPurist))
            {
                cyberFactor = Mathf.Pow(valueOfParts, -1);
            }

            return cyberFactor;
        }
    }
}