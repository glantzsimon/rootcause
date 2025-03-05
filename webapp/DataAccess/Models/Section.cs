using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using K9.DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolSections, PluralName = Globalisation.Strings.Names.ProtocolSections, Name = Globalisation.Strings.Names.ProtocolSection)]
    public class Section : ObjectBase
    {
        [NotMapped]
        [UIHint("ProtocolSection")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolSectionLabel)]
        public int ProtocolSectionId => Id;

        [UIHint("ProductRecommendations")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.RecommendationsLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public EProductRecommendation Recommendations { get; set; }
        
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ShortDescriptionLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string ShortDescription { get; set; }

        [Index("IX_ProtocolSection_DisplayOrder", IsUnique = true)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DisplayOrderLabel)]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// When true, the protocol display screen will not show the number of products to take, as it is a "custom" / flexible option
        /// </summary>
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CustomLabel)]
        public bool IsCustom { get; set; }
    }
}
