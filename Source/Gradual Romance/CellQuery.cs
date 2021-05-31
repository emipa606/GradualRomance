using System.Collections.Generic;
using Verse;

namespace Gradual_Romance
{
    public class CellQuery
    {
        public static IEnumerable<IntVec3> GetRandomCellSampleAround(Thing t, int numberOfSamples, int distance)
        {
            var center = t.Position;
            var area = new CellRect(center.x - distance, center.z - distance, distance * 2, distance * 2);
            if (!area.InBounds(t.Map))
            {
                area = area.ClipInsideMap(t.Map);
            }

            for (var i = 0; i < numberOfSamples; i++)
            {
                yield return area.RandomCell;
            }
        }
    }
}