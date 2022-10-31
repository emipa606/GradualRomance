using System.Collections.Generic;
using Verse;

namespace Gradual_Romance;

public class FlirtReactionWorker
{
    public FlirtReactionDef reaction = new FlirtReactionDef();

    public virtual void GiveThoughts(Pawn initiator, Pawn recipient, out List<RulePackDef> yetMoreSentencePacks)
    {
        yetMoreSentencePacks = new List<RulePackDef>();
        if (reaction.successful)
        {
            var thoughtToGive = reaction.givesTension.RandomElement();
            if (thoughtToGive == null)
            {
                return;
            }

            initiator.needs.mood.thoughts.memories.TryGainMemory(thoughtToGive, recipient);
            recipient.needs.mood.thoughts.memories.TryGainMemory(thoughtToGive, initiator);
        }
        else
        {
            initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOfGR.RomanticDisinterest, recipient);
        }
    }
}