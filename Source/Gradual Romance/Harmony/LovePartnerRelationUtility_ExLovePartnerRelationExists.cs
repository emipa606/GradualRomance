using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.ExLovePartnerRelationExists))]
public static class LovePartnerRelationUtility_ExLovePartnerRelationExists
{
    public static void Postfix(ref bool __result, Pawn first, Pawn second)
    {
        if (!__result)
        {
            __result = first.relations.DirectRelationExists(PawnRelationDefOfGR.ExLovefriend, second);
        }
    }
}