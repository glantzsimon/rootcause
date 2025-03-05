using K9.SharedLibrary.Extensions;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ECompatibilityEnumMetaDataAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public string GetName()
        {
            return ResourceType.GetValueFromResource(Name);
        }

        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Description);
        }
    }
}