using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.LovePartnerRelationExists))]
public static class LovePartnerRelationUtility_LovePartnerRelationExists
{
    public static void Postfix(ref bool __result, Pawn first, Pawn second)
    {
        if (!__result)
        {
            __result = first.relations.DirectRelationExists(PawnRelationDefOfGR.Lovefriend, second);
        }
    }
}