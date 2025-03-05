using K9.DataAccessLayer.Enums;
using K9.Globalisation;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using K9.DataAccessLayer.Attributes;
using K9.SharedLibrary.Extensions;

namespace K9.DataAccessLayer.Models
{
    public class NineStarKiModel 
    {
        [UIHint("NineStarKiEnergy")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MainEnergy)]
        public ENineStarKiEnergy MainEnergy { get; set; }

        [UIHint("NineStarKiEnergy")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CharacterEnergy)]
        public ENineStarKiEnergy CharacterEnergy { get; set; }

        [UIHint("NineStarKiEnergy")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SurfaceEnergy)]
        public ENineStarKiEnergy SurfaceEnergy { get; set; }

        [UIHint("NineStarKiEnergy")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.YearlyCycleEnergy)]
        public ENineStarKiEnergy YearlyCycleEnergy { get; set; }

        [UIHint("NineStarKiEnergy")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MonthlyCycleEnergy)]
        public ENineStarKiEnergy MonthlyCycleEnergy { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NineStarKiHealthAdvice)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string HealthAdvice {get; set; }

        [UIHint("Organ")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StrongYinOrgans)]
        public EOrgan? StrongYinOrgans => MainEnergy.GetAttribute<NineStarKiEnumMetaDataAttribute>()?.StrongYinOrgans;

        [UIHint("Organ")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StrongYangOrgans)]
        public EOrgan? StrongYangOrgans =>
            MainEnergy.GetAttribute<NineStarKiEnumMetaDataAttribute>()?.StrongYangOrgans;

        [UIHint("Organs")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WeakYinOrgans)]
        public EOrgan[] WeakYinOrgans =>
            MainEnergy.GetAttribute<NineStarKiEnumMetaDataAttribute>()?.WeakYinOrgans;

        [UIHint("Organs")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WeakYangOrgans)]
        public EOrgan[] WeakYangOrgans =>
            MainEnergy.GetAttribute<NineStarKiEnumMetaDataAttribute>()?.WeakYangOrgans;
    }
}
