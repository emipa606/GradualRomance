using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class FlirtReactionDef : Def
    {
        private readonly Type workerClass = typeof(FlirtReactionWorker);
        public float awkwardReaction;
        public float baseChance;
        public RulePackDef femaleRulePack;
        public List<ThoughtDef> givesTension = new List<ThoughtDef>();
        public float logicalReaction;

        public RulePackDef maleRulePack;
        public float obscureReaction;
        public List<PersonalityNodeModifier> personalityModifiers = new List<PersonalityNodeModifier>();
        public bool provokesJealousy;
        public float riskyReaction;
        public float romanticReaction;
        public float sexyReaction;
        public bool successful;
        public float sweetheartModifier = 1f;
        public List<TraitModifier> traitModifiers = new List<TraitModifier>();

        [Unsaved] private FlirtReactionWorker workerInt;

        public FlirtReactionWorker worker
        {
            get
            {
                if (workerInt != null)
                {
                    return workerInt;
                }

                workerInt = (FlirtReactionWorker) Activator.CreateInstance(workerClass);
                workerInt.reaction = this;

                return workerInt;
            }
        }
    }
}