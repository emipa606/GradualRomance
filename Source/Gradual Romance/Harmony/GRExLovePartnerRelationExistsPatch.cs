using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.ExLovePartnerRelationExists))]
public static class GRExLovePartnerRelationExistsPatch
{
    [HarmonyPostfix]
    public static void GRExLovePartnerRelationExists(ref bool __result, Pawn first, Pawn second)
    {
        if (__result != true)
        {
            __result = first.relations.DirectRelationExists(PawnRelationDefOfGR.ExLovefriend, second);
        }
    }
}