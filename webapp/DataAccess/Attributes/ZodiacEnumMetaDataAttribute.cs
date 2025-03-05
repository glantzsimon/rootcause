using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Extensions;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ZodiacEnumMetaDataAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        public EZodiacElement Element { get; set; }
        public int FromMonth { get; set; }
        public int FromDay { get; set; }
        public int ToMonth { get; set; }
        public int ToDay { get; set; }
    
        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }
        
        public string GetElement()
        {
            var attr = Element.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }
    }

}