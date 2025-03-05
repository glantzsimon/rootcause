using System;
using System.Collections.Generic;
using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using K9.SharedLibrary.Attributes;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProductIngredients, PluralName = Globalisation.Strings.Names.ProductIngredients, Name = Globalisation.Strings.Names.ProductIngredient)]
    public class ProductIngredient : ObjectBase
    {
        [UIHint("Product")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductLabel)]
        [LinkedColumn(LinkedTableName = "Product", LinkedColumnName = "Title")]
        public string ProductName { get; set; }

        [UIHint("Ingredient")]
        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IngredientLabel)]
        [LinkedColumn(LinkedTableName = "Ingredient", LinkedColumnName = "Name")]
        public string IngredientName { get; set; }

        [UIHint("Quantity")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public float Amount { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.NumberOfSubstitutesToUseLabel)]
        public int NumberOfSubstitutesToUse { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BatchSizeLabel)]
        public int BatchSize { get; set; } = 1;

        [UIHint("Quantity")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary),
            ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public float AmountPerBatch => Amount * BatchSize;

        [UIHint("Quantity")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public float AmountPerConcentration => Amount * (1 / (Ingredient?.Concentration ?? 1));

        [UIHint("Quantity")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public float AmountPerConcentrationPerBatch => AmountPerBatch * (1 / (Ingredient?.Concentration ?? 1));

        public string GetFormattedAmount() => $"{Amount} {Globalisation.Strings.Constants.Measures.Milligrams}";

        public string GetFormattedLabelAmount() => GetFormattedLabelAmountText();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountPerBatchLabel)] 
        public float AmountPer100Capsules => AmountPerConcentrationPerBatch * 100;

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsAddedLabel)]
        public bool IsAdded { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountPerBatchLabel)] public string FormattedAmountPer100Capsules => $"{AmountPer100Capsules} {Ingredient.GetMeasuredIn()}";

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountPerBatchLabel)]
        public string FormattedLargeAmountPer100Capsules =>
            $"{Math.Round(AmountPerConcentrationPerBatch / 10, 3, MidpointRounding.AwayFromZero)} {Ingredient.GetMeasuredInForLargeQuantity()}";

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostLabel)]
        [DataType(DataType.Currency)]
        public double Cost => Amount * Ingredient?.CostPerMilligram ?? 0;

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostLabel)]
        [DataType(DataType.Currency)]
        public double CostPer100Capsules => Cost * 100;

        private float GetRDA() => Ingredient?.RecommendedDailyAllownace ?? 0;

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PercentageOfRDALabel)]
        public float? GetPercentageOfDailyAllowance() => GetRDA() > 0 ? Amount / Ingredient?.RecommendedDailyAllownace : 0;

        public string GetFormattedPercentageOfDailyAllowance() =>
            GetPercentageOfDailyAllowance() > 0 ? GetPercentageOfDailyAllowance()?.ToString("P0") : "*";

        public virtual IEnumerable<ProductIngredientSubstitute> ProductIngredientSubstitutes { get; set; }

        [NotMapped]
        public List<ProductIngredientSubstitute> IngredientSubstitutes { get; set; }

        private string GetFormattedLabelAmountText()
        {
            var roundedMiligrams = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
            var roundedMicrograms = Math.Round(Amount * 1000, 0, MidpointRounding.AwayFromZero);

            if (Amount >= 1)
            {
                return $"{roundedMiligrams} {Globalisation.Strings.Constants.Measures.Milligrams}";
            }

            return $"{roundedMicrograms} {Globalisation.Strings.Constants.Measures.Micrograms}";
        }
    }
}
