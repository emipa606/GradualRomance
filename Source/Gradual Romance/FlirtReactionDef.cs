using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Psychology;

namespace Gradual_Romance
{
    public class FlirtReactionDef : Def
    {
        public FlirtReactionWorker worker
        {
            get
            {
                if (this.workerInt == null)
                {
                    this.workerInt = (FlirtReactionWorker)Activator.CreateInstance(this.workerClass);
                    this.workerInt.reaction = this;
                }
                return this.workerInt;
            }
        }

        public RulePackDef maleRulePack;
        public RulePackDef femaleRulePack;
        public bool successful;
        public List<PersonalityNodeModifier> personalityModifiers = new List<PersonalityNodeModifier> { };
        public List<TraitModifier> traitModifiers = new List<TraitModifier> { };
        public float baseChance;
        public float sexyReaction;
        public float romanticReaction;
        public float logicalReaction;
        public float awkwardReaction;
        public float riskyReaction;
        public float obscureReaction;
        public bool provokesJealousy;
        public List<ThoughtDef> givesTension = new List<ThoughtDef>() { };
        public float sweetheartModifier = 1f;

        private Type workerClass = typeof(FlirtReactionWorker);
        [Unsaved]
        private FlirtReactionWorker workerInt;

    }
}
