using System.Linq;
using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Incest : AttractionCalculator
    {
        public override bool Check(Pawn observer, Pawn assessed)
        {
            if (!observer.relations.FamilyByBlood.Contains(assessed))
            {
                return false;
            }

            //psychopathic lechers don't care at all
            if (observer.story.traits.HasTrait(TraitDefOfPsychology.Lecher) &&
                observer.story.traits.HasTrait(TraitDefOf.Psychopath))
            {
                return false;
            }

            return true;
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            var relations = observer.GetRelations(assessed).ToList();
            var relation = relations[0];
            foreach (var relationDef in relations)
            {
                if (relationDef.incestOpinionOffset > relation.incestOpinionOffset)
                {
                    relation = relationDef;
                }
            }

            var incestFactor = 1f / (Mathf.Abs(relation.incestOpinionOffset) + 1);

            return incestFactor;
        }
    }
}