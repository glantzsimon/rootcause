using K9.Base.DataAccessLayer.Attributes;
using K9.SharedLibrary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolProducts, PluralName = Globalisation.Strings.Names.ProtocolProducts, Name = Globalisation.Strings.Names.ProtocolProduct)]
    public class ProtocolProduct : ScorableBase
    {
        [UIHint("Protocol")]
        [ForeignKey("Protocol")]
        public int ProtocolId { get; set; }

        public virtual Protocol Protocol { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolLabel)]
        [LinkedColumn(LinkedTableName = "Protocol", LinkedColumnName = "Name")]
        public string ProtocolName { get; set; }

        [UIHint("Product")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductLabel)]
        [LinkedColumn(LinkedTableName = "Product", LinkedColumnName = "Title")]
        public string ProductName { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountLabel)]
        public int Amount { get; set; } = 1;

        [NotMapped]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AmountRequiredLabel)]
        public int AmountRequired { get; set; }
    }
}
