using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Extensions;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DoshaEnumMetaDataAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        public EDosha Dosha { get; set; }

        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }

        public string GetDosha()
        {
            var attr = Dosha.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }
    }

}