﻿using K9.Base.DataAccessLayer.Attributes;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.WebApplication.Models;
using System;

namespace K9.WebApplication.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NineStarKiEnumMetaDataAttribute : Attribute
    {
        public ENineStarKiFamilyMember FamilyMember { get; set; }
        public ENineStarKiElement Element { get; set; }
        public ENineStarKiColour Colour { get; set; }
        public ENineStarKiDirection Direction { get; set; }
        public ENineStarKiYinYang YinYang { get; set; }
        public ENineStarKiDescriptiveName DescriptiveName { get; set; }
        public ENineStarKiModality Modality { get; set; }
        public ENineStarKiCycle Cycle { get; set; }
        public Type ResourceType { get; set; }
        public string TrigramName { get; set; }
        public string Name { get; set; }

        public string ModalityDeescription => GetModalityDescription();

        public string GetDescription()
        {
            return ResourceType.GetValueFromResource(Name);
        }

        public string GetDescriptiveTitle()
        {
            var attr = DescriptiveName.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public string GetYinYang()
        {
            var attr = YinYang.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public string GetFamilyMember()
        {
            var attr = FamilyMember.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public string GetElement()
        {
            var attr = Element.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public string GetElementDescription()
        {
            switch (Element)
            {
                case ENineStarKiElement.Earth:
                    return Dictionary.earth_element;
                case ENineStarKiElement.Fire:
                    return Dictionary.fire_element;
                case ENineStarKiElement.Metal:
                    return Dictionary.metal_element;
                case ENineStarKiElement.Tree:
                    return Dictionary.tree_element;
                case ENineStarKiElement.Water:
                    return Dictionary.water_element;
                default:
                    return string.Empty;
            }
        }

        public string GetColour()
        {
            var attr = Colour.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public string GetDirection()
        {
            var attr = Direction.GetAttribute<EnumDescriptionAttribute>();
            return attr.GetDescription();
        }

        public string GetTrigram()
        {
            return ResourceType.GetValueFromResource(TrigramName);
        }

        private string GetEnergytNumberAndName(ENineStarKiEnergy energy)
        {
            return $"{(int)energy} {energy.GetAttribute<NineStarKiEnumMetaDataAttribute>().Name}";
        }

        private string GetModalityDescription()
        {
            var modalityText = "";

            switch (Modality)
            {
                case ENineStarKiModality.Dynamic:
                    modalityText = Dictionary.dynamic_modality;
                    break;

                case ENineStarKiModality.Stable:
                    modalityText = Dictionary.stable_modality;
                    break;

                case ENineStarKiModality.Flexible:
                    modalityText = Dictionary.flexible_modality;
                    break;
            }

            return TemplateProcessor.PopulateTemplate(modalityText,
                new
                {
                    water = GetEnergytNumberAndName(ENineStarKiEnergy.Water),
                    soil = GetEnergytNumberAndName(ENineStarKiEnergy.Soil),
                    thunder = GetEnergytNumberAndName(ENineStarKiEnergy.Thunder),
                    wind = GetEnergytNumberAndName(ENineStarKiEnergy.Wind),
                    coreearth = GetEnergytNumberAndName(ENineStarKiEnergy.CoreEarth),
                    heaven = GetEnergytNumberAndName(ENineStarKiEnergy.Heaven),
                    lake = GetEnergytNumberAndName(ENineStarKiEnergy.Lake),
                    mountain = GetEnergytNumberAndName(ENineStarKiEnergy.Mountain),
                    fire = GetEnergytNumberAndName(ENineStarKiEnergy.Fire)
                }
            );
        }
    }

}