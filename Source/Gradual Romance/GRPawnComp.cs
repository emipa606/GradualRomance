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
        if (pawn is { Spawned: false, Dead: true })
        {
            return;
        }

        if (pawn.IsHashIntervalTick(recachePerTick))
        {
            refreshCache();
        }


        if (!pawn.IsHashIntervalTick(GenDate.TicksPerDay))
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
        var recordsToRemove = new List<Pawn>();
        foreach (var p in keys)
        {
            if (!IsPawnAttractionRelevant(p))
            {
                recordsToRemove.Add(p);
            }
        }

        foreach (var pawn in recordsToRemove)
        {
            AttractionRecords.Remove(pawn);
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
}