using RimWorld;

namespace Gradual_Romance
{
    [DefOf]
    public static class ThoughtDefOfGR
    {
        public static ThoughtDef RomanticInterest;

        public static ThoughtDef RomanticDisinterest;

        public static ThoughtDef RomanticTension;

        public static ThoughtDef SexualTension;

        public static ThoughtDef LogicalTension;

        //public static ThoughtDef ColdFeetSharingBed;

        public static ThoughtDef BotchedFlirt;

        public static ThoughtDef CheatedOnMePolygamist;

        public static ThoughtDef CheatedOnMeTolerated;

        public static ThoughtDef CheatedOnMeHighOpinion;

        public static ThoughtDef LoversLover;

        public static ThoughtDef CaughtFlirting;

        public static ThoughtDef CaughtFlirtingWithLover;

        public static ThoughtDef FeelingNeglected;

        static ThoughtDefOfGR()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOfGR));
        }
    }
}