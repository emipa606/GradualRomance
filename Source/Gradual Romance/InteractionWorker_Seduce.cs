using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class InteractionWorker_Seduce : InteractionWorker
{
    private const float MinAttractionForRomanceAttempt = 0.25f;

    private const int MinOpinionForRomanceAttempt = 5;

    private const float BaseSuccessChance = 1f;

    private const float BaseFlirtWeight = 0.4f;

    private const float GoodFlirtBonus = 1.5f;

    private const float BadFlirtPenalty = 0.6f;

    private const float FamiliarityFactor = 0.5f;

    private List<AttractionFactorDef> highInitiatorReasons = new List<AttractionFactorDef>();
    private float initiatorAttraction = 1f;
    private Pawn lastInitiator;
    private Pawn lastRecipient;
    private List<AttractionFactorDef> lowInitiatorReasons = new List<AttractionFactorDef>();

    private List<AttractionFactorDef> veryHighInitiatorReasons = new List<AttractionFactorDef>();
    private List<AttractionFactorDef> veryLowInitiatorReasons = new List<AttractionFactorDef>();


    private void EmptyReasons()
    {
        veryHighInitiatorReasons.Clear();
        highInitiatorReasons.Clear();
        lowInitiatorReasons.Clear();
        veryLowInitiatorReasons.Clear();
    }

    private float CalculateAndSort(AttractionFactorCategoryDef category, Pawn observer, Pawn assessed,
        bool observerIsInitiator = true)
    {
        var result = AttractionUtility.CalculateAttractionCategory(category, observer, assessed,
            out var veryLowFactors, out var lowFactors, out var hightFactors, out var veryHighFactors,
            out _);
        if (!observerIsInitiator)
        {
            return result;
        }

        veryHighInitiatorReasons.AddRange(veryHighFactors);
        highInitiatorReasons.AddRange(hightFactors);
        lowInitiatorReasons.AddRange(lowFactors);
        veryLowInitiatorReasons.AddRange(veryLowFactors);

        return result;
    }

    public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
    {
        var pawnRelation = RelationshipUtility.MostAdvancedRelationshipBetween(initiator, recipient);
        if (!AttractionUtility.QuickCheck(initiator, recipient))
        {
            return 0f;
        }

        switch (GradualRomanceMod.SeductionMode)
        {
            case GradualRomanceMod.SeductionModeSetting.NoSeduction:
            case GradualRomanceMod.SeductionModeSetting.OnlyRelationship
                when pawnRelation == null || !RelationshipUtility.IsSexualRelationship(pawnRelation):
            case GradualRomanceMod.SeductionModeSetting.RelationshipAndNonColonists
                when pawnRelation == null && recipient.IsColonist:
            case GradualRomanceMod.SeductionModeSetting.RelationshipAndNonColonists when pawnRelation != null &&
                !RelationshipUtility.IsSexualRelationship(pawnRelation) &&
                recipient.IsColonist:
                return 0f;
        }

        //shouldn't seduce if you can't move
        if (initiator.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) <= 0.5f ||
            initiator.health.capacities.GetLevel(PawnCapacityDefOf.Moving) <= 0.25f)
        {
            return 0f;
        }

        if (recipient.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) <= 0.5f ||
            recipient.health.capacities.GetLevel(PawnCapacityDefOf.Moving) <= 0.25f)
        {
            return 0f;
        }

        //shouldn't seduce while working
        var initiatorAssignment = initiator.timetable.GetAssignment(GenLocalDate.HourOfDay(initiator.Map));
        var recipientAssignment = recipient.timetable.GetAssignment(GenLocalDate.HourOfDay(recipient.Map));
        if (initiatorAssignment != TimeAssignmentDefOf.Joy || initiatorAssignment != TimeAssignmentDefOf.Anything)
        {
            return 0f;
        }

        if (recipientAssignment != TimeAssignmentDefOf.Joy || recipientAssignment != TimeAssignmentDefOf.Anything)
        {
            return 0f;
        }

        EmptyReasons();
        initiatorAttraction = AttractionUtility.CalculateAttraction(initiator, recipient, false, false,
            out veryLowInitiatorReasons, out lowInitiatorReasons, out highInitiatorReasons,
            out veryHighInitiatorReasons, out _);
        var tensionFactor = 1.33f * RelationshipUtility.LevelOfSexualTension(initiator, recipient);
        tensionFactor = Mathf.Max(1f, tensionFactor);
        lastInitiator = initiator;
        lastRecipient = recipient;
        return GradualRomanceMod.BaseSeductionChance * initiatorAttraction * tensionFactor *
               AttractionUtility.PropensityToSeduce(initiator);
    }

    public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks,
        out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
    {
        if (lastInitiator != initiator || lastRecipient != recipient)
        {
            EmptyReasons();
            initiatorAttraction = AttractionUtility.CalculateAttraction(initiator, recipient, false, false,
                out veryLowInitiatorReasons, out lowInitiatorReasons, out highInitiatorReasons,
                out veryHighInitiatorReasons, out _);
        }


        letterText = null;
        letterLabel = null;
        letterDef = null;
        lookTargets = null;
    }


    private static void LogFlirt(string message)
    {
        if (GradualRomanceMod.detailedDebugLogs)
        {
            Log.Message(message);
        }
    }
}