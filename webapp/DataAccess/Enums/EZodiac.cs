using K9.DataAccessLayer.Attributes;
using K9.Globalisation;

namespace K9.DataAccessLayer.Enums
{
    public enum EZodiac
    {
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Unspecified)]
        Unspecified,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Aries, Element = EZodiacElement.Fire, 
            FromMonth = 3, FromDay = 21,
            ToMonth = 4, ToDay = 19)]
        Aries,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Taurus, Element = EZodiacElement.Earth,
            FromMonth = 4, FromDay = 20,
            ToMonth = 5, ToDay = 20)]
        Taurus,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Gemini, Element = EZodiacElement.Air,
            FromMonth = 5, FromDay = 21,
            ToMonth = 6, ToDay = 20)]
        Gemini,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Cancer, Element = EZodiacElement.Water,
            FromMonth = 6, FromDay = 21,
            ToMonth = 7, ToDay = 22)]
        Cancer,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Leo, Element = EZodiacElement.Fire,
            FromMonth = 7, FromDay = 23,
            ToMonth = 8, ToDay = 22)]
        Leo,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Virgo, Element = EZodiacElement.Earth,
            FromMonth = 8, FromDay = 23,
            ToMonth = 9, ToDay = 22)]
        Virgo,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Libra, Element = EZodiacElement.Air,
            FromMonth = 9, FromDay = 23,
            ToMonth = 10, ToDay = 22)]
        Libra,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Scorpio, Element = EZodiacElement.Water,
            FromMonth = 10, FromDay = 23,
            ToMonth = 11, ToDay = 21)]
        Scorpio,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Sagittarius, Element = EZodiacElement.Fire,
            FromMonth = 11, FromDay = 22,
            ToMonth = 12, ToDay = 21)]
        Sagittarius,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Capricorn, Element = EZodiacElement.Earth,
            FromMonth = 12, FromDay = 22,
            ToMonth = 1, ToDay = 19)]
        Capricorn,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Aquarius, Element = EZodiacElement.Air,
            FromMonth = 1, FromDay = 20,
            ToMonth = 2, ToDay = 18)]
        Aquarius,
        [ZodiacEnumMetaData(ResourceType = typeof(Dictionary), Name = Strings.Names.Pisces, Element = EZodiacElement.Water,
            FromMonth = 2, FromDay = 19,
            ToMonth = 3, ToDay = 20)]
        Pisces
    }
}
