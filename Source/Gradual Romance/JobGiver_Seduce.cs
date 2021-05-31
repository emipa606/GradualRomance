using Verse;
using Verse.AI;

namespace Gradual_Romance
{
    internal class JobGiver_Seduce : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.mindState.canLovinTick > Find.TickManager.TicksGame)
            {
                return null;
            }

            return null;
        }
    }
}