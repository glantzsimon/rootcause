using K9.DataAccessLayer.Attributes;

namespace K9.DataAccessLayer.Enums
{
    public enum EDisplayColor
    {
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "102, 255, 255")]
        Color1,
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "102, 255, 178")]
        Color2,
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "102, 255, 102")]
        Color3,
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "178, 255, 102")]
        Color4,
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "255, 255, 102")]
        Color5,
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "255, 178, 102")]
        Color6,
        [GenoTypeEnumMetaData(ResourceType = typeof(Globalisation.Dictionary), Color = "255, 102, 102")]
        Color7
    }
}
