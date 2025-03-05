using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Extensions;
using System;
using System.Collections.Generic;

namespace K9.DataAccessLayer.Models
{
    public class ZodiacModel
    {
        public EZodiac ZodiacSign { get; set; }
        public EZodiacElement ZodiacElement => ZodiacSign.GetAttribute<ZodiacEnumMetaDataAttribute>().Element;

        public ZodiacModel(DateTime dateOfBirth)
        {
            ZodiacSign = GetZodiacSign(dateOfBirth);
        }

        private EZodiac GetZodiacSign(DateTime dateOfBirth)
        {
            var zodiacs = new List<EZodiac>
            {
                EZodiac.Aries,
                EZodiac.Taurus,
                EZodiac.Gemini,
                EZodiac.Cancer,
                EZodiac.Leo,
                EZodiac.Virgo,
                EZodiac.Libra,
                EZodiac.Scorpio,
                EZodiac.Sagittarius,
                EZodiac.Capricorn,
                EZodiac.Aquarius,
                EZodiac.Pisces
            };

            foreach (var eZodiac in zodiacs)
            {
                var fromMonth = eZodiac.GetAttribute<ZodiacEnumMetaDataAttribute>().FromMonth;
                var toMonth = eZodiac.GetAttribute<ZodiacEnumMetaDataAttribute>().ToMonth;
                var fromDay = eZodiac.GetAttribute<ZodiacEnumMetaDataAttribute>().FromDay;
                var toDay = eZodiac.GetAttribute<ZodiacEnumMetaDataAttribute>().ToDay;

                if (dateOfBirth.Month >= fromMonth &&
                    dateOfBirth.Month <= toMonth &&
                    dateOfBirth.Day >= fromDay &&
                    dateOfBirth.Day <= toDay)
                {
                    return eZodiac;
                };
            }

            return EZodiac.Unspecified;
        }
    }
}
