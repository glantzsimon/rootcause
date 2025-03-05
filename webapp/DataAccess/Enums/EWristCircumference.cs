using K9.Base.DataAccessLayer.Attributes;

namespace K9.DataAccessLayer.Enums
{
    public enum EWristCircumference
    {
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.MiddleFingerAndThumbDoNotTouch)]
        DoNotTouch = 1,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.MiddleFingerAndThumbJustTouch)]
        JustTouch,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.MiddleFingerAndThumbOverlap)]
        Overlap
    }
}
