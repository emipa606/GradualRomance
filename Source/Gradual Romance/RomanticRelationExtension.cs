using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class RomanticRelationExtension : DefModExtension
    {
        public float baseAffairReluctance = 1f;
        public float baseRomanceChance = 0f;
        public float breakupIntensity;
        public bool caresAboutCheating;
        public List<ThoughtCondition> conditions = new List<ThoughtCondition>();
        public bool decayable = false;
        public bool doesLovin;
        public PawnRelationDef ex;
        public bool goesOnDates;
        public bool isFormalRelationship;
        public bool needsDivorce = false;
        public string newRelationshipLetterText;
        public string newRelationshipTitleText;
        public int relationshipLevel;
        public bool sharesBed;
    }
}