using HarmonyLib;
using RimWorld;

namespace Gradual_Romance.Harmony;

//This forces the want to sleep with spouse thought to be inactive.
[HarmonyPatch(typeof(ThoughtWorker_WantToSleepWithSpouseOrLover), "CurrentStateInternal")]
public class GRThoughtWorker_WantToSleepWithSpouseOrLoverPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(Priority.Low)]
    public static void GRNewThoughtState(ref ThoughtState __result)
    {
        __result = ThoughtState.Inactive;
    }
}