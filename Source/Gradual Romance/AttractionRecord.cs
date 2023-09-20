using System.Collections.Generic;
using Verse;

namespace Gradual_Romance;

public class AttractionRecord
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