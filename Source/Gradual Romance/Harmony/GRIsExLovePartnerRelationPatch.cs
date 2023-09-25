using HarmonyLib;
using RimWorld;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.IsExLovePartnerRelation))]
public static class GRIsExLovePartnerRelationPatch
{
    [HarmonyPostfix]
    public static void GRIsExLovePartnerRelation(ref bool __result, PawnRelationDef relation)
    {
        if (__result)
        {
            return;
        }

        if (relation == PawnRelationDefOfGR.ExLovefriend)
        {
            __result = true;
        }
    }
}