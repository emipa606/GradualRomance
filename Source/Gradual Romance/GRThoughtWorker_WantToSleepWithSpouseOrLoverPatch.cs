using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony
{
    //This forces the want to sleep with spouse thought to be inactive.
    [HarmonyPatch(typeof(ThoughtWorker_WantToSleepWithSpouseOrLover), "CurrentStateInternal")]
    public class GRThoughtWorker_WantToSleepWithSpouseOrLoverPatch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Low)]
        public static void GRNewThoughtState(ref ThoughtState __result, Pawn p)
        {
            __result = ThoughtState.Inactive;
        }
    }
}