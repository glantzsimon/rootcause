using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Helpers;
using K9.DataAccessLayer.Interfaces;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Enums;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.Products, PluralName = Globalisation.Strings.Names.Products, Name = Globalisation.Strings.Names.Product)]
    public class Product : GenoTypeBase, ICategorisable
    {
        public const int ProductLabelBenefitsCount = 9;

        public Guid ExternalId { get; set; }

        [UIHint("ProductCategory")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CategoryLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public ECategory Category { get; set; }

        public string GetCategoryText() => Category.GetAttribute<EnumDescriptionAttribute>().GetDescription();

        /// <summary>
        /// Used for labels in production
        /// </summary>
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ItemCodeLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int ItemCode { get; set; }

        [NotMapped]
        public int ExpectedItemCode { get; set; }

        [NotMapped]
        [UIHint("Product")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductLabel)]
        public int ProductId => Id;

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PurchaseUrlsLabel)]
        public string Url { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.QrCodeUrlsLabel)]
        public string QrCodeUrl { get; set; }

        [NotMapped]
        public List<Ingredient> IngredientsSelectList { get; set; }

        [UIHint("ProductType")]
        [Required]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProductTypeLabel)]
        public EProductType ProductType { get; set; }

        public string GetMeasuredIn() => GetMeasuredInText();

        public string GetServingMeasuredIn() => GetServingMeasuredInText();

        public string GetMeasuredInForLargeQuantity() => GetMeasuredInForLargeQuantityText();

        [UIHint("Client")]
        [ForeignKey("Client")]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.CustomerLabel)]
        public int? ClientId { get; set; }

        public virtual Client Client { get; set; }

        [LinkedColumn(LinkedTableName = "Client", LinkedColumnName = "FullName")]
        public string ClientName { get; set; }

        public virtual IEnumerable<ProductIngredient> ProductIngredients { get; set; }

        [NotMapped]
        public List<ProductIngredient> Ingredients { get; set; }

        [NotMapped]
        public List<ProductIngredient> IngredientsWithSubstitutes { get; set; }

        public List<ECategory> IngredientsCategories =>
            IngredientsWithSubstitutes?.Select(e => e.Ingredient.Category).Distinct().OrderBy(e => e).ToList();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsMainLabel)]
        public bool IsMain { get; set; }

        [UIHint("Quantity")]
        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public float Amount { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public int DisplayAmount { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public string FormattedAmount { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public string GetFormattedAmount() => $"{Amount} {GetMeasuredIn()}";

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)] public string FormattedSmallPackAmount => $"{Amount / 2} {GetMeasuredIn()}";

        [UIHint("Quantity")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountPerServingLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public float AmountPerServing { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountPerServingLabel)]
        public string FormattedAmountPerServing { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountPerServingLabel)]
        public string GetFormattedAmountPerServing() => $"{AmountPerServing} {GetServingMeasuredIn()}";

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ShortDescriptionLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string ShortDescription { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BodyLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Body { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BenefitsLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Benefits { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DosageLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Dosage { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AllowIngredientSubstitutesLabel)]
        public bool AllowIngredientSubstitutes { get; set; }

        [UIHint("ProductRecommendations")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.RecommendationsLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public EProductRecommendation Recommendations { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ShopPriceLabel)]
        [DataType(DataType.Currency)]
        public double GetShopPrice() => Price + (Price * 0.20);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceLabel)]
        public string GetFormattedPrice() => Price.ToString("C", CultureInfo.GetCultureInfo("th-TH"));

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceLabel)]
        public string GetFormattedShopPrice() => string.Format(CultureInfo.GetCultureInfo("th-TH"), "{0:C0}", GetShopPrice());

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceLabel)]
        [DataType(DataType.Currency)]
        public double GetInternationalPrice() => Price.ToInternationalPrice();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceLabel)]
        public string GetFormattedInternationalPrice() => GetInternationalPrice().ToString("C", CultureInfo.CurrentCulture);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceSmallPackLabel)]
        [DataType(DataType.Currency)]
        public double PriceSmallPack => Methods.RoundToInteger(Price * 0.60, 100);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceLabel)]
        public string GetFormattedPriceSmallPack() =>
            double.Parse(PriceSmallPack.ToString()).ToString("C", CultureInfo.GetCultureInfo("th-TH"));

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostOfMaterialsLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [DataType(DataType.Currency)]
        public double CostOfMaterials { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostOfIngredientsLabel)]
        [DataType(DataType.Currency)]
        public double CostOfIngredients { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostOfIngredientsLabel)]
        [DataType(DataType.Currency)]
        public double GetCostOfIngredients() => ((ProductIngredients?.Sum(e => e.Cost) ?? 0) * Amount);

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.TotalCostLabel)]
        [DataType(DataType.Currency)]
        public double TotalCost { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.TotalCostLabel)]
        [DataType(DataType.Currency)]
        public double GetTotalCost() => GetCostOfIngredients() + CostOfMaterials;

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.SuggestedRetailPriceLabel)]
        [DataType(DataType.Currency)]
        public double SuggestedRetailPrice { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.SuggestedRetailPriceLabel)]
        [DataType(DataType.Currency)]
        public double GetSuggestedRetailPrice() => Methods.RoundToInteger(GetTotalCost() * 2 + 900, 100);

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.SuggestedRetailPriceLabel)]
        public string FormattedSuggestedRetailPrice => double.Parse(SuggestedRetailPrice.ToString())
            .ToString("C", CultureInfo.GetCultureInfo("th-TH"));

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProfitMarginLabel)]
        [DataType(DataType.Currency)]
        public double ProfitMargin { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProfitMarginLabel)]
        [DataType(DataType.Currency)]
        public double GetProfitMargin() => Price - GetTotalCost();

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginSmallPackLabel)]
        [DataType(DataType.Currency)]
        public double ProfitMarginSmallPack { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginSmallPackLabel)]
        [DataType(DataType.Currency)]
        public double GetProfitMarginSmallPack() => PriceSmallPack - (GetTotalCost() / 2);

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginDiscount1Label)]
        [DataType(DataType.Currency)]
        public double ProfitMarginDiscount1 { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginDiscount1Label)]
        [DataType(DataType.Currency)]
        public double GetProfitMarginDiscount1() => PriceDiscount1 - GetTotalCost();

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginSmallPackDiscount1Label)]
        [DataType(DataType.Currency)]
        public double ProfitMarginSmallPackDiscount1 { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginSmallPackDiscount1Label)]
        [DataType(DataType.Currency)]
        public double GetProfitMarginSmallPackDiscount1() => PriceSmallPackDiscount1 - (GetTotalCost() / 2);

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginDiscount2Label)]
        [DataType(DataType.Currency)]
        public double ProfitMarginDiscount2 { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginDiscount2Label)]
        [DataType(DataType.Currency)]
        public double GetProfitMarginDiscount2() => PriceDiscount2 - GetTotalCost();

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginSmallPackDiscount2Label)]
        [DataType(DataType.Currency)]
        public double ProfitMarginSmallPackDiscount2 { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.ProfitMarginSmallPackDiscount2Label)]
        [DataType(DataType.Currency)]
        public double GetProfitMarginSmallPackDiscount2() => PriceSmallPackDiscount2 - (GetTotalCost() / 2);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProfitMarginLabel)]
        public string GetFormattedProfitMargin() =>
            double.Parse(GetProfitMargin().ToString()).ToString("C", CultureInfo.GetCultureInfo("th-TH"));

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceDiscount1Label)]
        [DataType(DataType.Currency)]
        public double PriceDiscount1 => Methods.RoundToInteger(Price * 0.80, 100);

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.PriceSmallPackDiscount1Label)]
        [DataType(DataType.Currency)]
        public double PriceSmallPackDiscount1 => Methods.RoundToInteger(PriceSmallPack * 0.80, 100);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceDiscount1Label)]
        public string GetFormattedPriceDiscount1() =>
            double.Parse(PriceDiscount1.ToString()).ToString("C", CultureInfo.GetCultureInfo("th-TH"));

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceDiscount2Label)]
        [DataType(DataType.Currency)]
        public double PriceDiscount2 => Methods.RoundToInteger(Price * 0.66, 100);

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.PriceSmallPackDiscount2Label)]
        [DataType(DataType.Currency)]
        public double PriceSmallPackDiscount2 => Methods.RoundToInteger(PriceSmallPack * 0.66, 100);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceDiscount2Label)]
        public string GetFormattedPriceDiscount2() =>
            double.Parse(PriceDiscount2.ToString()).ToString("C", CultureInfo.GetCultureInfo("th-TH"));

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.InStockLabel)]
        public bool IsHydroscopic() => Ingredients?.Any(e => e.Ingredient.IsHydroscopic) ?? false;

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.QuantityInStockLabel)]
        public int QuantityInStock { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.InStockLabel)]
        public bool GetIsInStock() => QuantityInStock > 0;

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.StockLowWarningLevelLabel)]
        public int StockLowWarningLevel { get; set; } = 10;

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.StockLowWarningLabel)]
        public bool GetIsStockLowWarning() => QuantityInStock < StockLowWarningLevel;

        [FileSourceInfo("upload/products", Filter = EFilesSourceFilter.Images)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadImages)]
        public FileSource ImageFileSource { get; set; }

        [FileSourceInfo("upload/products", Filter = EFilesSourceFilter.Videos)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadVideo)]
        public FileSource VideoFileSource { get; set; }

        [StringLength(512)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ImageUrlLabel)]
        public string ImageUrl { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AdditionalCssClassesLabel)]
        public string AdditionalCssClasses { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SeoFriendlyIdLabel)]
        public string SeoFriendlyId { get; set; }

        [StringLength(512)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.VideoUrlLabel)]
        public string VideoUrl { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BatchSizeLabel)]
        public int BatchSize { get; set; } = 1;

        #region Product Label

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductLabel)]
        public string GetProductName() => Name.ToUpper();

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SubTitleLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary),
            ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public string SubTitleLabelText { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SubTitleLabel)]
        public string GetProductSubTitle() => SubTitleLabelText?.ToUpper();

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.MaxDosageLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int MaxDosage { get; set; } = 1;

        [ProductLabel]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.MinDosageLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary),
            ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int MinDosage { get; set; } = 1;

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CapsulesDosageLabel)]
        public string GetCapsulesDosageLabelText() => MaxDosage > 1 && MaxDosage > MinDosage ? $"{MinDosage} - {MaxDosage}" : MaxDosage.ToString();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CapsulesLabel)]
        public string GetCapsulesLabellext() =>
            MaxDosage > 1 ? Globalisation.Dictionary.Capsules : Globalisation.Dictionary.Capsule;

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.CapsulesDailyText)]
        public string GetAmountToTakeDailyText()
        {
            switch (ProductType)
            {
                case EProductType.Capsules:
                    return $"{GetCapsulesLabellext()} {Globalisation.Dictionary.Daily}";

                default:
                    return $"{GetMeasuredInText().ToProperCase()} {Globalisation.Dictionary.Daily}";
            }
        }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.FullDosageText)]
        public string GetFullDosageLabellext() => $"{GetCapsulesDosageLabelText()} {GetAmountToTakeDailyText().ToLower()}";

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BenefitsLabel)]
        public string GetBenefitsLabelText() => Benefits.HtmlToText().SelectLines(ProductLabelBenefitsCount);

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IngredientLabel)]
        public string GetIngredientsList() => Ingredients?.Select(e => e.Ingredient.Name).ToArray().ToDisplayList();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.QuantitiesLabel)]
        public string GetQuantitiesList() => Ingredients?.Select(e => e.GetFormattedLabelAmount()).ToArray().ToDisplayList();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DailyValuesLabel)]
        public string GetDailyValues() =>
            Ingredients?.Select(e => e.GetFormattedPercentageOfDailyAllowance()).ToArray().ToDisplayList();

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.RecommendationsLabel)]
        public string GetRecommendationsText() => ProductType == EProductType.Capsules ?
            $"Take {GetFullDosageLabellext()} {Recommendations.GetAttribute<EnumDescriptionAttribute>()?.GetDescription().ToLower()}"
        : Dosage;

        #endregion

        public bool CheckRecommendations(EProductRecommendation recommendation)
        {
            switch (Recommendations)
            {
                case EProductRecommendation.WithOrWithoutFood:
                    return true;

                case EProductRecommendation.OnEmptyStomach:
                    return recommendation == EProductRecommendation.OnEmptyStomach;

                case EProductRecommendation.WithFood:
                    return recommendation == EProductRecommendation.WithFood || recommendation == EProductRecommendation.WithFat || recommendation == EProductRecommendation.WithOrWithoutFood;

                case EProductRecommendation.WithFat:
                    return recommendation == EProductRecommendation.WithFat;

                default:
                    return false;
            }
        }

        public static List<PropertyInfo> GetProductLabelProperties() => typeof(Product).GetProperties()
            .Where(e => e.GetCustomAttributes<ProductLabelAttribute>().Any()).ToList();

        public float GetTotalIngredientsAmount()
        {
            return Ingredients?.Sum(e => e.AmountPerConcentration) ?? 0;
        }

        public string GetFormattedTotalIngredientsAmount()
        {
            return $"{GetTotalIngredientsAmount()} {GetServingMeasuredIn()}";
        }

        public bool IngredientAmountsAreCorrect()
        {
            switch (ProductType)
            {
                case EProductType.Powder:
                    return GetTotalIngredientsAmount() == Amount;

                default:
                    return GetTotalIngredientsAmount() == AmountPerServing;
            }
        }

        public string GetIngredientAmountIncorrectError()
        {
            return $"Total ingredients per serving must equal {AmountPerServing} {GetServingMeasuredIn()}. The current value is {GetFormattedTotalIngredientsAmount()}";
        }

        private string GetMeasuredInText()
        {
            switch (ProductType)
            {
                case EProductType.Capsules:
                    return Globalisation.Strings.Constants.Measures.Capsules;

                case EProductType.Powder:
                    return Globalisation.Strings.Constants.Measures.Milligrams;

                case EProductType.Liquid:
                    return Globalisation.Strings.Constants.Measures.Millilitres;

                default:
                    return string.Empty;
            }
        }

        private string GetServingMeasuredInText()
        {
            switch (ProductType)
            {
                case EProductType.Capsules:
                case EProductType.Powder:
                    return Globalisation.Strings.Constants.Measures.Milligrams;

                case EProductType.Liquid:
                    return Globalisation.Strings.Constants.Measures.Millilitres;

                default:
                    return string.Empty;
            }
        }

        private string GetMeasuredInForLargeQuantityText()
        {
            switch (ProductType)
            {
                case EProductType.Capsules:
                    return Globalisation.Strings.Constants.Measures.Capsules;

                case EProductType.Powder:
                    return Globalisation.Strings.Constants.Measures.Grams;

                case EProductType.Liquid:
                    return Globalisation.Strings.Constants.Measures.Litres;

                default:
                    return string.Empty;
            }
        }
    }
}
