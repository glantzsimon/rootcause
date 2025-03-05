using K9.Base.DataAccessLayer.Attributes;

namespace K9.DataAccessLayer.Enums
{
    public enum EFoodFrequency
    {
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.AsNeeded)]
        AsNeeded,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.OneToTwoTimesDaily)]
        OneToTwoTimesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TwoToThreeTimesDaily)]
        TwoToThreeTimesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TwoToFourTimesDaily)]
        TwoToFourTimesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.OnwToFiveTimesDaily)]
        OneToFiveTImesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeTimesDaily)]
        ThreeTimesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeOrMoreTimesDaily)]
        ThreeOrMoreTimesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FourToFiveTimesDaily)]
        FourToFiveTimesDaily,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.OncePerWeek)]
        OncePerWeek,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.OneToThreeTimesWeekly)]
        OneToThreeTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.UpToTwoTimesWeekly)]
        UpToTwoTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TwoToThreeTimesWeekly)]
        TwoToThreeTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TwoToFourTimesWeekly)]
        TwoToFourTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeToFourTimesWeekly)]
        ThreeToFourTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TwoToFiveTimesWeekly)]
        TwoToFiveTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeTimesWeekly)]
        ThreeTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FourTimesWeekly)]
        FourTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.AtLeastFourTimesWeekly)]
        AtLeastFourTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeToFiveTimesWeekly)]
        ThreeToFiveTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeToSixTimesWeekly)]
        ThreeToSixTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeToSevenTimesWeekly)]
        ThreeToSevenTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ThreeToNineTimesWeekly)]
        ThreeToNineTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FourToSixTimesWeekly)]
        FourToSixTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FiveToSevenTimesWeekly)]
        FiveToSevenTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.SixToNineTimesWeekly)]
        SixToNineTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.SevenToNineTimesWeekly)]
        SevenToNineTimesWeekly,
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.SevenToTenTimesWeekly)]
        SevenToTenTimesWeekly
    }
}