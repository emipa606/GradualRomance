using RimWorld;
using Verse;

namespace Gradual_Romance;

public static class ThoughtDetector
{
    public static bool HasSituationalThought(Pawn pawn, ThoughtDef thought)
    {
        if (!ThoughtUtility.CanGetThought(pawn, thought))
        {
            return false;
        }

        var thoughtState = thought.Worker.CurrentState(pawn);
        return thoughtState.Active;
    }

    public static bool HasSocialSituationalThought(Pawn pawn, Pawn other, ThoughtDef thought)
    {
        if (!ThoughtUtility.CanGetThought(pawn, thought))
        {
            return false;
        }

        var thoughtState = thought.Worker.CurrentSocialState(pawn, other);
        return thoughtState.Active;
    }
}