using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Attributes;

namespace K9.DataAccessLayer.Enums
{
    public enum ECompatibilityLevel
    {
        Unspecified = -1,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Neutral)]
        [ECompatibilityEnumMetaData(
            ResourceType = typeof(Globalisation.Dictionary), 
            Name = Globalisation.Strings.Names.Neutral, 
            Color = "#f9f607",
            Description = Globalisation.Strings.Names.NeutralDescription)]
        Neutral,
        [ECompatibilityEnumMetaData(
            ResourceType = typeof(Globalisation.Dictionary), 
            Name = Globalisation.Strings.Names.Excellent, 
            Color = "#1de4b8",
            Description = Globalisation.Strings.Names.ExcellentDescription)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Excellent)]
        Excellent,
        [ECompatibilityEnumMetaData(
            ResourceType = typeof(Globalisation.Dictionary), 
            Name = Globalisation.Strings.Names.Optimal, 
            Color = "#1bb9fc",
            Description = Globalisation.Strings.Names.OptimalDescription)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Optimal)]
        Optimal,
        [ECompatibilityEnumMetaData(
            ResourceType = typeof(Globalisation.Dictionary), 
            Name = Globalisation.Strings.Names.Suboptimal, 
            Color = "#dbbd1c",
            Description = Globalisation.Strings.Names.SuboptimalDescription)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Suboptimal)]
        Suboptimal,
        [ECompatibilityEnumMetaData(
            ResourceType = typeof(Globalisation.Dictionary), 
            Name = Globalisation.Strings.Names.Unsuitable, 
            Color = "#e6530b",
            Description = Globalisation.Strings.Names.UnsuitableDescription)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Unsuitable)]
        Unsuitable
    }
}
