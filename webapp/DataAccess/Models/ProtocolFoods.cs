using K9.Base.DataAccessLayer.Attributes;
using K9.SharedLibrary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolFoods, PluralName = Globalisation.Strings.Names.ProtocolFoods, Name = Globalisation.Strings.Names.ProtocolFood)]
    public class ProtocolFoodItem : ScorableBase
    {
        [UIHint("Protocol")]
        [ForeignKey("Protocol")]
        public int ProtocolId { get; set; }

        public virtual Protocol Protocol { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolLabel)]
        [LinkedColumn(LinkedTableName = "Protocol", LinkedColumnName = "Name")]
        public string ProtocolName { get; set; }

        [UIHint("FoodItem")]
        [ForeignKey("FoodItem")]
        public int FoodItemId { get; set; }

        public virtual FoodItem FoodItem { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.FoodLabel)]
        [LinkedColumn(LinkedTableName = "FoodItem", LinkedColumnName = "Name")]
        public string FoodItemName { get; set; }
    }
}
