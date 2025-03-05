using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Extensions;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EFoodGroupMetaDataAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        public EFoodGroup FoodGroup { get; set; }
        
        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }
    }
}