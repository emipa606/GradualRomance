using System.Collections.Generic;
using System.Linq;
using System.Text;
using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance;

public class InteractionWorker_GRBreakup : InteractionWorker
{
    // Plundered and adapted from Psychology
    public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
    {
        if (!RelationshipUtility.HasInformalRelationship(initiator, recipient) &&
            !LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
        {
            return 0f;
        }

        if (initiator.story.traits.HasTrait(TraitDefOfPsychology.Codependent))
        {
            return 0f;
        }

        var chance = 0.02f * GradualRomanceMod.BaseBreakupChance;
        var romanticFactor = 1f;
        if (PsycheHelper.PsychologyEnabled(initiator))
        {
            chance = 0.05f * GradualRomanceMod.BaseBreakupChance;
            romanticFactor = Mathf.InverseLerp(1.05f, 0f,
                PsycheHelper.Comp(initiator).Psyche.GetPersonalityRating(PersonalityNodeDefOf.Romantic));
        }

        var opinionFactor = Mathf.InverseLerp(100f, -100f, initiator.relations.OpinionOf(recipient));
        var relation = RelationshipUtility.MostAdvancedRelationshipBetween(initiator, recipient);
        var spouseFactor = relation.GetModExtension<RomanticRelationExtension>().baseAffairReluctance;
        var justificationFactor = 0.75f;
        if (BreakupUtility.HasReasonForBreakup(initiator, recipient))
        {
            justificationFactor = 2f;
        }

        return chance * romanticFactor * opinionFactor * spouseFactor * justificationFactor;
    }

    private Thought RandomBreakupReason(Pawn initiator, Pawn recipient)
    {
        var list = (from m in initiator.needs.mood.thoughts.memories.Memories
            where m != null && m.otherPawn == recipient && m.CurStage is { baseOpinionOffset: < 0f }
            select m).ToList();
        if (list.Count == 0)
        {
            return null;
        }

        var worstMemoryOpinionOffset = list.Max(m => -m.CurStage.baseOpinionOffset);
        (from m in list
            where -m.CurStage.baseOpinionOffset >= worstMemoryOpinionOffset / 2f
            select m).TryRandomElementByWeight(m => -m.CurStage.baseOpinionOffset, out var result);
        return result;
    }

    public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks,
        out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
    {
        lookTargets = null;
        var thought = RandomBreakupReason(initiator, recipient);
        var relation = RelationshipUtility.MostAdvancedRelationshipBetween(initiator, recipient);

        if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Spouse, recipient))
        {
            BreakupUtility.RelationToEx(initiator, recipient, PawnRelationDefOf.Spouse);
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DivorcedMe, initiator);
            recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOfPsychology.BrokeUpWithMeCodependent,
                initiator);
            initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
            recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
            initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase,
                recipient);
            recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase,
                initiator);
        }
        else
        {
            BreakupUtility.ResolveBreakup(initiator, recipient,
                RelationshipUtility.MostAdvancedRelationshipBetween(initiator, recipient));
        }

        //Idea - who gets the bedroom? Could be interesting.
        if (initiator.ownership.OwnedBed != null && initiator.ownership.OwnedBed == recipient.ownership.OwnedBed)
        {
            var pawn = Rand.Value >= 0.5f ? recipient : initiator;
            pawn.ownership.UnclaimBed();
        }

        TaleRecorder.RecordTale(TaleDefOf.Breakup, initiator, recipient);
        var stringBuilder = new StringBuilder();
        if (RelationshipUtility.IsInformalRelationship(relation))
        {
            stringBuilder.AppendLine(
                "LetterInformalRelationsEnds".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2")));
            letterDef = LetterDefOf.NeutralEvent;
            letterLabel = "LetterLabelInformalRelationsEnds".Translate();
        }
        else
        {
            stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator.LabelShort, recipient.LabelShort,
                initiator.Named("PAWN1"), recipient.Named("PAWN2")));
            letterDef = LetterDefOf.NegativeEvent;
            letterLabel = "LetterLabelBreakup".Translate();
        }

        if (thought != null)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("FinalStraw".Translate(thought.CurStage.label.CapitalizeFirst()));
        }

        if (PawnUtility.ShouldSendNotificationAbout(initiator) ||
            PawnUtility.ShouldSendNotificationAbout(recipient))
        {
            letterDef = null;
            letterLabel = null;
            letterText = null;
        }
        else if (RelationshipUtility.IsInformalRelationship(relation) &&
                 GradualRomanceMod.informalRomanceLetters == false)
        {
            letterDef = null;
            letterLabel = null;
            letterText = null;
        }
        else
        {
            letterText = stringBuilder.ToString();
        }
    }
}