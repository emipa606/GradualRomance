using RimWorld;

namespace Gradual_Romance
{
    [DefOf]
    public static class AttractionFactorCategoryDefOf
    {
        public static AttractionFactorCategoryDef Physical;

        public static AttractionFactorCategoryDef Romantic;

        public static AttractionFactorCategoryDef Social;

        public static AttractionFactorCategoryDef Circumstance;

        public static AttractionFactorCategoryDef Relationship;

        static AttractionFactorCategoryDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PawnRelationDefOfGR));
        }
    }
}