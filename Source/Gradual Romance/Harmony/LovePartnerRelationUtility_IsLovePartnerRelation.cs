using HarmonyLib;
using RimWorld;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.IsLovePartnerRelation))]
public static class LovePartnerRelationUtility_IsLovePartnerRelation
{
    public static void Postfix(ref bool __result, PawnRelationDef relation)
    {
        if (__result)
        {
            return;
        }

        if (relation == PawnRelationDefOfGR.Lovefriend)
        {
            __result = true;
        }
    }
}