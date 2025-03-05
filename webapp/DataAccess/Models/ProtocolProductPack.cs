using K9.Base.DataAccessLayer.Attributes;
using K9.SharedLibrary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolProductPacks, PluralName = Globalisation.Strings.Names.ProtocolProductPacks, Name = Globalisation.Strings.Names.ProtocolProductPack)]
    public class ProtocolProductPack : ScorableBase
    {
        [UIHint("Protocol")]
        [ForeignKey("Protocol")]
        public int ProtocolId { get; set; }

        public virtual Protocol Protocol { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolLabel)]
        [LinkedColumn(LinkedTableName = "Protocol", LinkedColumnName = "Name")]
        public string ProtocolName { get; set; }

        [UIHint("ProductPack")]
        [ForeignKey("ProductPack")]
        public int ProductPackId { get; set; }

        public virtual ProductPack ProductPack { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProductPackLabel)]
        [LinkedColumn(LinkedTableName = "ProductPack", LinkedColumnName = "Name")]
        public string ProductPackName { get; set; }
    }
}
