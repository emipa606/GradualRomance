using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony
{
    [HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.LovePartnerRelationExists))]
    public static class GRLovePartnerRelationExistsPatch
    {
        [HarmonyPostfix]
        public static void GRLovePartnerRelationExists(ref bool __result, Pawn first, Pawn second)
        {
            if (__result != true)
            {
                __result = first.relations.DirectRelationExists(PawnRelationDefOfGR.Lovefriend, second);
            }
        }
    }
}