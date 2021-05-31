using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator_Xeno : AttractionCalculator
    {
        public override bool Check(Pawn observer, Pawn assessed)
        {
            return observer.def != assessed.def;
        }

        public override float Calculate(Pawn observer, Pawn assessed)
        {
            if (GradualRomanceMod.extraspeciesRomance == GradualRomanceMod.ExtraspeciesRomanceSetting.NoXenoRomance ||
                ModHooks.IsXenophobe(observer))
            {
                return 0f;
            }

            if (GradualRomanceMod.extraspeciesRomance == GradualRomanceMod.ExtraspeciesRomanceSetting.OnlyXenophiles &&
                !ModHooks.IsXenophile(observer))
            {
                return 0f;
            }

            if (GradualRomanceMod.extraspeciesRomance == GradualRomanceMod.ExtraspeciesRomanceSetting.CaptainKirk)
            {
                return 1f;
            }

            var observerXenoRomance = observer.def.GetModExtension<XenoRomanceExtension>();
            var assessedXenoRomance = assessed.def.GetModExtension<XenoRomanceExtension>();
            var extraspeciesAppeal = assessedXenoRomance.extraspeciesAppeal;
            if (extraspeciesAppeal <= 0)
            {
                return 0f;
            }

            if (extraspeciesAppeal >= 1)
            {
                return 1f;
            }

            var xenoFactor = extraspeciesAppeal;
            if (observerXenoRomance.faceCategory != assessedXenoRomance.faceCategory)
            {
                xenoFactor *= extraspeciesAppeal;
            }

            if (observerXenoRomance.bodyCategory != assessedXenoRomance.bodyCategory)
            {
                xenoFactor *= extraspeciesAppeal;
            }

            if (observerXenoRomance.mindCategory != assessedXenoRomance.mindCategory)
            {
                xenoFactor *= extraspeciesAppeal;
            }

            return xenoFactor;
        }
    }
}