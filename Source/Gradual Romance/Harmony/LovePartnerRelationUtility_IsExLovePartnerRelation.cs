using HarmonyLib;
using RimWorld;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.IsExLovePartnerRelation))]
public static class LovePartnerRelationUtility_IsExLovePartnerRelation
{
    public static void Postfix(ref bool __result, PawnRelationDef relation)
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