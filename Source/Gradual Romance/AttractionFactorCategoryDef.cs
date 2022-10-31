using Verse;

namespace Gradual_Romance;

public class AttractionFactorCategoryDef : Def
{
    //when always recalculate is on, GR never caches the result.
    public bool alwaysRecalculate = false;

    //when chance only is on, the result of the category is never reflected in the total attraction. For example, if you have an attraction factor based on the observer's
    //current mood, they aren't less attracted to the other pawn, just not in the mood.
    public bool chanceOnly = false;

    //when true, this category is only calculated for romance chance.
    public bool onlyForRomance = false;
}