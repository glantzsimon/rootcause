using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Extensions;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ENineStarKiElementEnumMetaDataAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        public ENineStarKiElement Element { get; set; }
        public ESeason Season { get; set; }
        public EOrgan StrongYinOrgans { get; set; }
        public EOrgan StrongYangOrgans { get; set; }
        public EOrgan[] WeakYinOrgans { get; set; }
        public EOrgan[] WeakYangOrgans { get; set; }
    
        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }

        public string GetElement()
        {
            return Element.GetAttribute<EnumDescriptionAttribute>().GetDescription();
        }
    }
}