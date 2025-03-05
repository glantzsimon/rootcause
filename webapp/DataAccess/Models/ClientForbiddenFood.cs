using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.SharedLibrary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ClientForbiddenFoods, PluralName = Globalisation.Strings.Names.ClientForbiddenFoods, Name = Globalisation.Strings.Names.ClientForbiddenFood)]
    public class ClientForbiddenFood : ObjectBase
    {
        [UIHint("Client")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [UIHint("FoodItem")]
        [ForeignKey("FoodItem")]
        public int FoodItemId { get; set; }

        public virtual FoodItem FoodItem { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.FoodLabel)]
        [LinkedColumn(LinkedTableName = "FoodItem", LinkedColumnName = "Name")]
        public string FoodItemName { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.FoodIsSuitableTitle)]
        public bool IsPromotion { get; set; }
    }
}
