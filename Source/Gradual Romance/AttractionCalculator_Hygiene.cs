using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Gradual_Romance
{
    public class AttractionCalculator_Hygiene : AttractionCalculator
    {
        public override bool Check(Pawn observer, Pawn assessed)
        {
            return (ModHooks.UsingDubsHygiene() && assessed.needs.AllNeeds.Any<Need>(x => x.def.defName == "Hygiene"));
        }
        public override float Calculate(Pawn observer, Pawn assessed)
        {

            return Mathf.Clamp01(Mathf.Lerp(maxFilthPenalty, 1f, Mathf.Clamp01(Mathf.InverseLerp(filthMax, filthMin, ModHooks.GetHygieneNeed(assessed)))));
        }
        const float maxFilthPenalty = 0.75f;
        const float filthMin = 0.7f;
        const float filthMax = 0.1f;
    }


}
