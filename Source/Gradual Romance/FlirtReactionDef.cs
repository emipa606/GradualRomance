using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class FlirtReactionDef : Def
{
    public readonly List<ThoughtDef> givesTension = [];
    public readonly List<PersonalityNodeModifier> personalityModifiers = [];
    public readonly float sweetheartModifier = 1f;
    public readonly List<TraitModifier> traitModifiers = [];
    private readonly Type workerClass = typeof(FlirtReactionWorker);
    public float awkwardReaction;
    public float baseChance;
    public RulePackDef femaleRulePack;
    public float logicalReaction;

    public RulePackDef maleRulePack;
    public float obscureReaction;
    public bool provokesJealousy;
    public float riskyReaction;
    public float romanticReaction;
    public float sexyReaction;
    public bool successful;

    [Unsaved] private FlirtReactionWorker workerInt;

    public FlirtReactionWorker worker
    {
        get
        {
            if (workerInt != null)
            {
                return workerInt;
            }

            workerInt = (FlirtReactionWorker)Activator.CreateInstance(workerClass);
            workerInt.reaction = this;

            return workerInt;
        }
    }
}