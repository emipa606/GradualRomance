using RimWorld;
using Verse;

namespace Gradual_Romance
{
    [DefOf]
    public static class RulePackDefOfGR
    {
        public static RulePackDef Sentence_FlirtAttemptAccepted;

        public static RulePackDef Sentence_FlirtAttemptRejectedNeutral;

        public static RulePackDef Sentence_FlirtAttemptRejectedOffended;

        public static RulePackDef Sentence_FlirtAttemptRejectedPolite;

        public static RulePackDef Sentence_FlirtAttemptOblivious;

        public static RulePackDef Sentence_InterpretFlirtPositively;

        public static RulePackDef Sentence_InterpretFlirtNegatively;

        public static RulePackDef Sentence_FeltShame;

        static RulePackDefOfGR()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RulePackDefOfGR));
        }
    }
}