using System.Collections.Generic;
using System.Linq;
using Psychology;
using RimWorld;
using Verse;

namespace Gradual_Romance;

internal class GRPawnRelationUtility
{
    private static bool IsPolygamist(Pawn pawn)
    {
        return pawn.story.traits.HasTrait(TraitDefOfPsychology.Polygamous) || GradualRomanceMod.polygamousWorld;
    }

    public static List<PawnRelationDef> ListOfBreakupRelationships()
    {
        var list = new List<PawnRelationDef>
        {
            PawnRelationDefOf.Fiance, PawnRelationDefOf.Lover, PawnRelationDefOfGR.Lovefriend,
            PawnRelationDefOfGR.Paramour, PawnRelationDefOfGR.Lovebuddy
        };
        return list;
    }

    private static List<PawnRelationDef> ListOfRomanceStages()
    {
        var list = new List<PawnRelationDef>
        {
            PawnRelationDefOf.Spouse, PawnRelationDefOf.Fiance, PawnRelationDefOf.Lover,
            PawnRelationDefOfGR.Lovefriend, PawnRelationDefOfGR.Paramour, PawnRelationDefOfGR.Sweetheart,
            PawnRelationDefOfGR.Lovebuddy
        };
        return list;
    }

    private static List<PawnRelationDef> ListOfFormalRelationships()
    {
        var list = new List<PawnRelationDef>
        {
            PawnRelationDefOf.Spouse, PawnRelationDefOf.Fiance, PawnRelationDefOf.Lover,
            PawnRelationDefOfGR.Lovefriend
        };
        return list;
    }

    private static List<PawnRelationDef> ListOfRomanceAndExStages()
    {
        var list = new List<PawnRelationDef>
        {
            PawnRelationDefOf.Spouse, PawnRelationDefOf.ExSpouse, PawnRelationDefOf.Fiance, PawnRelationDefOf.Lover,
            PawnRelationDefOf.ExLover, PawnRelationDefOfGR.Lovefriend, PawnRelationDefOfGR.ExLovefriend,
            PawnRelationDefOfGR.Paramour, PawnRelationDefOfGR.Sweetheart, PawnRelationDefOfGR.Lovebuddy
        };
        return list;
    }

    public static Pawn PawnWithMostAdvancedRelationship(Pawn pawn, out PawnRelationDef relation,
        bool mostLikedLover = true)
    {
        var listOfRomanceStages = ListOfRomanceStages();
        Pawn lover = null;
        relation = null;
        for (var i = 0; i < listOfRomanceStages.Count; i++)
        {
            relation = listOfRomanceStages[i];
            var lovers = GetAllPawnsWithGivenRelationshipTo(pawn, listOfRomanceStages[i]);
            if (lovers.Count <= 0)
            {
                continue;
            }

            for (var i2 = 0; i < lovers.Count; i++)
            {
                if (lover == null)
                {
                    lover = lovers[i2];
                }
                else if (mostLikedLover == false &&
                         lover.relations.OpinionOf(pawn) < lovers[i2].relations.OpinionOf(pawn))
                {
                    lover = lovers[i2];
                }
                else if (mostLikedLover &&
                         pawn.relations.OpinionOf(lover) < pawn.relations.OpinionOf(lovers[i2]))
                {
                    lover = lovers[i2];
                }
            }

            break;
        }

        return lover;
    }

    //returns the most logical romance stage for the given romance stage.
    public static PawnRelationDef NextRomanceStage(PawnRelationDef currentStage)
    {
        if (currentStage == PawnRelationDefOf.Fiance)
        {
            return PawnRelationDefOf.Spouse;
        }

        if (currentStage == PawnRelationDefOf.Lover)
        {
            return PawnRelationDefOf.Fiance;
        }

        if (currentStage == PawnRelationDefOfGR.Lovefriend)
        {
            return PawnRelationDefOf.Lover;
        }

        if (currentStage == PawnRelationDefOfGR.Sweetheart)
        {
            return PawnRelationDefOfGR.Lovefriend;
        }

        return currentStage == PawnRelationDefOfGR.Lovebuddy ? PawnRelationDefOfGR.Lovefriend : null;
    }

    public static Pawn MostAdvancedRelationship(Pawn pawn)
    {
        var listOfRomanceStages = ListOfRomanceStages();
        foreach (var pawnRelationDef in listOfRomanceStages)
        {
            var lover = pawn.relations.GetFirstDirectRelationPawn(pawnRelationDef);
            if (lover != null)
            {
                return pawn.relations.GetFirstDirectRelationPawn(pawnRelationDef);
            }
        }

        return null;
    }

    private static PawnRelationDef MostAdvancedRelationshipBetween(Pawn pawn, Pawn other)
    {
        var listOfRomanceStages = ListOfRomanceStages();
        foreach (var relation in listOfRomanceStages)
        {
            if (pawn.relations.DirectRelationExists(relation, other))
            {
                return relation;
            }
        }

        return null;
    }

    public static bool IsAnAffair(Pawn pawn, Pawn other, out Pawn pawnSO, out Pawn otherSO)
    {
        pawnSO = null;
        otherSO = null;
        if (GradualRomanceMod.polygamousWorld)
        {
            return false;
        }

        var listOfFormalRelationships = ListOfFormalRelationships();
        for (var i = 0; i < listOfFormalRelationships.Count; i++)
        {
            if (!CaresAboutCheating(listOfFormalRelationships[i]))
            {
                continue;
            }

            var pawns = GetAllPawnsWithGivenRelationshipTo(pawn, listOfFormalRelationships[i]);
            foreach (var unused in pawns)
            {
                var SO = pawns[i];
                if (SO.Dead || IsPolygamist(SO) || SO.IsColonist)
                {
                    continue;
                }

                pawnSO = SO;
                break;
            }

            if (pawnSO != null)
            {
                break;
            }
        }

        for (var i = 0; i < listOfFormalRelationships.Count; i++)
        {
            var pawns = GetAllPawnsWithGivenRelationshipTo(other, listOfFormalRelationships[i]);
            foreach (var unused in pawns)
            {
                var SO = pawns[i];
                if (SO.Dead || IsPolygamist(SO) || SO.IsColonist)
                {
                    continue;
                }

                otherSO = SO;
                break;
            }

            if (otherSO != null)
            {
                break;
            }
        }

        return pawnSO != null || otherSO != null;
    }

    private static PawnRelationDef CurrentRomanceStage(Pawn pawn, Pawn other)
    {
        var listOfRomanceStages = ListOfRomanceStages();
        foreach (var currentRomanceStage in listOfRomanceStages)
        {
            if (pawn.relations.GetDirectRelation(currentRomanceStage, other) != null)
            {
                return currentRomanceStage;
            }
        }

        return null;
    }

    public static PawnRelationDef MostAdvancedRomanceOrExStage(Pawn pawn, Pawn other)
    {
        var listOfRomanceStages = ListOfRomanceAndExStages();
        foreach (var mostAdvancedRomanceOrExStage in listOfRomanceStages)
        {
            if (pawn.relations.GetDirectRelation(mostAdvancedRomanceOrExStage, other) != null)
            {
                return mostAdvancedRomanceOrExStage;
            }
        }

        return null;
    }

    public static float AffairReluctance(PawnRelationDef currentStage)
    {
        try
        {
            return currentStage.GetModExtension<RomanticRelationExtension>().baseAffairReluctance;
        }
        catch
        {
            return 1f;
        }
    }

    public static bool IsRomanticOrSexualRelationship(PawnRelationDef pawnRelation)
    {
        try
        {
            return pawnRelation.GetModExtension<RomanticRelationExtension>().goesOnDates ||
                   pawnRelation.GetModExtension<RomanticRelationExtension>().doesLovin;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsRomanticRelationship(PawnRelationDef pawnRelation)
    {
        try
        {
            return pawnRelation.GetModExtension<RomanticRelationExtension>().goesOnDates;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsSexualRelationship(PawnRelationDef pawnRelation)
    {
        try
        {
            return pawnRelation.GetModExtension<RomanticRelationExtension>().doesLovin;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsInformalRelationship(PawnRelationDef pawnRelation)
    {
        try
        {
            return !pawnRelation.GetModExtension<RomanticRelationExtension>().isFormalRelationship;
        }
        catch
        {
            return false;
        }
    }

    public static bool HasInformalRelationship(Pawn pawn, Pawn other)
    {
        var relation = CurrentRomanceStage(pawn, other);
        if (relation == null)
        {
            return false;
        }

        try
        {
            return !relation.GetModExtension<RomanticRelationExtension>().isFormalRelationship;
        }
        catch
        {
            return false;
        }
    }

    private static bool CaresAboutCheating(PawnRelationDef pawnRelation)
    {
        try
        {
            return pawnRelation.GetModExtension<RomanticRelationExtension>().caresAboutCheating;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsBedSharingRelationship(PawnRelationDef pawnRelation)
    {
        try
        {
            return pawnRelation.GetModExtension<RomanticRelationExtension>().sharesBed;
        }
        catch
        {
            return false;
        }
    }

    private static List<Pawn> GetAllPawnsWithGivenRelationshipTo(Pawn pawn, PawnRelationDef relation)
    {
        var pawnList = new List<Pawn>();
        var directRelations = pawn.relations.DirectRelations;
        for (var i = 0; i > directRelations.Count; i++)
        {
            var thisRelation = directRelations[i];
            if (thisRelation.def == relation)
            {
                pawnList.Add(thisRelation.otherPawn);
            }
        }

        return pawnList;
    }

    public static List<Pawn> GetAllPawnsRomanticWith(Pawn pawn)
    {
        var relationsList = ListOfRomanceStages();
        var loversList = new List<Pawn>();
        for (var i = 0; i < relationsList.Count; i++)
        {
            var newLovers = GetAllPawnsWithGivenRelationshipTo(pawn, relationsList[i]);
            for (var i2 = 0; i2 < newLovers.Count; i++)
            {
                loversList.Add(newLovers[i2]);
            }
        }

        return loversList;
    }

    public static bool ShouldShareBed(Pawn pawn, Pawn other)
    {
        var relation = CurrentRomanceStage(pawn, other);
        return IsBedSharingRelationship(relation);
    }

    private static DirectPawnRelation MostLikedBedSharingRelationship(Pawn pawn, bool allowDead)
    {
        if (!pawn.RaceProps.IsFlesh)
        {
            return null;
        }

        DirectPawnRelation directPawnRelation = null;
        var num = int.MinValue;
        var directRelations = pawn.relations.DirectRelations;
        foreach (var pawnRelation in directRelations)
        {
            if (!allowDead && pawnRelation.otherPawn.Dead)
            {
                continue;
            }

            if (!IsBedSharingRelationship(pawnRelation.def))
            {
                continue;
            }

            var num2 = pawn.relations.OpinionOf(pawnRelation.otherPawn);
            if (directPawnRelation != null && num2 <= num)
            {
                continue;
            }

            directPawnRelation = pawnRelation;
            num = num2;
        }

        return directPawnRelation;
    }

    public static Pawn MostLikedBedSharingPawn(Pawn pawn, bool allowDead)
    {
        var directPawnRelation = MostLikedBedSharingRelationship(pawn, allowDead);
        return directPawnRelation?.otherPawn;
    }

    public static void AdvanceRelationship(Pawn pawn, Pawn other, PawnRelationDef newRelation)
    {
        var oldRelation = MostAdvancedRelationshipBetween(pawn, other);
        if (pawn.relations.DirectRelationExists(newRelation, other))
        {
            return;
        }

        pawn.relations.AddDirectRelation(newRelation, other);
        if (oldRelation != null)
        {
            pawn.relations.TryRemoveDirectRelation(oldRelation, other);
        }
    }

    //TODO Should add more sophisticated behavior
    public static int NumberOfFriends(Pawn pawn)
    {
        if (pawn.MapHeld == null || !pawn.IsColonist)
        {
            return -1;
        }

        IEnumerable<Pawn> allPawns = pawn.MapHeld.mapPawns.FreeColonists;
        var numOfFriends = (from friend in allPawns
            where friend != pawn && friend.IsColonist && !friend.Dead && friend.relations.OpinionOf(pawn) >=
                Pawn_RelationsTracker.FriendOpinionThreshold
            select friend).Count();
        return numOfFriends;
    }

    private static bool RelationshipCanEvolveTo(Pawn pawn, Pawn other, PawnRelationDef newRelation)
    {
        List<ThoughtCondition> conditions;
        try
        {
            conditions = newRelation.GetModExtension<RomanticRelationExtension>().conditions;
        }
        catch
        {
            return false;
        }

        if (conditions.NullOrEmpty())
        {
            return false;
        }

        if (!AttractionUtility.IsAgeAppropriate(pawn) || !AttractionUtility.IsAgeAppropriate(other))
        {
            try
            {
                if (newRelation.GetModExtension<RomanticRelationExtension>().doesLovin)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        foreach (var thoughtCondition in conditions)
        {
            Log.Message(
                $"{thoughtCondition.thought.defName}: {pawn.needs.mood.thoughts.memories.NumMemoriesOfDef(thoughtCondition.thought)} {other.needs.mood.thoughts.memories.NumMemoriesOfDef(thoughtCondition.thought)} needs {thoughtCondition.numberRequired}");

            if (GRThoughtUtility.NumOfMemoriesOfDefWhereOtherPawnIs(pawn, other, thoughtCondition.thought) <
                thoughtCondition.numberRequired ||
                GRThoughtUtility.NumOfMemoriesOfDefWhereOtherPawnIs(other, pawn, thoughtCondition.thought) <
                thoughtCondition.numberRequired)
            {
                return false;
            }
        }

        return true;
    }

    private static int GetRelationLevel(PawnRelationDef relation)
    {
        try
        {
            return relation.GetModExtension<RomanticRelationExtension>().relationshipLevel;
        }
        catch
        {
            return -1;
        }
    }

    public static void AdvanceInformalRelationship(Pawn pawn, Pawn other, out PawnRelationDef newRelation,
        float sweetheartChance = 0.5f)
    {
        var oldRelation = MostAdvancedRelationshipBetween(pawn, other);
        //if (!IsInformalRelationship(oldRelation))
        newRelation = null;
        var targetLevel = 1;
        if (oldRelation != null && IsInformalRelationship(oldRelation))
        {
            targetLevel = oldRelation.GetModExtension<RomanticRelationExtension>().relationshipLevel + 1;
        }

        var candidateRelations = from relation in DefDatabase<PawnRelationDef>.AllDefsListForReading
            where GetRelationLevel(relation) == targetLevel
            select relation;
        if (!candidateRelations.Any())
        {
            return;
        }

        foreach (var relation in candidateRelations)
        {
            Log.Message($"Testing {relation.defName}");
            if (!RelationshipCanEvolveTo(pawn, other, relation))
            {
                continue;
            }

            if (oldRelation != null)
            {
                pawn.relations.TryRemoveDirectRelation(oldRelation, other);
            }

            pawn.relations.AddDirectRelation(relation, other);
            newRelation = relation;
            break;
        }
    }

    public static int LevelOfSexualTension(Pawn pawn, Pawn other)
    {
        return GRThoughtUtility.NumOfMemoriesOfDefWhereOtherPawnIs(pawn, other, ThoughtDefOfGR.SexualTension);
    }
}