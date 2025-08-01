using HarmonyLib;
using RimWorld;

namespace Gradual_Romance.Harmony;

//This forces the want to sleep with spouse thought to be inactive.
[HarmonyPatch(typeof(ThoughtWorker_WantToSleepWithSpouseOrLover), "CurrentStateInternal")]
public class ThoughtWorker_WantToSleepWithSpouseOrLover_CurrentStateInternal
{
    [HarmonyPriority(Priority.Low)]
    public static void Postfix(ref ThoughtState __result)
    {
        __result = ThoughtState.Inactive;
    }
}