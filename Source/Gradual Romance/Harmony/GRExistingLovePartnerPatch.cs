using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.ExistingLovePartner))]
public static class GRExistingLovePartnerPatch
{
    [HarmonyPostfix]
    public static void GRExistingLovePartner(ref Pawn __result, Pawn pawn)
    {
        if (__result != null)
        {
            return;
        }

        var firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOfGR.Lovefriend);
        if (firstDirectRelationPawn != null)
        {
            __result = firstDirectRelationPawn;
        }
    }
}