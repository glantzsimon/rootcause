using K9.Base.Globalisation;
using K9.DataAccessLayer.Attributes;
using System.ComponentModel.DataAnnotations;

namespace K9.WebApplication.Models
{
    public class ProductItem
    {
        public int Id { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductLabel)]
        public string ProductName { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SubTitleLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary),
            ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public string ProductSubTitle { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CategoryLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public string CategoryText  { get; set; }

        /// <summary>
        /// Used for labels in production
        /// </summary>
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ItemCodeLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int ItemCode { get; set; }


        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public float Amount { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.MaxDosageLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int MaxDosage { get; set; } = 1;

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.MinDosageLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary),
            ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int MinDosage { get; set; } = 1;

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CapsulesDosageLabel)]
        public string CapsulesDosageLabelText { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CapsulesDosageLabel)]
        public string CapsulesDailyLabellext { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.FullDosageText)]
        public string FullDosageLabellext { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BenefitsLabel)]
        public string BenefitsLabelText { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IngredientLabel)]
        public string IngredientsList { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.QuantitiesLabel)]
        public string QuantitiesList { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DailyValuesLabel)]
        public string DailyValues { get; set; }
        
        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.RecommendationsLabel)]
        public string RecommendationsText { get; set; }

        

    }
}