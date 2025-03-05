using K9.Base.Globalisation;
using K9.DataAccessLayer.Attributes;
using System.ComponentModel.DataAnnotations;

namespace K9.WebApplication.Models
{
    public class IngredientItem
    {
        public int Id { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IngredientLabel)]
        public string Name { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IngredientLabel)]
        public string NameCapitalised => Name.ToUpper();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CategoryLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public string CategoryText { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SummaryLabel)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Used for labels in production
        /// </summary>
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ItemCodeLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int ItemCode { get; set; }

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsHydroscopicLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public bool IsHydroscopic { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IngredientTypeLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public string IngredientTypeText { get; set; }

    }
}