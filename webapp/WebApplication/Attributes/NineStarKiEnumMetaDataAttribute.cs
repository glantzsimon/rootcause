using K9.Base.DataAccessLayer.Attributes;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.WebApplication.Enums;
using K9.WebApplication.Models;
using System;
using System.Collections.Generic;

namespace K9.WebApplication.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GetToTheRootKiEnumMetaDataAttribute : Attribute
    {
        public EGetToTheRootKiFamilyMember FamilyMember { get; set; }
        public EGetToTheRootKiElement Element { get; set; }
        public EGetToTheRootKiColour Colour { get; set; }
        public EGetToTheRootKiDirection Direction { get; set; }
        public EGetToTheRootKiYinYang YinYang { get; set; }
        public EGetToTheRootKiDescriptiveName DescriptiveName { get; set; }
        public EGetToTheRootKiModality Modality { get; set; }
        public EGetToTheRootKiCycle Cycle { get; set; }
        public EOrgan StrongYinOrgans => GetStrongYinOrgans();
        public EOrgan StrongYangOrgans => GetStrongYangOrgans();
        public EOrgan[] WeakYinOrgans => GetWeakYinOrgans();
        public EOrgan[] WeakYangOrgans => GetWeakYangOrgans();

        public Type ResourceType { get; set; }
        public string TrigramName { get; set; }
        public string Name { get; set; }

        public string ModalityDescription
        {
            get { return GetModalityDescription(); }
        }

        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }

        public string GetDescriptiveTitle()
        {
            return GetEnumDescription(DescriptiveName);
        }

        public string GetYinYang()
        {
            return GetEnumDescription(YinYang);
        }

        public string GetFamilyMember()
        {
            return GetEnumDescription(FamilyMember);
        }

        public string GetElement()
        {
            return GetEnumDescription(Element);
        }

        public string GetColour()
        {
            return GetEnumDescription(Colour);
        }

        public string GetDirection()
        {
            return GetEnumDescription(Direction);
        }

        public string GetTrigram()
        {
            return ResourceType.GetValueFromResource(TrigramName);
        }

        private EOrgan GetStrongYinOrgans()
        {
            return Element.GetAttribute<EGetToTheRootKiElementEnumMetaDataAttribute>().StrongYinOrgans;
        }

        private EOrgan GetStrongYangOrgans()
        {
            return Element.GetAttribute<EGetToTheRootKiElementEnumMetaDataAttribute>().StrongYangOrgans;
        }

        private EOrgan[] GetWeakYinOrgans()
        {
            return Element.GetAttribute<EGetToTheRootKiElementEnumMetaDataAttribute>().WeakYinOrgans;
        }

        private EOrgan[] GetWeakYangOrgans()
        {
            return Element.GetAttribute<EGetToTheRootKiElementEnumMetaDataAttribute>().WeakYangOrgans;
        }

        /// <summary>
        /// Generic method to get the description of any enum with EnumDescriptionAttribute.
        /// </summary>
        private static string GetEnumDescription<TEnum>(TEnum value) where TEnum : Enum
        {
            var attr = value.GetAttribute<EnumDescriptionAttribute>();
            return attr != null ? attr.GetDescription() : value.ToString();
        }

        /// <summary>
        /// Dictionary for fast lookup of element descriptions.
        /// </summary>
        private static readonly Dictionary<EGetToTheRootKiElement, string> _elementDescriptions = new Dictionary<EGetToTheRootKiElement, string>
        {
            { EGetToTheRootKiElement.Earth, Dictionary.earth_element },
            { EGetToTheRootKiElement.Fire, Dictionary.fire_element },
            { EGetToTheRootKiElement.Metal, Dictionary.metal_element },
            { EGetToTheRootKiElement.Tree, Dictionary.tree_element },
            { EGetToTheRootKiElement.Water, Dictionary.water_element }
        };

        public string GetElementDescription()
        {
            return _elementDescriptions.TryGetValue(Element, out var desc) ? desc : string.Empty;
        }

        /// <summary>
        /// Returns the energy number and name in a formatted string.
        /// </summary>
        private string GetEnergyNumberAndName(EGetToTheRootKiEnergy energy)
        {
            var attr = energy.GetAttribute<GetToTheRootKiEnumMetaDataAttribute>();
            return string.Format("{0} {1}", (int)energy, attr != null ? attr.Name : energy.ToString());
        }

        /// <summary>
        /// Dictionary for fast lookup of modality descriptions.
        /// </summary>
        private static readonly Dictionary<EGetToTheRootKiModality, string> _modalityDescriptions = new Dictionary<EGetToTheRootKiModality, string>
        {
            { EGetToTheRootKiModality.Dynamic, Dictionary.dynamic_modality },
            { EGetToTheRootKiModality.Stable, Dictionary.stable_modality },
            { EGetToTheRootKiModality.Flexible, Dictionary.flexible_modality }
        };

        /// <summary>
        /// Returns the description of the modality with formatted energy names.
        /// </summary>
        private string GetModalityDescription()
        {
            if (!_modalityDescriptions.TryGetValue(Modality, out var modalityText))
            {
                modalityText = string.Empty;
            }

            return TemplateParser.Parse(
                modalityText,
                new
                {
                    water = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Water),
                    soil = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Soil),
                    thunder = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Thunder),
                    wind = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Wind),
                    coreearth = GetEnergyNumberAndName(EGetToTheRootKiEnergy.CoreEarth),
                    heaven = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Heaven),
                    lake = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Lake),
                    mountain = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Mountain),
                    fire = GetEnergyNumberAndName(EGetToTheRootKiEnergy.Fire)
                }
            );
        }
    }
}
