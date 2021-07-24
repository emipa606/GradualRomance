using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony
{
    [HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.HasAnyLovePartner))]
    public static class GRHasAnyLovePartnerPatch
    {
        [HarmonyPostfix]
        public static void GRHasAnyLovePartner(ref bool __result, Pawn pawn)
        {
            if (__result)
            {
                return;
            }

            if (pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOfGR.Lovefriend) != null)
            {
                __result = true;
            }
        }
    }


    //Let's throw out even Psychology's formula!
    //Since this is causing a lot of redundant effort, it's probably a good idea to find a way to bypass Psychology and Vanilla calculation entirely.
    //For right now, this will do.
}