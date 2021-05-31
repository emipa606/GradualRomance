using System.Collections.Generic;
using System.Linq;
using HugsLib;
using HugsLib.Settings;
using Psychology;
using RimWorld;
using UnityEngine;
using Verse;

namespace Gradual_Romance
{
    public class GradualRomanceMod : ModBase
    {
        public enum AttractionCalculationSetting
        {
            Vanilla,
            Simplified,
            Complex
        }

        public enum ExtraspeciesRomanceSetting
        {
            NoXenoRomance,
            OnlyXenophiles,
            OnlyXenophobesNever,
            CaptainKirk
        }

        public enum GenderModeSetting
        {
            Vanilla,
            None,
            Inverse
        }

        public enum KinseyDescriptor
        {
            ExclusivelyHeterosexual,
            MostlyHeterosexual,
            LeansHeterosexual,
            Bisexual,
            LeansHomosexual,
            MostlyHomosexual,
            ExclusivelyHomosexual
        }

        public enum SeductionModeSetting
        {
            NoRestriction,
            RelationshipAndNonColonists,
            OnlyRelationship,
            NoSeduction
        }

        //Settings to change the default gender changes
        private static readonly Gender alteredGender = Gender.Female;
        public static GenderModeSetting genderMode;
        public static AttractionCalculationSetting AttractionCalculation = AttractionCalculationSetting.Complex;
        public static readonly SeductionModeSetting SeductionMode = SeductionModeSetting.OnlyRelationship;

        private static int baseRomanceChance;
        private static int baseBreakupChance;
        private static int baseFlirtChance;

        private static int romanticSuccessRate;

        private static int baseSeductionChance;

        private static int minAttractionForSeduction;

        private static int sweetheartRate;

        private static int decayRate;

        public static KinseyDescriptor averageKinseyFemale;
        public static KinseyDescriptor averageKinseyMale;

        public static ExtraspeciesRomanceSetting extraspeciesRomance;

        public static bool detailedAttractionLogs;

        public static bool rerollBeautyTraits;

        public static bool polygamousWorld;

        public static int numberOfRelationships;

        public static bool informalRomanceLetters;

        //public static bool useFacialAttractiveness;
        public static bool detailedDebugLogs;


        //CHECK MODS
        /*
        public static bool UsingDubsBadHygiene()
        {
            try
            {
                if (LoadedModManager.RunningModsListForReading.Any(x=> x.Name == "Dubs Bad Hygiene"))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (TypeLoadException ex) { return false; }
        }
        */

        private static readonly Dictionary<ThingDef, SimpleCurve> maleSexDriveCurves =
            new Dictionary<ThingDef, SimpleCurve>();

        private static readonly Dictionary<ThingDef, SimpleCurve> femaleSexDriveCurves =
            new Dictionary<ThingDef, SimpleCurve>();

        private static readonly Dictionary<ThingDef, SimpleCurve> maleMaturityCurves =
            new Dictionary<ThingDef, SimpleCurve>();

        private static readonly Dictionary<ThingDef, SimpleCurve> femaleMaturityCurves =
            new Dictionary<ThingDef, SimpleCurve>();

        private SettingHandle<AttractionCalculationSetting> settingAttractionCalculation;
        private SettingHandle<KinseyDescriptor> settingAverageKinseyFemale;
        private SettingHandle<KinseyDescriptor> settingAverageKinseyMale;
        private SettingHandle<int> settingBaseBreakupChance;
        private SettingHandle<int> settingBaseFlirtChance;
        private SettingHandle<int> settingBaseRomanceChance;
        private SettingHandle<int> settingBaseSeductionChance;
        private SettingHandle<int> settingDecayRate;
        private SettingHandle<bool> settingDetailedAttractionLogs;
        private SettingHandle<bool> settingDetailedDebugLogs;
        private SettingHandle<ExtraspeciesRomanceSetting> settingExtraspeciesRomance;
        private SettingHandle<GenderModeSetting> settingGenderMode;
        private SettingHandle<bool> settingInformalRomanceLetters;
        private SettingHandle<int> settingMinAttractionForSeduction;
        private SettingHandle<int> settingNumberOfRelationships;
        private SettingHandle<bool> settingPolygamousWorld;
        private SettingHandle<bool> settingRerollBeautyTraits;
        private SettingHandle<int> settingRomanticSuccessRate;
        private SettingHandle<SeductionModeSetting> settingSeductionMode;
        private SettingHandle<int> settingSweetheartRate;

        public override string ModIdentifier => "GradualRomance";

        public static float BaseRomanceChance => Mathf.Max(0f, baseRomanceChance) * 0.01f;

        public static float BaseBreakupChance => Mathf.Max(0f, baseBreakupChance) * 0.01f;

        public static float BaseFlirtChance => Mathf.Max(0f, baseFlirtChance) * 0.01f;

        public static float RomanticSuccessRate => Mathf.Max(0f, romanticSuccessRate) * 0.01f;

        public static float BaseSeductionChance => Mathf.Max(0f, baseSeductionChance) * 0.01f;

        public static float MinAttractionForSeduction => Mathf.Max(0f, minAttractionForSeduction) * 0.01f;

        public static float SweetheartRate => Mathf.Max(0f, sweetheartRate) * 0.01f;

        public static float DecayRate => Mathf.Clamp01(decayRate * 0.01f);

        public static Gender AlteredGender()
        {
            return alteredGender;
        }

        public override void SettingsChanged()
        {
            AttractionCalculation = settingAttractionCalculation.Value;
            genderMode = settingGenderMode.Value;
            averageKinseyFemale = settingAverageKinseyFemale.Value;
            extraspeciesRomance = settingExtraspeciesRomance.Value;
            averageKinseyMale = settingAverageKinseyMale.Value;
            baseRomanceChance = settingBaseRomanceChance.Value;
            baseBreakupChance = settingBaseBreakupChance.Value;
            baseFlirtChance = settingBaseFlirtChance.Value;
            romanticSuccessRate = settingRomanticSuccessRate.Value;
            decayRate = settingDecayRate.Value;
            numberOfRelationships = settingNumberOfRelationships.Value;
            polygamousWorld = settingPolygamousWorld.Value;
            rerollBeautyTraits = settingRerollBeautyTraits.Value;
            informalRomanceLetters = settingInformalRomanceLetters.Value;
            detailedDebugLogs = settingDetailedDebugLogs.Value;
            detailedAttractionLogs = settingDetailedAttractionLogs.Value;
        }

        public override void DefsLoaded()
        {
            settingAttractionCalculation = Settings.GetHandle("GRAttractionCalculationSetting",
                "AttractionCalculationSetting_title".Translate(), "AttractionCalculationSetting_desc".Translate(),
                AttractionCalculationSetting.Complex, null, "AttractionCalculationSetting_");
            settingGenderMode = Settings.GetHandle("GRGenderModeSetting", "GenderModeSetting_title".Translate(),
                "GenderModeSetting_desc".Translate(), GenderModeSetting.Vanilla, null, "GenderModeSetting_");
            settingAverageKinseyFemale = Settings.GetHandle("GRaverageKinseyFemale",
                "AverageKinseyFemale_title".Translate(), "AverageKinseyFemale_desc".Translate(),
                KinseyDescriptor.ExclusivelyHeterosexual, null, "KinseyDescriptor_");
            settingAverageKinseyMale = Settings.GetHandle("GRaverageKinseyMale", "AverageKinseyMale_title".Translate(),
                "AverageKinseyMale_desc".Translate(), KinseyDescriptor.ExclusivelyHeterosexual, null,
                "KinseyDescriptor_");
            settingSeductionMode = Settings.GetHandle("GRseductionModeSetting",
                "SeductionModeSetting_title".Translate(), "SeductionModeSetting_desc".Translate(),
                SeductionModeSetting.OnlyRelationship, null, "SeductionModeDescriptor_");
            settingExtraspeciesRomance = Settings.GetHandle("GRextraspeciesRomance",
                "ExtraspeciesRomance_title".Translate(), "ExtraspeciesRomance_desc".Translate(),
                ExtraspeciesRomanceSetting.OnlyXenophobesNever, null, "ExtraspeciesRomanceDescriptor_");
            settingBaseRomanceChance = Settings.GetHandle("GRbaseRomanceChance", "BaseRomanceChance_title".Translate(),
                "BaseRomanceChance_desc".Translate(), 100);
            settingBaseBreakupChance = Settings.GetHandle("GRbaseBreakupChance", "BaseBreakupChance_title".Translate(),
                "BaseBreakupChance_desc".Translate(), 100);
            settingBaseFlirtChance = Settings.GetHandle("GRbaseFlirtChance", "BaseFlirtChance_title".Translate(),
                "BaseFlirtChance_desc".Translate(), 100);
            settingBaseSeductionChance = Settings.GetHandle("GRbaseSeductionChance",
                "BaseSeductionChance_title".Translate(), "BaseSeductionChance_desc".Translate(), 100);
            settingRomanticSuccessRate = Settings.GetHandle("GRromanticSuccessRate",
                "RomanceSuccessRate_title".Translate(), "RomanceSuccessRate_desc".Translate(), 100);
            settingDecayRate = Settings.GetHandle("GRdecayRate", "DecayRate_title".Translate(),
                "DecayRate_desc".Translate(), 25);
            settingMinAttractionForSeduction = Settings.GetHandle("GRminAttractionForSeduction",
                "MinAttraction_title".Translate(), "MinAttraction_desc".Translate(), 90);
            settingNumberOfRelationships = Settings.GetHandle("GRnumberOfRelationships",
                "NumberOfRelationships_title".Translate(), "NumberOfRelationships_desc".Translate(), 3);
            settingPolygamousWorld = Settings.GetHandle<bool>("GRpolygamousWorld", "PolygamousWorld_title".Translate(),
                "PolygamousWorld_desc".Translate());
            settingRerollBeautyTraits = Settings.GetHandle<bool>("GRrerollBeautyTraits",
                "RerollBeautyTraits_title".Translate(), "RerollBeautyTraits_desc".Translate());
            settingInformalRomanceLetters = Settings.GetHandle("GRinformalRomanceLetters",
                "InformalLetters_title".Translate(), "InformalLetters_desc".Translate(), true);
            //useFacialAttractiveness = Settings.GetHandle<bool>("GRuseFacialAttractiveness", "UseFacialAttractiveness_title".Translate(), "UseFacialAttractiveness_desc".Translate(), false);
            settingDetailedDebugLogs = Settings.GetHandle<bool>("GRdetailedDebugLogs",
                "DetailedDebugLog_title".Translate(),
                "DetailedDebugLog_desc".Translate());
            settingDetailedAttractionLogs = Settings.GetHandle<bool>("GRdetailedAttractionLogs",
                "DetailedAttractionLog_title".Translate(), "DetailedAttractionLog_desc".Translate());

            AttractionCalculation = settingAttractionCalculation.Value;
            genderMode = settingGenderMode.Value;
            averageKinseyFemale = settingAverageKinseyFemale.Value;
            extraspeciesRomance = settingExtraspeciesRomance.Value;
            averageKinseyMale = settingAverageKinseyMale.Value;
            baseRomanceChance = settingBaseRomanceChance.Value;
            baseBreakupChance = settingBaseBreakupChance.Value;
            baseFlirtChance = settingBaseFlirtChance.Value;
            baseSeductionChance = settingBaseSeductionChance.Value;
            romanticSuccessRate = settingRomanticSuccessRate.Value;
            decayRate = settingDecayRate.Value;
            numberOfRelationships = settingNumberOfRelationships.Value;
            polygamousWorld = settingPolygamousWorld.Value;
            rerollBeautyTraits = settingRerollBeautyTraits.Value;
            informalRomanceLetters = settingInformalRomanceLetters.Value;
            detailedDebugLogs = settingDetailedDebugLogs.Value;
            detailedAttractionLogs = settingDetailedAttractionLogs.Value;


            var allDefsListForReading = DefDatabase<FlirtStyleDef>.AllDefsListForReading;
            Logger.Message("Gradual Romance loaded with " + allDefsListForReading.Count + " flirt styles.");

            //Only give GRPawnComp to pawns that have a psychecomp
            foreach (var def in DefDatabase<ThingDef>.AllDefs)
            {
                var addComp = false;
                if (def.comps == null)
                {
                    continue;
                }

                foreach (var compProperties in def.comps)
                {
                    if (compProperties is not CompProperties_Psychology)
                    {
                        continue;
                    }

                    addComp = true;
                    break;
                }

                if (!addComp)
                {
                    continue;
                }

                def.comps.Add(new GRPawnComp_Properties());
                if (def.modExtensions == null)
                {
                    def.modExtensions = new List<DefModExtension>();
                }

                if (!def.HasModExtension<XenoRomanceExtension>())
                {
                    Logger.Message("Autopatching " + def.defName);
                    def.modExtensions.Add(CreateXenoRomanceExtensionFor(def));
                }

                maleSexDriveCurves.Add(def,
                    CreateCurveFrom(def.GetModExtension<XenoRomanceExtension>().sexDriveByAgeCurveMale));
                femaleSexDriveCurves.Add(def,
                    CreateCurveFrom(def.GetModExtension<XenoRomanceExtension>().sexDriveByAgeCurveFemale));
                maleMaturityCurves.Add(def,
                    CreateCurveFrom(def.GetModExtension<XenoRomanceExtension>().maturityByAgeCurveMale));
                femaleMaturityCurves.Add(def,
                    CreateCurveFrom(def.GetModExtension<XenoRomanceExtension>().maturityByAgeCurveFemale));
            }

            //Patches
        }

        private static XenoRomanceExtension CreateXenoRomanceExtensionFor(ThingDef pawnType)
        {
            var xenoRomance = new XenoRomanceExtension();
            var maxAge = pawnType.race.lifeExpectancy;
            //xenoRomance.youngAdultAge = (maxAge * 0.15f);
            //xenoRomance.midlifeAge = (maxAge * 0.5f); 
            //xenoRomance.subspeciesOf = null;
            xenoRomance.extraspeciesAppeal = 0.5f;
            var lifeStageDefs = pawnType.race.lifeStageAges;
            var reproductiveStart = 0f;
            foreach (var lifeStageAge in lifeStageDefs)
            {
                if (lifeStageAge.def.reproductive)
                {
                    reproductiveStart = lifeStageAge.minAge;
                }
            }

            xenoRomance.maturityByAgeCurveFemale.Add(new Vector2(0f, 0.01f));
            xenoRomance.maturityByAgeCurveFemale.Add(new Vector2(reproductiveStart, 1f));
            xenoRomance.maturityByAgeCurveFemale.Add(new Vector2(Mathf.Lerp(reproductiveStart, maxAge, 0.5f), 2f));
            xenoRomance.maturityByAgeCurveFemale.Add(new Vector2(maxAge, 3f));
            xenoRomance.maturityByAgeCurveMale.Add(new Vector2(0f, 0.01f));
            xenoRomance.maturityByAgeCurveMale.Add(new Vector2(reproductiveStart, 1f));
            xenoRomance.maturityByAgeCurveMale.Add(new Vector2(Mathf.Lerp(reproductiveStart, maxAge, 0.5f), 2f));
            xenoRomance.maturityByAgeCurveMale.Add(new Vector2(maxAge, 3f));
            xenoRomance.sexDriveByAgeCurveFemale.Add(new Vector2(reproductiveStart * 0.7f, 0f));
            xenoRomance.sexDriveByAgeCurveFemale.Add(new Vector2(reproductiveStart, 3f));
            xenoRomance.sexDriveByAgeCurveFemale.Add(new Vector2(Mathf.Lerp(reproductiveStart, maxAge, 0.5f), 1f));
            xenoRomance.sexDriveByAgeCurveFemale.Add(new Vector2(maxAge, 0.1f));
            xenoRomance.sexDriveByAgeCurveMale.Add(new Vector2(reproductiveStart * 0.7f, 0f));
            xenoRomance.sexDriveByAgeCurveMale.Add(new Vector2(reproductiveStart, 3f));
            xenoRomance.sexDriveByAgeCurveMale.Add(new Vector2(Mathf.Lerp(reproductiveStart, maxAge, 0.5f), 1f));
            xenoRomance.sexDriveByAgeCurveMale.Add(new Vector2(maxAge, 0.1f));
            return xenoRomance;
        }

        private static SimpleCurve CreateCurveFrom(List<Vector2> curvePoints)
        {
            var curve = new SimpleCurve();
            foreach (var vector2 in curvePoints)
            {
                curve.Add(new CurvePoint(vector2));
            }

            return curve;
        }

        public static SimpleCurve GetMaturityCurveFor(Pawn pawn)
        {
            var def = pawn.def;
            var gender = pawn.gender;
            if (gender == Gender.Female)
            {
                if (femaleMaturityCurves.Keys.Contains(def))
                {
                    return femaleMaturityCurves[def];
                }
            }

            if (gender != Gender.Male)
            {
                return null;
            }

            if (maleMaturityCurves.Keys.Contains(def))
            {
                return maleMaturityCurves[def];
            }

            return null;
        }

        public static SimpleCurve GetSexDriveCurveFor(Pawn pawn)
        {
            var def = pawn.def;
            var gender = pawn.gender;
            if (gender == Gender.Female)
            {
                if (femaleSexDriveCurves.Keys.Contains(def))
                {
                    return femaleSexDriveCurves[def];
                }
            }

            if (gender != Gender.Male)
            {
                return null;
            }

            if (maleSexDriveCurves.Keys.Contains(def))
            {
                return maleSexDriveCurves[def];
            }

            return null;
        }

        //ERROR MESSAGES
        public void Error_TriedDecayNullRelationship(Pawn pawn, Pawn other, PawnRelationDef pawnRelationDef)
        {
            Logger.Error("Tried to decay a null relationship {2} between {0} and {1}".Formatted(pawn.Name.ToStringShort,
                other.Name.ToStringShort, pawnRelationDef.defName));
        }
    }
}