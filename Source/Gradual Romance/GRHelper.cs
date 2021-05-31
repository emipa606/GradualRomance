using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class GRHelper
    {
        public static bool ShouldApplyFemaleDifference(Gender testedGender = Gender.Female)
        {
            return GradualRomanceMod.genderMode == GradualRomanceMod.GenderModeSetting.Vanilla &&
                   testedGender == Gender.Female ||
                   GradualRomanceMod.genderMode == GradualRomanceMod.GenderModeSetting.Inverse &&
                   testedGender == Gender.Male;
        }

        public static bool ShouldApplyMaleDifference(Gender testedGender = Gender.Male)
        {
            return GradualRomanceMod.genderMode == GradualRomanceMod.GenderModeSetting.Vanilla &&
                   testedGender == Gender.Male ||
                   GradualRomanceMod.genderMode == GradualRomanceMod.GenderModeSetting.Inverse &&
                   testedGender == Gender.Female;
        }

        public static GRPawnComp GRPawnComp(Pawn pawn)
        {
            return pawn.GetComp<GRPawnComp>();
        }

        public static GRBodyTypeExtension BodyTypeExtension(BodyTypeDef bodyType)
        {
            try
            {
                return bodyType.GetModExtension<GRBodyTypeExtension>();
            }
            catch
            {
                return null;
            }
        }

        public static XenoRomanceExtension XenoRomanceExtension(ThingDef thing)
        {
            try
            {
                return thing.GetModExtension<XenoRomanceExtension>();
            }
            catch
            {
                return null;
            }
        }

        public static RomanticRelationExtension RomanticRelationExtension(PawnRelationDef relation)
        {
            try
            {
                return relation.GetModExtension<RomanticRelationExtension>();
            }
            catch
            {
                return null;
            }
        }
    }
}