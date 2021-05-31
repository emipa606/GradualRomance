using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Gradual_Romance
{
    public class FlirtStyleDef : Def
    {
        public float acquaitanceFactor;

        /// <summary>
        ///     The more awkward a flirt is, the more likely a pawn will embarrass themselves and/or the recipient. It can also be
        ///     endearing.
        /// </summary>
        public float baseAwkwardness;

        /// <summary>
        ///     Global modifier on base chance
        /// </summary>
        public float baseChance;

        /// <summary>
        ///     Logical flirts are more likely to be accepted if that pawn finds the other pawn socially attractive
        /// </summary>
        public float baseLogic;

        /// <summary>
        ///     The more obscure a flirt style is, the more likely it is not to be noticed.
        /// </summary>
        public float baseObscurity;

        /// <summary>
        ///     The more risky a flirt is, the more likely the recipient pawn will get angry if they reject it. In extreme
        ///     circumstances it might cause a social fight.
        /// </summary>
        public float baseRiskiness;

        /// <summary>
        ///     Romantic flirts are more likely to be accepted if the pawn likes the other pawn.
        /// </summary>
        public float baseRomance;

        /// <summary>
        ///     Sexy flirts are more likely to be accepted if the pawn finds the other pawn physically hot.
        /// </summary>
        public float baseSexiness;

        /// <summary>
        ///     Base chance two pawns become sweethearts after using this flirt, as opposed to lovebuddies.
        /// </summary>
        public float baseSweetheartChance;

        /// <summary>
        ///     Pawns with this trait will never use this flirt style.
        /// </summary>
        public List<TraitDef> cancelingTraits = new List<TraitDef>();

        /// <summary>
        ///     Socially incompetent pawns are much more likely to use this flirt style.
        /// </summary>
        public bool incompetent;

        /// <summary>
        ///     Pawns low in these personality nodes are less likely to use this flirt style
        /// </summary>
        public List<PersonalityNodeModifier> lessLikelyPersonalities = new List<PersonalityNodeModifier>();

        public float loveBuddyFactor;
        public float lovefriendFactor;
        public float loverFactor;

        /// <summary>
        ///     Pawns high in these personality nodes are more likely to use this flirt style
        /// </summary>
        public List<PersonalityNodeModifier> moreLikelyPersonalities = new List<PersonalityNodeModifier>();

        /// <summary>
        ///     Which rule pack the flirt style uses when flirting with a woman.
        /// </summary>
        public RulePackDef rulePackFemale;

        /// <summary>
        ///     Which rule pack the flirt style uses when flirting with a man.
        /// </summary>
        public RulePackDef rulePackMale;

        public float spouseFactor;
        public float sweetheartFactor;

        /// <summary>
        ///     Pawns with this trait are 1.5 times more likely to use this flirt style.
        /// </summary>
        public List<TraitModifier> traitModifiers = new List<TraitModifier>();

        public static FlirtStyleDef Named(string str)
        {
            return DefDatabase<FlirtStyleDef>.GetNamed(str);
        }

        public override int GetHashCode()
        {
            return defName.GetHashCode();
        }
    }
}