using K9.Base.DataAccessLayer.Attributes;
using K9.Base.Globalisation;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Helpers;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Enums;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.Ingredients, PluralName = Globalisation.Strings.Names.Ingredients, Name = Globalisation.Strings.Names.Ingredient)]
    public class Ingredient : GenoTypeBase
	{
	    public string GetDyamicFontLabelSize()
	    {
	        var charLength = Name.Length;
	        if (charLength < 7)
	        {
	            return "2.2em";
	        }
	        if (charLength < 10)
	        {
	            return "1.9em";
	        }
	        if (charLength < 14)
	        {
	            return "1.6em";
	        }
	        if (charLength < 18)
	        {
	            return "1.3em";
	        }
	        else
	        {
	            return "1.1em";
	        }
	    }

	    [UIHint("IngredientCategory")]
	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CategoryLabel)]
	    [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
	    public ECategory Category { get; set; }

	    public string CategoryText => Category.GetAttribute<EnumDescriptionAttribute>()?.GetDescription()?.Pluralise();

	    /// <summary>
	    /// Used for labels in production
	    /// </summary>
	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ItemCodeLabel)]
	    [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
	    public int ItemCode { get; set; }
        
	    [UIHint("IngredientType")]
	    [Required]
	    [Display(ResourceType = typeof(Globalisation.Dictionary),
	        Name = Globalisation.Strings.Labels.IngredientTypeLabel)]
	    public EIngredientType IngredientType { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary),
	        Name = Globalisation.Strings.Labels.IngredientTypeLabel)]
	    public string GetIngredientTypeText() => IngredientType.GetAttribute<EnumDescriptionAttribute>().GetDescription();

	    public string GetMeasuredIn() => GetMeasuredInText();

	    public string GetMeasuredInForLargeQuantity() => GetMeasuredInForLargeQuantityText();

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SummaryLabel)]
	    [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
	    [StringLength(int.MaxValue)]
	    [DataType(DataType.Html)]
	    [AllowHtml]
	    public string ShortDescription { get; set; }

		[Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DescriptionLabel)]
		[StringLength(int.MaxValue)]
		[DataType(DataType.Html)]
		[AllowHtml]
		public string Body { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BenefitsLabel)]
	    [StringLength(int.MaxValue)]
	    [DataType(DataType.Html)]
	    [AllowHtml]
	    public string Benefits { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SubjectiveEffectsLabel)]
	    [StringLength(int.MaxValue)]
	    [DataType(DataType.Html)]
	    [AllowHtml]
	    public string SubjectiveEffects { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SideEffectsLabel)]
	    [StringLength(int.MaxValue)]
	    [DataType(DataType.Html)]
	    [AllowHtml]
	    public string SideEffects { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DeficiencySymptomsLabel)]
	    [StringLength(int.MaxValue)]
	    [DataType(DataType.Html)]
	    [AllowHtml]
	    public string DeficiencySymptoms { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ResearchLabel)]
	    [StringLength(int.MaxValue)]
	    [DataType(DataType.Html)]
	    [AllowHtml]
	    public string Research { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostLabel)]
	    [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
	    [DataType(DataType.Currency)]
	    public double Cost { get; set; }

        [UIHint("Quantity")]
	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.QuantityLabel)]
	    [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
	    public int Quantity { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary),
	        Name = Globalisation.Strings.Labels.QuantityLabel)] public string FormattedQuantity => $"{Quantity} {GetMeasuredIn()}";

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostPer100GramsLabel)]
	    [DataType(DataType.Currency)]
	    public double GetCostPerGram() => (1000f / Quantity) * Cost;

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CostPerMilligramLabel)]
	    [DataType(DataType.Currency)]
	    public double CostPerMilligram => GetCostPerGram() / 1000;
        
	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsHydroscopicLabel)]
	    public bool IsHydroscopic { get; set; } = false;

	    [UIHint("Quantity")]
	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.QuantityInStockLabel)]
	    public int QuantityInStock { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary),
	        Name = Globalisation.Strings.Labels.QuantityInStockLabel)] public string FormattedQuantityInStock => $"{QuantityInStock} {GetMeasuredIn()}";

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.InStockLabel)]
	    public bool GetIsInStock() => QuantityInStock > 0;

	    public float GetLowStockFactor() => QuantityInStock / StockLowWarningLevel;

	    [Display(ResourceType = typeof(Globalisation.Dictionary),
	        Name = Globalisation.Strings.Labels.StockLowWarningLevelLabel)]
	    public int StockLowWarningLevel { get; set; } = 100000;

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.InStockLabel)] public bool IsStockLowWarning => QuantityInStock < StockLowWarningLevel;

	    [FileSourceInfo("upload/ingredients", Filter = EFilesSourceFilter.Images)]
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadImages)]
		public FileSource ImageFileSource { get; set; }

	    [FileSourceInfo("upload/ingredients", Filter = EFilesSourceFilter.Videos)]
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

        [UIHint("Url")]
	    [StringLength(512)]
	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PurchaseUrlsLabel)]
	    public string PurchaseUrls { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsHiddenLabel)]
	    public bool IsHidden { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ConcentrationLabel)]
	    public float Concentration { get; set; } = 1;

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.RDALabel)]
        [UIHint("Quantity")]
	    public float RecommendedDailyAllownace { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.RDALabel)] 
	    public string FormattedRDA => GetFormattedRDAText();

	    private string GetFormattedRDAText()
	    {
	        var amountIndMiligrams = RecommendedDailyAllownace;
	        var amountInMicrograms = RecommendedDailyAllownace * 1000;

	        if (RecommendedDailyAllownace == 0)
	        {
	            return string.Empty;
	        }

	        var mgAmount = $"{amountIndMiligrams} {Globalisation.Strings.Constants.Measures.Milligrams}";
	        if (RecommendedDailyAllownace >= 1)
	        {
	            return mgAmount;
	        }

	        return $"{mgAmount} ({amountInMicrograms} {Globalisation.Strings.Constants.Measures.Micrograms})";
	    }

	    private string GetMeasuredInText()
	    {
	        switch (IngredientType)
	        {
	            case EIngredientType.Liquid:
	                return Globalisation.Strings.Constants.Measures.Millilitres;

	            case EIngredientType.Powder:
	                return Globalisation.Strings.Constants.Measures.Milligrams;
                    
	            default:
	                return string.Empty;

	        }
	    }

	    private string GetMeasuredInForLargeQuantityText()
	    {
	        switch (IngredientType)
	        {
	            case EIngredientType.Liquid:
	                return Globalisation.Strings.Constants.Measures.Litres;

	            case EIngredientType.Powder:
	                return Globalisation.Strings.Constants.Measures.Grams;
                    
	            default:
	                return string.Empty;

	        }
	    }
	}
}
