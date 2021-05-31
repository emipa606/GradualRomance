using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class XenoRomanceExtension : DefModExtension
    {
        public readonly int averageKinseyFemale = -1;
        public readonly int averageKinseyMale = -1;

        public readonly string bodyCategory = "Humanoid";

        //public float minimumAgeDeviation = 1f;
        // public float maximumAgeDeviation = 10f;
        public readonly bool canGoIntoHeat = false;

        public readonly string faceCategory = "Humanoid";
        public readonly List<Vector2> maturityByAgeCurveFemale = new List<Vector2>();
        public readonly List<Vector2> maturityByAgeCurveMale = new List<Vector2>();
        public readonly string mindCategory = "Humanoid";
        public readonly List<Vector2> sexDriveByAgeCurveFemale = new List<Vector2>();

        public readonly List<Vector2> sexDriveByAgeCurveMale = new List<Vector2>();

        //public float youngAdultAge = 18f;
        //public float midlifeAge = 40f;
        public float extraspeciesAppeal = 0.75f;

        //public List<Vector2> attractivenessByAgeCurveMale = new List<Vector2> { };
        //public List<Vector2> attractivenessByAgeCurveFemale = new List<Vector2> { };
        public ThingDef subspeciesOf = null;
    }
}