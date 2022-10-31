using System.Collections.Generic;
using Psychology;
using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class FlirtReactionWorker_Ignorant : FlirtReactionWorker
{
    public override void GiveThoughts(Pawn initiator, Pawn recipient, out List<RulePackDef> yetMoreSentencePacks)
    {
        yetMoreSentencePacks = new List<RulePackDef>();
        var interpretChance = 1f;
        if (PsycheHelper.PsychologyEnabled(initiator))
        {
            interpretChance *= 0.5f + PsycheHelper.Comp(initiator).Psyche
                .GetPersonalityRating(PersonalityNodeDefOfGR.Optimistic);
            interpretChance *= 0.5f + Mathf.Abs(1 - PsycheHelper.Comp(initiator).Psyche
                .GetPersonalityRating(PersonalityNodeDefOf.Empathetic));
            interpretChance = Mathf.InverseLerp(0.50f, 2f, interpretChance);
        }
        else
        {
            interpretChance = 0.5f;
        }

        if (Rand.Value < interpretChance)
        {
            yetMoreSentencePacks.Add(RulePackDefOfGR.Sentence_InterpretFlirtPositively);
            initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOfGR.RomanticTension, recipient);
        }
        else
        {
            initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOfGR.RomanticDisinterest, recipient);
        }
    }
}