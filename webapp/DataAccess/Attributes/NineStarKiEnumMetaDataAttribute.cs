using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Extensions;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NineStarKiEnumMetaDataAttribute : Attribute
    {
        public Type ResourceType { get; set; }
        public string Name { get; set; }
        public ENineStarKiElement Element { get; set; }
        public EOrgan StrongYinOrgans => GetStrongYinOrgans();
        public EOrgan StrongYangOrgans => GetStrongYangOrgans();
        public EOrgan[] WeakYinOrgans => GetWeakYinOrgans();
        public EOrgan[] WeakYangOrgans => GetWeakYangOrgans();

        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }
        
        public string GetElement()
        {
            var attr = Element.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public EOrgan GetStrongYinOrgans()
        {
            return Element.GetAttribute<ENineStarKiElementEnumMetaDataAttribute>().StrongYinOrgans;
        }

        public EOrgan GetStrongYangOrgans()
        {
            return Element.GetAttribute<ENineStarKiElementEnumMetaDataAttribute>().StrongYangOrgans;
        }

        public EOrgan[] GetWeakYinOrgans()
        {
            return Element.GetAttribute<ENineStarKiElementEnumMetaDataAttribute>().WeakYinOrgans;
        }

        public EOrgan[] GetWeakYangOrgans()
        {
            return Element.GetAttribute<ENineStarKiElementEnumMetaDataAttribute>().WeakYangOrgans;
        }
        
        private string GetEnergytNumberAndName(ENineStarKiEnergy energy)
        {
            return $"{(int)energy} {energy.GetAttribute<NineStarKiEnumMetaDataAttribute>().Name}";
        }
    }

}