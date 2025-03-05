using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Attributes;

namespace K9.DataAccessLayer.Enums
{
    public enum EGenoType
    {
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Unspecified)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Name = Globalisation.Strings.Names.Unspecified)]
        Unspecified = 0,

        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Hunter)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Color = "255, 51, 51",
            Name = Globalisation.Strings.Names.Hunter, 
            Strategy = EGenoTypeStrategy.Reactive)]
        Hunter = 1,

        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Gatherer)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Color = "0, 226, 114",
            Name = Globalisation.Strings.Names.Gatherer, 
            Strategy = EGenoTypeStrategy.Thrifty)]
        Gatherer = 2,

        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Teacher)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Color = "51, 153, 255",
            Name = Globalisation.Strings.Names.Teacher, 
            Strategy = EGenoTypeStrategy.Tolerant)]
        Teacher = 3,

        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Explorer)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Color = "102, 102, 255",
            Name = Globalisation.Strings.Names.Explorer, 
            Strategy = EGenoTypeStrategy.Reactive)]
        Explorer = 4,

        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Warrior)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Color = "255, 128, 0",
            Name = Globalisation.Strings.Names.Warrior, 
            Strategy = EGenoTypeStrategy.Tolerant)]
        Warrior = 5,

        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Nomad)]
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), 
            Color = "255, 102, 255",
            Name = Globalisation.Strings.Names.Nomad, 
            Strategy = EGenoTypeStrategy.Tolerant)]
        Nomad = 6
    }
}
