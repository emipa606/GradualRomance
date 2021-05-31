using Verse;

namespace Gradual_Romance
{
    public class AttractionCalculator
    {
        public AttractionFactorDef def;

        public virtual bool Check(Pawn observer, Pawn assessed)
        {
            return true;
        }

        public virtual float Calculate(Pawn observer, Pawn assessed)
        {
            return 1f;
        }
    }
}