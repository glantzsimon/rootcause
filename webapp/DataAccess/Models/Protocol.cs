using DataAnnotationsExtensions;
using K9.Base.DataAccessLayer.Attributes;
using K9.Base.Globalisation;
using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Enums;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.Protocols, PluralName = Globalisation.Strings.Names.Protocols, Name = Globalisation.Strings.Names.Protocol)]
    public class Protocol : GenoTypeBase
    {
        public Guid ExternalId { get; set; }

        public EGenoType GenoType { get; set; }

        public GenoTypeEnumMetaDataAttribute GenoTypeEnumMetaDataAttribute() =>
            GenoType.GetAttribute<GenoTypeEnumMetaDataAttribute>();

        public string GenoTypeName => GenoTypeEnumMetaDataAttribute().GetDescription();

        public string GenoTypeDetailsName => $"{GenoTypeName}Details";

        [NotMapped]
        public Doshas Vikruti { get; set; }

        [NotMapped]
        public Doshas Prakruti { get; set; }
        
        [NotMapped]
        public NineStarKiModel NineStarKi { get; set; }

        #region Food Choices

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.RedMeat)]
        public bool EatsRedMeat { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Poultry)]
        public bool EatsPoultry { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FishAndSeafood)]
        public bool EatsFishAndSeafood { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.EggsAndRoes)]
        public bool EatsEggsAndRoes { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Dairy)]
        public bool EatsDairy { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Vegetables)]
        public bool EatsVegetables { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.VegetableProteinTitle)]
        public bool EatsVegetableProtein { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Fungi)]
        public bool EatsFungi { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Fruit)]
        public bool EatsFruit { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Grains)]
        public bool EatsGrains { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowOxalateLabel, Description = Globalisation.Strings.Names.OxalateDescription)]
        public bool IsLowOxalate { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowLectinLabel, Description = Globalisation.Strings.Names.LectinDescription)]
        public bool IsLowLectin { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowPhytateLabel, Description = Globalisation.Strings.Names.PhytateDescription)]
        public bool IsLowPhytate { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowHistamineLabel, Description = Globalisation.Strings.Names.HistamineDescription)]
        public bool IsLowHistamine { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowMycotoxinLabel, Description = Globalisation.Strings.Names.MycotoxinDescription)]
        public bool IsLowMycotoxin { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowOmega6Label, Description = Globalisation.Strings.Names.Omega6Description)]
        public bool IsLowOmega6 { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsBulletProofLabel, Description = Globalisation.Strings.Names.BulletProofDescription)]
        public bool IsBulletProof { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsSattvicLabel, Description = Globalisation.Strings.Names.SattvicDescription)]
        public bool IsSattvic { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowSulphur, Description = Globalisation.Strings.Names.LowSulphurDescription)]
        public bool IsLowSulphur { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsKeto, Description = Globalisation.Strings.Names.KetoDescription)]
        public bool IsKeto { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowGoitrogen, Description = Globalisation.Strings.Names.LowGoitrogenDescription)]
        public bool IsLowGoitrogen { get; set; }

        [NotMapped]
        [UIHint("Range")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CurrentHealthLevelSliderLabel)]
        [Min(1)]
        [Max(10)]
        public int? CurrentHealthLevel { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AutomaticallyFilterFoodsLabel)]
        public bool AutomaticallyFilterFoods { get; set; }


        #endregion

        [NotMapped]
        [UIHint("Protocol")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolLabel)]
        public int ProtocolId => Id;

        [UIHint("ProtocolType")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolTypeLabel)]
        public EProtocolType Type { get; set; }

        [UIHint("ProtocolFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.FrequencyLabel)]
        public EProtocolFrequency ProtocolFrequency { get; set; }

        [UIHint("Period")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PeriodLabel)]
        public EPeriod Period { get; set; }

        [UIHint("Every")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PeriodValueLabel)]
        public int PeriodValue { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.NumberOfPeriodsOffLabel)]
        public int NumberOfPeriodsOff { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.NumberOfPeriodsOnLabel)]
        public int NumberOfPeriodsOn { get; set; }

        [UIHint("Client")]
        [ForeignKey("Client")]
        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.CustomerLabel)]
        public int? ClientId { get; set; }

        public virtual Client Client { get; set; }

        [LinkedColumn(LinkedTableName = "Client", LinkedColumnName = "FullName")]
        public string ClientName { get; set; }

        [UIHint("ProtocolDuration")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DurationLabel)]
        public EProtocolDuration Duration { get; set; } = EProtocolDuration.ThreeMonths;

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DaysDurationLabel)]
        public int DaysDuration => GetDaysDuration();

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DaysDurationLabel)]
        public string DaysDurationTitle => $"{DaysDuration} {Dictionary.Days}";

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CustomDurationLabel)]
        public int CustomDaysDuration { get; set; }

        public virtual IEnumerable<ProtocolActivity> ProtocolActivities { get; set; }

        [NotMapped]
        public List<ProtocolActivity> Activities { get; set; }

        public virtual IEnumerable<ProtocolDietaryRecommendation> ProtocolDietaryRecommendations { get; set; }

        [NotMapped]
        public List<ProtocolDietaryRecommendation> DietaryRecommendations { get; set; }

        public virtual IEnumerable<ProtocolFoodItem> ProtocolFoodItems { get; set; }

        [NotMapped]
        public List<ProtocolFoodItem> RecommendedFoods { get; set; }

        public virtual IEnumerable<ProtocolProduct> ProtocolProducts { get; set; }

        public ProtocolProduct GetProtocolProductByProductId(int productId) =>
            Products?.FirstOrDefault(e => e.ProductId == productId);

        public ProtocolProductPack GetProtocolProductPackByProductId(int productId) =>
            ProductPacks?.FirstOrDefault(e => e.ProductPack.Products.Select(p => p.ProductId).Contains(productId));

        public ProductPackProduct GetProtocolProductPackProductByProductId(int productId) =>
            GetProtocolProductPackByProductId(productId)?.ProductPack.Products.FirstOrDefault(e => e.ProductId == productId);

        [NotMapped]
        public List<ProtocolProduct> Products { get; set; }

        public virtual IEnumerable<ProtocolProductPack> ProtocolProductPacks { get; set; }

        public List<Product> GetAllProducts() => Products?.Select(e => e?.Product).ToList()
                                                .Concat(ProductPacks?
                                                .SelectMany(e => e.ProductPack.Products.Select(p => p.Product)))
                                                .Distinct().ToList();

        [NotMapped]
        public List<ProtocolProductPack> ProductPacks { get; set; }

        public virtual IEnumerable<ProtocolSection> ProtocolProtocolSections { get; set; }

        [NotMapped]
        public List<ProtocolSection> ProtocolSections { get; set; }

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

        [FileSourceInfo("upload/protocols", Filter = EFilesSourceFilter.Images)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadImages)]
        public FileSource ImageFileSource { get; set; }

        [FileSourceInfo("upload/protocols", Filter = EFilesSourceFilter.Videos)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadVideo)]
        public FileSource VideoFileSource { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SeoFriendlyIdLabel)]
        public string SeoFriendlyId { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsScheduleDisplayedLabel)]
        public bool IsScheduleDisplayed { get; set; }

        [StringLength(512)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.VideoUrlLabel)]
        public string VideoUrl { get; set; }

        public int GetPeriodLength()
        {
            switch (ProtocolFrequency)
            {
                case EProtocolFrequency.Fortnightly:
                    return 14;

                case EProtocolFrequency.Monthly:
                    return 28;

                case EProtocolFrequency.Daily:
                    return 7;
            }

            return 0;
        }

        public int GetNumberOfPeriodsPerDuration()
        {
            if (Type == EProtocolType.Default)
            {
                var numberOfPeriodsPerDuration = (float)DaysDuration / (float)GetPeriodLength();
                if (numberOfPeriodsPerDuration <= 0)
                {
                    throw new Exception(Globalisation.Dictionary.ProtocolDurationIsTooShort);
                }

                return (int)Math.Round(numberOfPeriodsPerDuration, 0, MidpointRounding.AwayFromZero);
            }

            return 0;
        }

        public int GetNumberOfDaysOnPerDuration()
        {
            if (Type != EProtocolType.Default)
            {
                // Presume every day (for now)
                return DaysDuration;
            }

            var numberOfPeriodsPerDuration = GetNumberOfPeriodsPerDuration();
            switch (ProtocolFrequency)
            {
                case EProtocolFrequency.Daily:
                    return numberOfPeriodsPerDuration * (GetPeriodLength() - NumberOfPeriodsOff);

                case EProtocolFrequency.Fortnightly:
                case EProtocolFrequency.Monthly:
                    return numberOfPeriodsPerDuration * (NumberOfPeriodsOn);

                default:
                    return 0;
            }
        }

        public Product GetProduct(int productId)
        {
            return Products?.Select(e => e.Product).FirstOrDefault(e => e.Id == productId) ?? ProductPacks
                       ?.SelectMany(e => e.ProductPack.Products.Select(p => p.Product))
                       .FirstOrDefault(e => e.ProductId == productId);
        }

        private int GetDaysDuration()
        {
            switch (Duration)
            {
                case EProtocolDuration.Custom:
                    return CustomDaysDuration;

                default:
                    return (int)Duration;
            }
        }
    }
}
