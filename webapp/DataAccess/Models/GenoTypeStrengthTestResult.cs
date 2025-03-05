using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using System.ComponentModel.DataAnnotations;

namespace K9.DataAccessLayer.Models
{
    public class GenoTypeStrengthTestResult
    {
        [UIHint("BloodGroup")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.GenoTypeLabel)]
        public EGenoType GenoType { get; set; }
        
        public GenoTypeEnumMetaDataAttribute GenoTypeEnumMetaDataAttribute() =>
            GenoType.GetAttribute<GenoTypeEnumMetaDataAttribute>();

        public string GenoTypeName => GenoTypeEnumMetaDataAttribute().GetDescription();
        
        public string GenoTypeDetailsName => $"{GenoTypeName}Details";
        
        public string Color => GenoTypeEnumMetaDataAttribute().Color;

        public int Count { get; set; }

        public int Max => Count > 20 ? Count : 20;
        
        [UIHint("Strength")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StrengthLabel)]
        public EStrength Strength => GetStrength();

        public EStrength GetStrength()
        {
            if (Count > 20)
            {
                return EStrength.VeryStrong;
            }

            if (Count >= 11)
            {
                return EStrength.Strong;
            }

            if (Count >= 5)
            {
                return EStrength.Positive;
            }

            return EStrength.Negative;
        }
        
    }
}