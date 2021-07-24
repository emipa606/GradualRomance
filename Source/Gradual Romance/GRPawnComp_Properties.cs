using System;
using Verse;

namespace Gradual_Romance
{
    public class GRPawnComp_Properties : CompProperties
    {
        public float facialAttractiveness;

        public GRPawnComp_Properties()
        {
            compClass = typeof(GRPawnComp);
        }

        public GRPawnComp_Properties(Type compClass) : base(compClass)
        {
            this.compClass = compClass;
        }
    }
}