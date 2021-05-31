using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public static class GRThoughtUtility
    {
        public static bool IsTensionMemory(ThoughtDef thought)
        {
            return thought == ThoughtDefOfGR.SexualTension || thought == ThoughtDefOfGR.RomanticTension ||
                   thought == ThoughtDefOfGR.LogicalTension;
        }

        public static int NumOfMemoriesOfDefWhereOtherPawnIs(Pawn pawn, Pawn other, ThoughtDef thought)
        {
            var memories = pawn.needs.mood.thoughts.memories.Memories;
            var count = 0;
            foreach (var thoughtMemory in memories)
            {
                if (thoughtMemory.def == thought && thoughtMemory.otherPawn == other)
                {
                    count++;
                }
            }

            return count;
        }
    }
}