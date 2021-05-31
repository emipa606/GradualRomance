using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class InteractionWorker_Flirt : InteractionWorker
    {
        //List<InteractionDef> allDefsListForReading = DefDatabase<InteractionDef>.AllDefsListForReading;
        //allDefsListForReading.TryRandomElementByWeight((InteractionDef x) => x.Worker.RandomSelectionWeight(this.pawn, p), out intDef)
        private const float MinAttractionForRomanceAttempt = 0.25f;

        private const int MinOpinionForRomanceAttempt = 5;

        private const float BaseSuccessChance = 1f;

        private const float BaseFlirtWeight = 0.4f;

        private const float GoodFlirtBonus = 1.5f;

        private const float BadFlirtPenalty = 0.6f;

        private const float FamiliarityFactor = 0.5f;

        private static float pressureCache;
        private readonly List<AttractionFactorDef> highRecipientReasons = new List<AttractionFactorDef>();
        private readonly List<AttractionFactorDef> lowRecipientReasons = new List<AttractionFactorDef>();
        private readonly List<AttractionFactorDef> veryHighRecipientReasons = new List<AttractionFactorDef>();
        private readonly List<AttractionFactorDef> veryLowRecipientReasons = new List<AttractionFactorDef>();
        private List<AttractionFactorDef> highInitiatorReasons = new List<AttractionFactorDef>();
        private float initiatorPhysicalAttraction;
        private float initiatorRomanticAttraction;
        private float initiatorSocialAttraction;
        private Pawn lastInitiator;
        private Pawn lastRecipient;
        private List<AttractionFactorDef> lowInitiatorReasons = new List<AttractionFactorDef>();
        private float recipientCircumstances;
        private float recipientPhysicalAttraction;
        private float recipientRomanticAttraction;
        private float recipientSocialAttraction;
        private bool successImpossible;
        private List<AttractionFactorDef> veryHighInitiatorReasons = new List<AttractionFactorDef>();
        private List<AttractionFactorDef> veryLowInitiatorReasons = new List<AttractionFactorDef>();


        private void EmptyReasons()
        {
            veryHighInitiatorReasons.Clear();
            highInitiatorReasons.Clear();
            lowInitiatorReasons.Clear();
            veryLowInitiatorReasons.Clear();
            veryHighRecipientReasons.Clear();
            highRecipientReasons.Clear();
            lowRecipientReasons.Clear();
            veryLowRecipientReasons.Clear();
        }

        private float CalculateAndSort(AttractionFactorCategoryDef category, Pawn observer, Pawn assessed,
            bool observerIsInitiator = true)
        {
            var result = AttractionUtility.CalculateAttractionCategory(category, observer, assessed,
                out var veryLowFactors, out var lowFactors, out var hightFactors, out var veryHighFactors,
                out _);
            if (observerIsInitiator)
            {
                veryHighInitiatorReasons.AddRange(veryHighFactors);
                highInitiatorReasons.AddRange(hightFactors);
                lowInitiatorReasons.AddRange(lowFactors);
                veryLowInitiatorReasons.AddRange(veryLowFactors);
            }
            else
            {
                veryHighRecipientReasons.AddRange(veryHighFactors);
                highRecipientReasons.AddRange(hightFactors);
                lowRecipientReasons.AddRange(lowFactors);
                veryLowRecipientReasons.AddRange(veryLowFactors);
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="initiator"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (!AttractionUtility.QuickCheck(initiator, recipient))
            {
                return 0f;
            }

            EmptyReasons();
            var currentAttraction = AttractionUtility.CalculateAttraction(initiator, recipient, false, false,
                out veryLowInitiatorReasons, out lowInitiatorReasons, out highInitiatorReasons,
                out veryHighInitiatorReasons, out _);
            recipientPhysicalAttraction = GRHelper.GRPawnComp(initiator)
                .RetrieveAttractionForCategory(recipient, AttractionFactorCategoryDefOf.Physical);
            recipientRomanticAttraction = GRHelper.GRPawnComp(initiator)
                .RetrieveAttractionForCategory(recipient, AttractionFactorCategoryDefOf.Romantic);
            recipientSocialAttraction = GRHelper.GRPawnComp(initiator)
                .RetrieveAttractionForCategory(recipient, AttractionFactorCategoryDefOf.Social);

            //initiatorCircumstances = CalculateAndSort(AttractionFactorCategoryDefOf.Circumstance, initiator, recipient);
            //if (intiatorFailureReasons.Count() > 0)
            //{
            //    EmptyReasons();
            //    return 0f;
            //}
            var flirtFactor = 0.5f;

            var memoryList = initiator.needs.mood.thoughts.memories.Memories;

            foreach (var curMemory in memoryList)
            {
                if (curMemory.def == ThoughtDefOfGR.RomanticDisinterest && curMemory.otherPawn == recipient)
                {
                    flirtFactor = flirtFactor * BadFlirtPenalty;
                }
            }

            flirtFactor = Mathf.Max(flirtFactor, 0.05f);
            lastInitiator = initiator;
            lastRecipient = recipient;
            return GradualRomanceMod.BaseFlirtChance * currentAttraction * flirtFactor * BaseFlirtWeight;
        }
        /*
        public float SuccessChance(Pawn initiator, Pawn recipient)
        {
            float recipientAttraction = recipient.relations.SecondaryRomanceChanceFactor(initiator);
            return 1f;
        }
        */
        //
        //allDefsListForReading.TryRandomElementByWeight((InteractionDef x) => x.Worker.RandomSelectionWeight(this.pawn, p), out intDef)

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks,
            out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            lookTargets = null;
            if (lastInitiator != initiator || lastRecipient != recipient)
            {
                EmptyReasons();
                recipientPhysicalAttraction =
                    CalculateAndSort(AttractionFactorCategoryDefOf.Physical, initiator, recipient);
                recipientRomanticAttraction =
                    CalculateAndSort(AttractionFactorCategoryDefOf.Romantic, initiator, recipient);
                recipientSocialAttraction =
                    CalculateAndSort(AttractionFactorCategoryDefOf.Social, initiator, recipient);
            }

            /*
            initiatorPhysicalAttraction = CalculateAndSort(AttractionFactorCategoryDefOf.Physical, recipient, initiator, false);
            initiatorRomanticAttraction = CalculateAndSort(AttractionFactorCategoryDefOf.Romantic, recipient, initiator, false);
            initiatorSocialAttraction = CalculateAndSort(AttractionFactorCategoryDefOf.Social, recipient, initiator, false);
            recipientCircumstances = CalculateAndSort(AttractionFactorCategoryDefOf.Circumstance, recipient, initiator, false);
            */
            recipientCircumstances =
                AttractionUtility.CalculateAttractionCategory(AttractionFactorCategoryDefOf.Circumstance, recipient,
                    initiator);
            initiatorPhysicalAttraction = GRHelper.GRPawnComp(recipient)
                .RetrieveAttractionForCategory(initiator, AttractionFactorCategoryDefOf.Physical);
            initiatorRomanticAttraction = GRHelper.GRPawnComp(recipient)
                .RetrieveAttractionForCategory(initiator, AttractionFactorCategoryDefOf.Romantic);
            initiatorSocialAttraction = GRHelper.GRPawnComp(recipient)
                .RetrieveAttractionForCategory(initiator, AttractionFactorCategoryDefOf.Social);
            LogFlirt(initiator.Name.ToStringShort + "=>" + recipient.Name.ToStringShort + " attraction: physical " +
                     recipientPhysicalAttraction + ", romantic " + recipientRomanticAttraction + ", social " +
                     recipientSocialAttraction + ".");
            var allDefsListForReading = DefDatabase<FlirtStyleDef>.AllDefsListForReading;
            pressureCache = AttractionUtility.RelationshipStress(initiator, recipient);
            allDefsListForReading.TryRandomElementByWeight(x => CalculateFlirtStyleWeight(x, initiator, recipient),
                out var flirtStyle);
            if (flirtStyle == null)
            {
                Log.Error("FailedToFindFlirt_error".Translate());
                letterText = null;
                letterLabel = null;
                letterDef = null;
                return;
            }

            if (veryHighInitiatorReasons.Count > 0)
            {
                var reason = veryHighInitiatorReasons.RandomElement();
                extraSentencePacks.Add(reason.intriguedByText);
            }
            else if (highInitiatorReasons.Count > 0)
            {
                var reason = highInitiatorReasons.RandomElement();
                extraSentencePacks.Add(reason.intriguedByText);
            }

            if (recipient.gender == Gender.Male)
            {
                extraSentencePacks.Add(flirtStyle.rulePackMale);
            }

            if (recipient.gender == Gender.Female)
            {
                extraSentencePacks.Add(flirtStyle.rulePackFemale);
            }

            LogFlirt("Flirt chosen: " + flirtStyle.defName + ".");
            LogFlirt(recipient.Name.ToStringShort + "=>" + initiator.Name.ToStringShort + " attraction: physical " +
                     initiatorPhysicalAttraction + ", romantic " + initiatorRomanticAttraction + ", social " +
                     initiatorSocialAttraction + ".");

            if (initiatorPhysicalAttraction == 0f || initiatorRomanticAttraction == 0f ||
                initiatorSocialAttraction == 0f)
            {
                successImpossible = true;
            }
            else
            {
                successImpossible = false;
            }

            FlirtReactionDef flirtReaction;
            var successfulFlirtReactions = from reaction in DefDatabase<FlirtReactionDef>.AllDefsListForReading
                where reaction.successful
                select reaction;
            var unsuccessfulFlirtReactions = from reaction in DefDatabase<FlirtReactionDef>.AllDefsListForReading
                where !reaction.successful
                select reaction;
            successfulFlirtReactions.TryRandomElementByWeight(
                x => CalculateFlirtReactionWeight(flirtStyle, x, initiator, recipient), out var successfulFlirt);
            unsuccessfulFlirtReactions.TryRandomElementByWeight(
                x => CalculateFlirtReactionWeight(flirtStyle, x, initiator, recipient), out var unsuccessfulFlirt);
            if (successImpossible)
            {
                flirtReaction = unsuccessfulFlirt;
            }
            else
            {
                //revise to include flirt type
                var chance = Mathf.Clamp01(GradualRomanceMod.RomanticSuccessRate *
                                           Mathf.Pow(initiatorPhysicalAttraction, flirtStyle.baseSexiness) *
                                           Mathf.Pow(initiatorRomanticAttraction, flirtStyle.baseRomance) *
                                           Mathf.Pow(initiatorSocialAttraction, flirtStyle.baseLogic) *
                                           recipientCircumstances * 0.65f);
                Log.Message("Romance success chance: " + chance);
                flirtReaction = Rand.Value < chance ? successfulFlirt : unsuccessfulFlirt;

                LogFlirt(recipient.Name.ToStringShort + " chose reaction " + flirtReaction.defName +
                         " from Successful: " + successfulFlirt.defName + "; Unsuccessful: " +
                         unsuccessfulFlirt.defName + ".");
            }


            if (flirtReaction == null)
            {
                Log.Error("FailedToFindReaction_error".Translate());
                letterText = null;
                letterLabel = null;
                letterDef = null;
                return;
            }

            if (initiator.gender == Gender.Male)
            {
                extraSentencePacks.Add(flirtReaction.maleRulePack);
            }

            if (initiator.gender == Gender.Female)
            {
                extraSentencePacks.Add(flirtReaction.femaleRulePack);
            }

            if (flirtReaction != FlirtReactionDefOf.Ignorant)
            {
                if (flirtReaction.successful)
                {
                    if (veryHighRecipientReasons.Count > 0)
                    {
                        var reason = veryHighRecipientReasons.RandomElement();
                        extraSentencePacks.Add(reason.reactionPositiveText);
                    }
                    else if (highRecipientReasons.Count > 0)
                    {
                        var reason = highRecipientReasons.RandomElement();
                        extraSentencePacks.Add(reason.reactionPositiveText);
                    }
                }
                else
                {
                    if (veryLowRecipientReasons.Count > 0)
                    {
                        var reason = veryLowRecipientReasons.RandomElement();
                        extraSentencePacks.Add(reason.reactionNegativeText);
                    }
                    else if (lowRecipientReasons.Count > 0)
                    {
                        var reason = lowRecipientReasons.RandomElement();
                        extraSentencePacks.Add(reason.reactionNegativeText);
                    }
                }
            }

            flirtReaction.worker.GiveThoughts(initiator, recipient, out var yetMoreSentencePacks);

            extraSentencePacks.AddRange(yetMoreSentencePacks);

            letterText = null;
            letterLabel = null;
            letterDef = null;

            var loversInSight = RelationshipUtility.PotentiallyJealousPawnsInLineOfSight(initiator);
            var loversInSight2 = RelationshipUtility.PotentiallyJealousPawnsInLineOfSight(recipient);

            foreach (var observer in loversInSight)
            {
                if (!BreakupUtility.ShouldBeJealous(observer, initiator, recipient))
                {
                    continue;
                }

                observer.needs.mood.thoughts.memories
                    .TryGainMemory(ThoughtDefOfGR.CaughtFlirting, initiator);
                if (flirtReaction.successful)
                {
                    observer.needs.mood.thoughts.memories
                        .TryGainMemory(ThoughtDefOfGR.CaughtFlirtingWithLover, recipient);
                }
            }

            if (!flirtReaction.successful)
            {
                return;
            }

            if (flirtReaction.provokesJealousy)
            {
                foreach (var observer in loversInSight2)
                {
                    if (!BreakupUtility.ShouldBeJealous(observer, initiator, recipient))
                    {
                        continue;
                    }

                    observer.needs.mood.thoughts.memories
                        .TryGainMemory(ThoughtDefOfGR.CaughtFlirting, recipient);
                    observer.needs.mood.thoughts.memories
                        .TryGainMemory(ThoughtDefOfGR.CaughtFlirtingWithLover, initiator);
                }
            }


            RelationshipUtility.AdvanceInformalRelationship(initiator, recipient, out var newRelation,
                flirtStyle.baseSweetheartChance * flirtReaction.sweetheartModifier);

            if (newRelation == null || !PawnUtility.ShouldSendNotificationAbout(initiator) &&
                !PawnUtility.ShouldSendNotificationAbout(recipient))
            {
                return;
            }

            var initiatorParagraph = AttractionUtility.WriteReasonsParagraph(initiator, recipient,
                veryHighInitiatorReasons, highInitiatorReasons, lowInitiatorReasons, veryLowInitiatorReasons);
            var recipientParagraph = AttractionUtility.WriteReasonsParagraph(recipient, initiator,
                veryHighRecipientReasons, highRecipientReasons, lowRecipientReasons, veryLowRecipientReasons);
            letterDef = LetterDefOf.PositiveEvent;
            letterLabel = newRelation.GetModExtension<RomanticRelationExtension>().newRelationshipTitleText
                .Translate();
            letterText = newRelation.GetModExtension<RomanticRelationExtension>().newRelationshipLetterText
                .Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"));


            letterText += initiatorParagraph;
            letterText += recipientParagraph;

            /*    if (newRelation == PawnRelationDefOfGR.Sweetheart)
                        {
                            letterDef = LetterDefOf.PositiveEvent;
                            letterLabel = "NewSweetheartsLabel".Translate();
                            letterText = "NewSweetheartsText".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"));
                        }
                        if (newRelation == PawnRelationDefOfGR.Lovebuddy)
                        {
                            letterDef = LetterDefOf.PositiveEvent;
                            letterLabel = "NewLovebuddiesLabel".Translate();
                            letterText = "NewLovebuddiesText".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"));
                        }
                        if (newRelation == PawnRelationDefOfGR.Paramour)
                        {
                            if (RelationshipUtility.IsAnAffair(initiator, recipient, out Pawn initiatorSO, out Pawn recipientSO))
                            {
                                letterDef = LetterDefOf.NegativeEvent;
                                letterLabel = "ParamoursAffairLabel".Translate();
                                if (initiatorSO != null && recipientSO != null)
                                {
                                    letterText = "ParamoursAffairTwoCuckoldsText".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"), initiatorSO.Named("CUCKOLD1"), recipientSO.Named("CUCKOLD2"));
                                }
                                if (initiatorSO != null && recipientSO == null)
                                {
                                    letterText = "ParamoursAffairInitiatorCuckoldText".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"), initiatorSO.Named("CUCKOLD1"));
                                }
                                if (initiatorSO == null && recipientSO != null)
                                {
                                    letterText = "ParamoursAffairRecipientCuckoldText".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"), recipientSO.Named("CUCKOLD1"));
                                }
                            }
                            else
                            {
                                letterDef = LetterDefOf.PositiveEvent;
                                letterLabel = "NewParamoursLabel".Translate();
                                letterText = "NewParamoursText".Translate(initiator.Named("PAWN1"), recipient.Named("PAWN2"));
                            }
                        }*/
        }


        private float CalculateFlirtStyleWeight(FlirtStyleDef flirtStyle, Pawn pawn, Pawn other)
        {
            var flirtLog = pawn.Name.ToStringShort + " => " + other.Name.ToStringShort + " considers " +
                           flirtStyle.defName + ": ";
            //if a pawn has a canceling trait, we abort immediately
            foreach (var traitDef in flirtStyle.cancelingTraits)
            {
                if (!pawn.story.traits.HasTrait(traitDef))
                {
                    continue;
                }

                flirtLog += "canceled by " + traitDef.defName + ".";
                LogFlirt(flirtLog);
                return 0f;
            }

            //we start with base weight chance
            var weight = flirtStyle.baseChance;

            //add relationship factor
            weight *= RelationshipFactorForFlirtStyle(RelationshipUtility.MostAdvancedRomanceOrExStage(pawn, other),
                flirtStyle);
            flirtLog += "base " + weight + " ";
            //calculate attraction factors
            /*
            weight *= recipientPhysicalAttraction * flirtStyle.baseSexiness;
            flirtLog += "physical " + weight.ToString() + " ";
            weight *= recipientRomanticAttraction * flirtStyle.baseRomance;
            flirtLog += "romantic " + weight.ToString() + " ";
            weight *= recipientSocialAttraction * flirtStyle.baseLogic;
            flirtLog += "logical " + weight.ToString() + " ";
            */


            //calculate promoting traits
            foreach (var traitModifier in flirtStyle.traitModifiers)
            {
                if (!pawn.story.traits.HasTrait(traitModifier.trait))
                {
                    continue;
                }

                weight = weight * traitModifier.modifier;
                flirtLog += traitModifier.trait.defName + ": " + weight + " ";
            }

            if (PsycheHelper.PsychologyEnabled(pawn) && GradualRomanceMod.AttractionCalculation ==
                GradualRomanceMod.AttractionCalculationSetting.Complex)
            {
                //calculate contributing personality traits
                foreach (var currentModifier in flirtStyle.moreLikelyPersonalities)
                {
                    weight = weight *
                             Mathf.Pow(
                                 Mathf.Lerp(0.5f, 1.5f,
                                     PsycheHelper.Comp(pawn).Psyche
                                         .GetPersonalityRating(currentModifier.personalityNode)),
                                 currentModifier.modifier);
                    flirtLog += currentModifier.personalityNode.defName + "+: " + weight + " ";
                }

                foreach (var currentModifier in flirtStyle.lessLikelyPersonalities)
                {
                    weight = weight *
                             Mathf.Pow(
                                 Mathf.Lerp(0.5f, 1.5f,
                                     Mathf.Abs(1 - PsycheHelper.Comp(pawn).Psyche
                                         .GetPersonalityRating(currentModifier.personalityNode))),
                                 currentModifier.modifier);
                    flirtLog += currentModifier.personalityNode.defName + "-: " + weight + " ";
                }
            }

            if (flirtStyle.incompetent)
            {
                weight *= pressureCache;
                flirtLog += "pressure: " + weight + " ";
            }

            flirtLog += "end.";
            LogFlirt(flirtLog);
            return weight;
        }

        private float CalculateFlirtReactionWeight(FlirtStyleDef flirtStyle, FlirtReactionDef flirtReaction,
            Pawn initiator, Pawn recipient)
        {
            var chance = GradualRomanceMod.RomanticSuccessRate * flirtReaction.baseChance;
            var log = "Reaction " + flirtReaction.defName + ": base chance " + chance;

            if (successImpossible && flirtReaction.successful)
            {
                log += ". Canceled, success impossible.";
                LogFlirt(log);
                return 0f;
            }

            if (successImpossible == false)
            {
                chance *= Mathf.Pow(flirtReaction.sexyReaction * initiatorPhysicalAttraction, flirtStyle.baseSexiness);
                log += " sexiness " + chance;
                chance *= Mathf.Pow(flirtReaction.romanticReaction * initiatorRomanticAttraction,
                    flirtStyle.baseRomance);
                log += " romance " + chance;
                chance *= Mathf.Pow(flirtReaction.logicalReaction * initiatorSocialAttraction, flirtStyle.baseLogic);
                log += " logic " + chance;
            }

            chance *= Mathf.Pow(flirtReaction.obscureReaction, flirtStyle.baseObscurity);
            log += " obscurity " + chance;
            //risky flirts are less risky if the two pawns are familiar with each other.
            if (RelationshipUtility.MostAdvancedRelationshipBetween(initiator, recipient) == null)
            {
                chance *= Mathf.Pow(flirtReaction.riskyReaction, flirtStyle.baseRiskiness);
            }
            else
            {
                chance *= Mathf.Pow(Mathf.Pow(flirtReaction.riskyReaction, flirtStyle.baseRiskiness),
                    FamiliarityFactor);
            }

            chance *= Mathf.Pow(flirtReaction.riskyReaction, flirtStyle.baseRiskiness);
            log += " riskiness " + chance;
            chance *= Mathf.Pow(flirtReaction.awkwardReaction, flirtStyle.baseAwkwardness);
            log += " awkward " + chance + "; ";

            if (GradualRomanceMod.AttractionCalculation == GradualRomanceMod.AttractionCalculationSetting.Complex)
            {
                foreach (var node in flirtReaction.personalityModifiers)
                {
                    if (node.modifier >= 0)
                    {
                        chance = chance *
                                 Mathf.Pow(
                                     Mathf.Lerp(0.5f, 1.5f,
                                         PsycheHelper.Comp(recipient).Psyche
                                             .GetPersonalityRating(node.personalityNode)), node.modifier);
                        log += node.personalityNode.defName + "+: " + chance + ", ";
                    }
                    else
                    {
                        chance = chance *
                                 Mathf.Pow(
                                     Mathf.Lerp(0.5f, 1.5f,
                                         Mathf.Abs(1 - PsycheHelper.Comp(recipient).Psyche
                                             .GetPersonalityRating(node.personalityNode))), Mathf.Abs(node.modifier));
                        log += node.personalityNode.defName + "-: " + chance + ", ";
                    }
                }
            }

            foreach (var traitModifier in flirtReaction.traitModifiers)
            {
                if (!recipient.story.traits.HasTrait(traitModifier.trait))
                {
                    continue;
                }

                chance *= traitModifier.modifier;
                log += traitModifier.trait.defName + " " + chance + ", ";
            }

            /*
            if (flirtReaction.successful == true)
            {
                chance *= initiator.GetStatValue(StatDefOf.SocialImpact);
                log += "social impact: " + chance.ToString() + ", ";
                //chance *= recipientCircumstances;
            }*/
            log += "end.";
            LogFlirt(log);
            return chance;
        }


        private static float RelationshipFactorForFlirtStyle(PawnRelationDef relation, FlirtStyleDef flirtStyle)
        {
            if (relation == PawnRelationDefOf.Spouse || relation == PawnRelationDefOf.ExSpouse)
            {
                return flirtStyle.spouseFactor;
            }

            if (relation == PawnRelationDefOf.Fiance || relation == PawnRelationDefOf.Lover ||
                relation == PawnRelationDefOf.ExLover)
            {
                return flirtStyle.loverFactor;
            }

            if (relation == PawnRelationDefOfGR.Lovefriend || relation == PawnRelationDefOfGR.ExLovefriend ||
                relation == PawnRelationDefOfGR.Paramour)
            {
                return flirtStyle.lovefriendFactor;
            }

            if (relation == PawnRelationDefOfGR.Lovebuddy)
            {
                return flirtStyle.loveBuddyFactor;
            }

            if (relation == PawnRelationDefOfGR.Sweetheart)
            {
                return flirtStyle.sweetheartFactor;
            }

            return flirtStyle.acquaitanceFactor;
        }

        private static void LogFlirt(string message)
        {
            if (GradualRomanceMod.detailedDebugLogs)
            {
                Log.Message(message);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(veryHighRecipientReasons.All(item => item != null));
        }
    }
}