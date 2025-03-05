using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using K9.WebApplication.Attributes;
using K9.WebApplication.Enums;
using K9.WebApplication.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace K9.WebApplication.Models
{
    public enum EGetToTheRootKiColour
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.White)]
        White,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Black)]
        Black,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.BrightGreen)]
        BrightGreen,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Green)]
        Green,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Yellow)]
        Yellow,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Red)]
        Red,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Purple)]
        Purple
    }

    public enum EGetToTheRootKiFamilyMember
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.MiddleSon)]
        MiddleSon,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Mother)]
        Mother,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.EldestSon)]
        EldestSon,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.EldestDaughter)]
        EldestDaughter,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.SeventhChild)]
        SeventhChild,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Father)]
        Father,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.YoungestDaughter)]
        YoungestDaughter,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.YoungestSon)]
        YoungestSon,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.MiddleDaughter)]
        MiddleDaughter
    }

    public enum EGetToTheRootKiDescriptiveName
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Diplomat)]
        Diplomat,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Nurturer)]
        Nurturer,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Pioneer)]
        Pioneer,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Influencer)]
        Influencer,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Hub)]
        Hub,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Leader)]
        Leader,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Advisor)]
        Advisor,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Pragmatist)]
        Pragmatist,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Communicator)]
        Communicator
    }

    public enum EGetToTheRootKiCycleDescriptiveName
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Hibernation)]
        Hibernation,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Planning)]
        Planning,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Sprouting)]
        Sprouting,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Blossoming)]
        Blossoming,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Consolidating)]
        Consolidating,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Ripening)]
        Ripening,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Harvest)]
        Harvest,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Revolution)]
        Revolution,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Spotlight)]
        Spotlight
    }

    public enum EGetToTheRootKiCycle
    {
        Unspecified,
        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.Winter,
            Element = EGetToTheRootKiElement.Water,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Hibernation,
            YearlyDescriptionName = Strings.Names.WaterYear,
            MonthlyDescriptionName = Strings.Names.WaterMonth)]
        Winter,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.WinterToSpring,
            Element = EGetToTheRootKiElement.Earth,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Planning,
            YearlyDescriptionName = Strings.Names.SoilYear,
            MonthlyDescriptionName = Strings.Names.SoilMonth)]
        WinterToSpring,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.EarlySpring,
            Element = EGetToTheRootKiElement.Tree,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Sprouting,
            YearlyDescriptionName = Strings.Names.ThunderYear,
            MonthlyDescriptionName = Strings.Names.ThunderMonth)]
        EarlySpring,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.LateSpring,
            Element = EGetToTheRootKiElement.Tree,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Blossoming,
            YearlyDescriptionName = Strings.Names.WindYear,
            MonthlyDescriptionName = Strings.Names.WindMonth)]
        LateSpring,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.Centre,
            Element = EGetToTheRootKiElement.Earth,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Consolidating,
            YearlyDescriptionName = Strings.Names.CoreEarthYear,
            MonthlyDescriptionName = Strings.Names.CoreEarthMonth)]
        Centre,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.EarlyAutumn,
            Element = EGetToTheRootKiElement.Metal,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Ripening,
            YearlyDescriptionName = Strings.Names.HeavenYear,
            MonthlyDescriptionName = Strings.Names.HeavenMonth)]
        EarlyAutumn,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.LateAutumn,
            Element = EGetToTheRootKiElement.Metal,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Harvest,
            YearlyDescriptionName = Strings.Names.LakeYear,
            MonthlyDescriptionName = Strings.Names.LakeMonth)]
        LateAutumn,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.AutumnToWinter,
            Element = EGetToTheRootKiElement.Earth,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Revolution,
            YearlyDescriptionName = Strings.Names.MountainYear,
            MonthlyDescriptionName = Strings.Names.MountainMonth)]
        AutumnToWinter,

        [GetToTheRootKiCycleEnumMetaData(ResourceType = typeof(Dictionary),
            Season = Strings.Names.Summer,
            Element = EGetToTheRootKiElement.Fire,
            DescriptiveName = EGetToTheRootKiCycleDescriptiveName.Spotlight,
            YearlyDescriptionName = Strings.Names.FireYear,
            MonthlyDescriptionName = Strings.Names.FireMonth)]
        Summer
    }

    public enum EGetToTheRootKiDirection
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Centre,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.North)]
        North,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.NorthWest)]
        NorthWest,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.West)]
        West,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.SouthWest)]
        SouthWest,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.South)]
        South,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.SouthEast)]
        SouthEast,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.East)]
        East,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.NorthEast)]
        NorthEast
    }

    public enum EGetToTheRootKiYinYang
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Unspecified,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Yin)]
        Yin,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Yang)]
        Yang
    }

    public enum EGetToTheRootKiModality
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Unspecified,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Flexible)]
        Flexible,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Stable)]
        Stable,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Dynamic)]
        Dynamic
    }

    public enum EGetToTheRootKiEnergyType
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.MainEnergy)]
        MainEnergy,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.CharacterEnergy)]
        CharacterEnergy,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.SurfaceEnergy)]
        SurfaceEnergy
    }

    public enum EGetToTheRootKiEnergyCycleType
    {
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Unspecified,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.YearlyCycleEnergy)]
        YearlyCycleEnergy,
        [EnumDescription(ResourceType = typeof(Dictionary), Name = Strings.Names.MonthlyCycleEnergy)]
        MonthlyCycleEnergy,
    }

    public enum EGetToTheRootKiEnergy
    {
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Unspecified,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Water, Colour = EGetToTheRootKiColour.White, Element = EGetToTheRootKiElement.Water, Direction = EGetToTheRootKiDirection.North, FamilyMember = EGetToTheRootKiFamilyMember.MiddleSon, YinYang = EGetToTheRootKiYinYang.Yang, TrigramName = "Kan", DescriptiveName = EGetToTheRootKiDescriptiveName.Diplomat, Modality = EGetToTheRootKiModality.Flexible, Cycle = EGetToTheRootKiCycle.Winter)]
        Water,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Soil, Colour = EGetToTheRootKiColour.Black, Element = EGetToTheRootKiElement.Earth, Direction = EGetToTheRootKiDirection.SouthWest, FamilyMember = EGetToTheRootKiFamilyMember.Mother, YinYang = EGetToTheRootKiYinYang.Yin, TrigramName = "Kun", DescriptiveName = EGetToTheRootKiDescriptiveName.Nurturer, Modality = EGetToTheRootKiModality.Stable, Cycle = EGetToTheRootKiCycle.WinterToSpring)]
        Soil,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Thunder, Colour = EGetToTheRootKiColour.BrightGreen, Element = EGetToTheRootKiElement.Tree, Direction = EGetToTheRootKiDirection.East, FamilyMember = EGetToTheRootKiFamilyMember.EldestSon, YinYang = EGetToTheRootKiYinYang.Yang, TrigramName = "Chen", DescriptiveName = EGetToTheRootKiDescriptiveName.Pioneer, Modality = EGetToTheRootKiModality.Dynamic, Cycle = EGetToTheRootKiCycle.EarlySpring)]
        Thunder,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Wind, Colour = EGetToTheRootKiColour.Green, Element = EGetToTheRootKiElement.Tree, Direction = EGetToTheRootKiDirection.SouthEast, FamilyMember = EGetToTheRootKiFamilyMember.EldestDaughter, YinYang = EGetToTheRootKiYinYang.Yin, TrigramName = "Sun", DescriptiveName = EGetToTheRootKiDescriptiveName.Influencer, Modality = EGetToTheRootKiModality.Flexible, Cycle = EGetToTheRootKiCycle.LateSpring)]
        Wind,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.CoreEarth, Colour = EGetToTheRootKiColour.Yellow, Element = EGetToTheRootKiElement.Earth, Direction = EGetToTheRootKiDirection.Centre, FamilyMember = EGetToTheRootKiFamilyMember.SeventhChild, YinYang = EGetToTheRootKiYinYang.Unspecified, TrigramName = "None", DescriptiveName = EGetToTheRootKiDescriptiveName.Hub, Modality = EGetToTheRootKiModality.Stable, Cycle = EGetToTheRootKiCycle.Centre)]
        CoreEarth,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Heaven, Colour = EGetToTheRootKiColour.White, Element = EGetToTheRootKiElement.Metal, Direction = EGetToTheRootKiDirection.NorthWest, FamilyMember = EGetToTheRootKiFamilyMember.Father, YinYang = EGetToTheRootKiYinYang.Yang, TrigramName = "Chien", DescriptiveName = EGetToTheRootKiDescriptiveName.Leader, Modality = EGetToTheRootKiModality.Dynamic, Cycle = EGetToTheRootKiCycle.EarlyAutumn)]
        Heaven,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Lake, Colour = EGetToTheRootKiColour.Red, Element = EGetToTheRootKiElement.Metal, Direction = EGetToTheRootKiDirection.West, FamilyMember = EGetToTheRootKiFamilyMember.YoungestDaughter, YinYang = EGetToTheRootKiYinYang.Yin, TrigramName = "Tui", DescriptiveName = EGetToTheRootKiDescriptiveName.Advisor, Modality = EGetToTheRootKiModality.Flexible, Cycle = EGetToTheRootKiCycle.LateAutumn)]
        Lake,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Mountain, Colour = EGetToTheRootKiColour.White, Element = EGetToTheRootKiElement.Earth, Direction = EGetToTheRootKiDirection.NorthEast, FamilyMember = EGetToTheRootKiFamilyMember.YoungestSon, YinYang = EGetToTheRootKiYinYang.Yang, TrigramName = "Ken", DescriptiveName = EGetToTheRootKiDescriptiveName.Pragmatist, Modality = EGetToTheRootKiModality.Stable, Cycle = EGetToTheRootKiCycle.AutumnToWinter)]
        Mountain,
        [GetToTheRootKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Fire, Colour = EGetToTheRootKiColour.Purple, Element = EGetToTheRootKiElement.Fire, Direction = EGetToTheRootKiDirection.South, FamilyMember = EGetToTheRootKiFamilyMember.MiddleDaughter, YinYang = EGetToTheRootKiYinYang.Yin, TrigramName = "Li", DescriptiveName = EGetToTheRootKiDescriptiveName.Communicator, Modality = EGetToTheRootKiModality.Dynamic, Cycle = EGetToTheRootKiCycle.Summer)]
        Fire
    }

    public class NineStarKiEnergy
    {
        public NineStarKiEnergy(EGetToTheRootKiEnergy energy, EGetToTheRootKiEnergyType type, bool isAdult = true, EGetToTheRootKiEnergyCycleType energyCycleType = EGetToTheRootKiEnergyCycleType.Unspecified)
        {
            Energy = energy;
            EnergyType = type;
            IsAdult = isAdult;
            EnergyCycleType = energyCycleType;
        }
        
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EnergyLabel)]
        public string EnergyName => MetaData.GetDescription();

        public int EnergyNumber => (int)Energy;
        
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ElementLabel)]
        public string ElementName => MetaData.GetElement();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ElementLabel)]
        public string ElementDescription => MetaData.GetElementDescription();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ElementLabel)]
        public string ElementTitle => $"{ElementName} {Dictionary.Element}";
        
        public string Season => CycleMetaData.Season;
        
        public string SeasonDescription => CycleMetaData.SeasonDescription;
        
        public string CycleDescription => EnergyType == EGetToTheRootKiEnergyType.MainEnergy ? CycleMetaData.YearlyDescription : CycleMetaData.MonthlyDescription;
        
        public string YinYangName => YinYang == EGetToTheRootKiYinYang.Unspecified ? string.Empty : YinYang.ToString();

        [ScriptIgnore] public EGetToTheRootKiEnergy Energy { get; }
        [ScriptIgnore] public EGetToTheRootKiEnergy RelatedEnergy { get; set; }
        [ScriptIgnore] public EGender Gender { get; set; }
        [ScriptIgnore] public bool IsAdult { get; set; }
        [ScriptIgnore] public EGetToTheRootKiEnergyType EnergyType { get; }
        [ScriptIgnore] public EGetToTheRootKiEnergyCycleType EnergyCycleType { get; set; }
       
        [ScriptIgnore]
        public EGetToTheRootKiYinYang YinYang => GetYinYang();

        [ScriptIgnore]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ElementLabel)]
        public EGetToTheRootKiElement Element => MetaData.Element;

        [ScriptIgnore]
        public string ElementWithYingYang => $"{YinYangName} {ElementName}".Trim();
        
        [ScriptIgnore]
        public string CycleDescriptiveName => CycleMetaData.DescriptiveTitle;

        private GetToTheRootKiEnumMetaDataAttribute _metaData;

        [ScriptIgnore]
        public GetToTheRootKiEnumMetaDataAttribute MetaData
        {
            get
            {
                if (_metaData == null)
                {
                    _metaData = Energy.GetAttribute<GetToTheRootKiEnumMetaDataAttribute>();
                }
                return _metaData;
            }
        }
        
        private GetToTheRootKiCycleEnumMetaDataAttribute _cycleMetaData;
        internal GetToTheRootKiCycleEnumMetaDataAttribute CycleMetaData
        {
            get
            {
                if (_cycleMetaData == null)
                {
                    _cycleMetaData = MetaData.Cycle.GetAttribute<GetToTheRootKiCycleEnumMetaDataAttribute>();
                }
                return _cycleMetaData;
            }
        }

        private EGetToTheRootKiYinYang GetYinYang()
        {
            if (Energy == EGetToTheRootKiEnergy.CoreEarth && RelatedEnergy == EGetToTheRootKiEnergy.CoreEarth)
            {
                return Gender.IsYin() ? EGetToTheRootKiYinYang.Yin : EGetToTheRootKiYinYang.Yang;
            }
            if (Energy == EGetToTheRootKiEnergy.CoreEarth && RelatedEnergy != EGetToTheRootKiEnergy.Unspecified)
            {
                return RelatedEnergy.GetAttribute<GetToTheRootKiEnumMetaDataAttribute>().YinYang;
            }
            return MetaData.YinYang;
        }
    }
}