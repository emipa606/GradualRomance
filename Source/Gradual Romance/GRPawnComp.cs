using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance;

public class GRPawnComp : ThingComp
{
    private const int recalculateAttractionPerTick = 5000;
    private const int recachePerTick = 2500;

    private readonly Dictionary<Pawn, AttractionRecord>
        AttractionRecords = new Dictionary<Pawn, AttractionRecord>();
    /*
    public float cachedSkillAttractiveness = 1f;
    public float cachedBeautyAttractiveness = 1f;
    public float cachedWealthAttractiveness = 1f;
    public int cachedNumberOfColonyFriends = 0;
    */

    public GRPawnComp_Properties Props => (GRPawnComp_Properties)props;

    //public float facialAttractiveness = 0f;


    public override void CompTick()
    {
        var pawn = parent as Pawn;
        var gameTicks = Find.TickManager.TicksGame;
        if (pawn != null && gameTicks % recachePerTick == 0 && pawn.Spawned && !pawn.Dead)
        {
            refreshCache();
        }

        if (pawn != null && (gameTicks % GenDate.TicksPerDay != 0 || !pawn.Spawned || pawn.Dead))
        {
            return;
        }

        var relations = pawn?.relations.DirectRelations;
        if (relations != null)
        {
            foreach (var directPawnRelation in relations)
            {
                if (!RelationshipUtility.ListOfRomanceStages().Contains(directPawnRelation.def))
                {
                    continue;
                }

                if (!BreakupUtility.CanDecay(pawn, directPawnRelation.otherPawn, directPawnRelation.def))
                {
                    continue;
                }

                if (GradualRomanceMod.DecayRate <= Rand.Value)
                {
                    BreakupUtility.DecayRelationship(pawn, directPawnRelation.otherPawn,
                        directPawnRelation.def);
                }
            }
        }

        CleanAttractionRecords();
    }

    private void refreshCache()
    {
        //cachedSkillAttractiveness = AttractionUtility.GetObjectiveSkillAttractiveness(pawn);
        //cachedWealthAttractiveness = AttractionUtility.GetObjectiveWealthAttractiveness(pawn);
        //cachedNumberOfColonyFriends = RelationshipUtility.NumberOfFriends(pawn);
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        refreshCache();
        /*if (facialAttractiveness == 0f)
        {
            Rand.PushState((pawn.thingIDNumber ^ 17) * Time.time.GetHashCode());
            facialAttractiveness = Mathf.Clamp(Rand.Gaussian(1f, .3f), 0.01f, 3f);
            Rand.PopState();
        }*/
    }

    public override void PostExposeData()
    {
        //Scribe_Values.Look(ref facialAttractiveness, "facialattractiveness", 0f);
    }

    private bool IsPawnAttractionRelevant(Pawn other)
    {
        if (other.Dead)
        {
            return false;
        }

        return other.IsColonist || other.Spawned;
    }

    private void CleanAttractionRecords()
    {
        IEnumerable<Pawn> keys = AttractionRecords.Keys;
        foreach (var p in keys)
        {
            if (!IsPawnAttractionRelevant(p))
            {
                AttractionRecords.Remove(p);
            }
        }
    }

    private AttractionRecord PullRecord(Pawn other, bool noUpdate = false)
    {
        var p = (Pawn)parent;
        if (!AttractionRecords.ContainsKey(other))
        {
            AttractionRecords.Add(other, new AttractionRecord(p, other));
        }
        else if (noUpdate == false && Find.TickManager.TicksGame - AttractionRecords[other].lastRefreshedGameTick >
                 recalculateAttractionPerTick)
        {
            AttractionRecords[other].Update(p, other);
        }

        return AttractionRecords[other];
    }


    public float RetrieveAttraction(Pawn other, bool romantic = false, bool chanceOnly = false)
    {
        var record = PullRecord(other);
        return record.RetrieveAttraction(romantic, chanceOnly);
    }

    public float RetrieveAttractionForCategory(Pawn other, AttractionFactorCategoryDef category)
    {
        var record = PullRecord(other);
        return record.RetrieveAttractionForCategory(category);
    }

    public void RetrieveFactors(Pawn other, out List<AttractionFactorDef> veryLowFactors,
        out List<AttractionFactorDef> lowFactors, out List<AttractionFactorDef> highFactors,
        out List<AttractionFactorDef> veryHighFactors, bool romantic = false, bool chanceOnly = false)
    {
        var record = PullRecord(other);
        record.RetrieveFactors(out veryLowFactors, out lowFactors, out highFactors, out veryHighFactors);
        if (!romantic)
        {
            veryHighFactors.RemoveAll(x => x.category.onlyForRomance);
            highFactors.RemoveAll(x => x.category.onlyForRomance);
            lowFactors.RemoveAll(x => x.category.onlyForRomance);
            veryLowFactors.RemoveAll(x => x.category.onlyForRomance);
        }

        if (chanceOnly)
        {
            return;
        }

        {
            veryHighFactors.RemoveAll(x => x.category.chanceOnly);
            highFactors.RemoveAll(x => x.category.chanceOnly);
            lowFactors.RemoveAll(x => x.category.chanceOnly);
            veryLowFactors.RemoveAll(x => x.category.chanceOnly);
        }
    }

    public float RetrieveAttractionAndFactors(Pawn other, out List<AttractionFactorDef> veryLowFactors,
        out List<AttractionFactorDef> lowFactors, out List<AttractionFactorDef> highFactors,
        out List<AttractionFactorDef> veryHighFactors, bool romantic = false, bool chanceOnly = false)
    {
        var record = PullRecord(other);
        record.RetrieveFactors(out veryLowFactors, out lowFactors, out highFactors, out veryHighFactors);
        if (!romantic)
        {
            veryHighFactors.RemoveAll(x => x.category.onlyForRomance);
            highFactors.RemoveAll(x => x.category.onlyForRomance);
            lowFactors.RemoveAll(x => x.category.onlyForRomance);
            veryLowFactors.RemoveAll(x => x.category.onlyForRomance);
        }

        if (chanceOnly)
        {
            return record.RetrieveAttraction(romantic, true);
        }

        {
            veryHighFactors.RemoveAll(x => x.category.chanceOnly);
            highFactors.RemoveAll(x => x.category.chanceOnly);
            lowFactors.RemoveAll(x => x.category.chanceOnly);
            veryLowFactors.RemoveAll(x => x.category.chanceOnly);
        }

        return record.RetrieveAttraction(romantic);
    }

    private class AttractionRecord
    {
        private readonly Dictionary<AttractionFactorCategoryDef, float> categoryCalculations;
        private readonly List<AttractionFactorDef> highFactors = new List<AttractionFactorDef>();
        private readonly List<AttractionFactorDef> lowFactors = new List<AttractionFactorDef>();
        private readonly List<AttractionFactorDef> veryHighFactors = new List<AttractionFactorDef>();
        private readonly List<AttractionFactorDef> veryLowFactors = new List<AttractionFactorDef>();
        public int lastRefreshedGameTick;

        public AttractionRecord(Pawn pawn, Pawn other)
        {
            //Log.Message("Start constructor for " + pawn.Name.ToStringShort + " : " + other.Name.ToStringShort);
            lastRefreshedGameTick = Find.TickManager.TicksGame;
            //Log.Message("Made dictionary.");
            categoryCalculations = new Dictionary<AttractionFactorCategoryDef, float>();
            if (pawn == other)
            {
                return;
            }

            //Log.Message("Going through categories.");
            var allDefs = DefDatabase<AttractionFactorCategoryDef>.AllDefsListForReading;
            foreach (var category in allDefs)
            {
                //Log.Message("Processing " + category.defName);
                if (category.alwaysRecalculate)
                {
                    //Log.Message("Passing " + category.defName);
                    continue;
                }

                //Log.Message("Adding result " + category.defName);
                var result = AttractionUtility.CalculateAttractionCategory(category, pawn, other,
                    out var newVeryLowFactors, out var newLowFactors, out var newHighFactors,
                    out var newVeryHighFactors,
                    out _);

                categoryCalculations.Add(category, result);
                //Log.Message("Adding factors.");
                veryHighFactors.AddRange(newVeryHighFactors);
                highFactors.AddRange(newHighFactors);
                lowFactors.AddRange(newLowFactors);
                veryLowFactors.AddRange(newVeryLowFactors);
                //Log.Message("Finished adding factors.");
            }
        }

        public void Update(Pawn pawn, Pawn other)
        {
            lastRefreshedGameTick = Find.TickManager.TicksGame;
            veryHighFactors.Clear();
            highFactors.Clear();
            lowFactors.Clear();
            veryLowFactors.Clear();
            if (pawn == other)
            {
                return;
            }

            foreach (var category in DefDatabase<AttractionFactorCategoryDef>.AllDefs)
            {
                if (category.alwaysRecalculate || category.chanceOnly)
                {
                    continue;
                }

                var result = AttractionUtility.CalculateAttractionCategory(category, pawn, other,
                    out var newVeryLowFactors, out var newLowFactors, out var newHighFactors,
                    out var newVeryHighFactors,
                    out _);

                categoryCalculations[category] = result;
                veryHighFactors.AddRange(newVeryHighFactors);
                highFactors.AddRange(newHighFactors);
                lowFactors.AddRange(newLowFactors);
                veryLowFactors.AddRange(newVeryLowFactors);
            }
        }

        public void RetrieveFactors(out List<AttractionFactorDef> veryLowFactors2,
            out List<AttractionFactorDef> lowFactors2, out List<AttractionFactorDef> highFactors2,
            out List<AttractionFactorDef> veryHighFactors2)
        {
            veryLowFactors2 = new List<AttractionFactorDef>();
            lowFactors2 = new List<AttractionFactorDef>();
            highFactors2 = new List<AttractionFactorDef>();
            veryHighFactors2 = new List<AttractionFactorDef>();
            veryLowFactors2.AddRange(veryLowFactors);
            lowFactors2.AddRange(lowFactors);
            highFactors2.AddRange(highFactors);
            veryHighFactors2.AddRange(veryHighFactors);
        }

        public float RetrieveAttraction(bool romantic = false, bool chanceOnly = false)
        {
            var attraction = 1f;
            foreach (var category in categoryCalculations.Keys)
            {
                if (!romantic && category.onlyForRomance)
                {
                    continue;
                }

                if (!chanceOnly && category.chanceOnly)
                {
                    continue;
                }

                attraction *= categoryCalculations[category];
            }

            return attraction;
        }

        public float RetrieveAttractionForCategory(AttractionFactorCategoryDef category)
        {
            if (!category.alwaysRecalculate)
            {
                return categoryCalculations[category];
            }

            Log.Error(
                $"[Gradual_Romance] Tried to pull a record for {category.defName}, but category is set to AlwaysRecalculate and is never stored.");
            return 1f;
        }
    }
}