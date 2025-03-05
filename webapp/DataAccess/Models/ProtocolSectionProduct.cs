using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using K9.SharedLibrary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using K9.DataAccessLayer.Enums;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolProtocolSectionProducts, PluralName = Globalisation.Strings.Names.ProtocolProtocolSectionProducts, Name = Globalisation.Strings.Names.ProtocolProtocolSectionProduct)]
    public class ProtocolSectionProduct : ObjectBase
    {
        [ForeignKey("ProtocolSection")]
        public int ProtocolSectionId { get; set; }

        public virtual ProtocolSection ProtocolSection { get; set; }

        [UIHint("Product")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductLabel)]
        [LinkedColumn(LinkedTableName = "Product", LinkedColumnName = "Title")]
        public string ProductName { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        public int Amount { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public string FormattedAmount { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public string GetFormattedAmount() => $"{Amount} {GetMeasuredInText(Product)}";

        private string GetMeasuredInText(Product product)
        {
            switch (product.ProductType)
            {
                case EProductType.Capsules:
                    if (Amount == 1)
                    {
                        return Globalisation.Dictionary.Capsule;
                    }
                    else
                    {
                        return Globalisation.Strings.Constants.Measures.Capsules;
                    }

                case EProductType.Powder:
                    return Globalisation.Strings.Constants.Measures.Milligrams;

                case EProductType.Liquid:
                    return Globalisation.Strings.Constants.Measures.Millilitres;

                default:
                    return string.Empty;
            }
        }

        [NotMapped]
        public bool IsVisible { get; set; }
    }
}
