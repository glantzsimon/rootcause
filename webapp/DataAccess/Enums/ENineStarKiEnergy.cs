using K9.DataAccessLayer.Attributes;
using K9.Globalisation;

namespace K9.DataAccessLayer.Enums
{
    public enum ENineStarKiEnergy
    {
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Unspecified,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Water,
            Element = ENineStarKiElement.Water)]
        Water,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Soil,
            Element = ENineStarKiElement.Earth)]
        Soil,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Thunder,
            Element = ENineStarKiElement.Tree)]
        Thunder,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Wind,
            Element = ENineStarKiElement.Tree)]
        Wind,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.CoreEarth,
            Element = ENineStarKiElement.Earth)]
        CoreEarth,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Heaven,
            Element = ENineStarKiElement.Metal)]
        Heaven,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Lake,
            Element = ENineStarKiElement.Metal)]
        Lake,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Mountain,
            Element = ENineStarKiElement.Earth)]
        Mountain,
        [NineStarKiEnumMetaData(ResourceType = typeof(Dictionary),
            Name = Strings.Names.Fire,
            Element = ENineStarKiElement.Fire)]
        Fire
    }
}
