using HarmonyLib;
using RimWorld;
using Verse;

namespace Gradual_Romance.Harmony;

[HarmonyPatch(typeof(LovePartnerRelationUtility),
    nameof(LovePartnerRelationUtility.GiveRandomExLoverOrExSpouseRelation))]
public static class LovePartnerRelationUtility_GiveRandomExLoverOrExSpouseRelation
{
    public static bool Prefix(Pawn first, Pawn second)
    {
        PawnRelationDef def;
        var value = Rand.Value;
        if (value < 0.33)
        {
            def = PawnRelationDefOfGR.ExLovefriend;
        }
        else if (value < 0.66)
        {
            def = PawnRelationDefOf.ExLover;
        }
        else
        {
            def = PawnRelationDefOf.ExSpouse;
        }

        first.relations.AddDirectRelation(def, second);
        return false;
    }
}