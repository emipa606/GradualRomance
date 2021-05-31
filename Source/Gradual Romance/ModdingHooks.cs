using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public static class ModHooks
    {
        //BIRDS AND BEES
        public static bool UsingBirdsAndBees()
        {
            return LoadedModManager.RunningModsListForReading.Any(x => x.Name == "The Birds and the Bees");
        }

        //HUMANOID ALIEN RACES
        public static bool UsingHumanoidAlienFramework()
        {
            return LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Humanoid Alien Races 2.0");
        }

        public static bool IsXenophile(Pawn pawn)
        {
            return pawn.story.traits.allTraits.Any(x => x.def.defName == "Xenophobia" && x.Degree > 0);
        }

        public static bool IsXenophobe(Pawn pawn)
        {
            return pawn.story.traits.allTraits.Any(x => x.def.defName == "Xenophobia" && x.Degree < 0);
        }

        //DUBS BAD HYGIENE       
        public static bool UsingDubsHygiene()
        {
            return LoadedModManager.RunningModsListForReading.Any(x => x.Name == "Dubs Bad Hygiene");
        }

        public static float GetHygieneNeed(Pawn pawn)
        {
            var hygiene = DefDatabase<NeedDef>.AllDefsListForReading.Find(x => x.defName == "Hygiene");
            if (hygiene == null)
            {
                return 1f;
            }

            return pawn.needs.TryGetNeed(hygiene).CurLevelPercentage;
        }
    }
}