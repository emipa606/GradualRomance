using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility), nameof(LovePartnerRelationUtility.HasAnyLovePartner))]
public static class LovePartnerRelationUtility_HasAnyLovePartner
{
    public static void Postfix(ref bool __result, Pawn pawn)
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