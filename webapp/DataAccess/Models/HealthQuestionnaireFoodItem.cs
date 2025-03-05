using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolActivities, PluralName = Globalisation.Strings.Names.ProtocolActivities, Name = Globalisation.Strings.Names.ProtocolActivity)]
    public class HealthQuestionnaireFoodItem : ObjectBase
    {
        [ForeignKey("HealthQuestionnaire")]
        public int HealthQuestionnaireId { get; set; }

        public virtual HealthQuestionnaire HealthQuestionnaire { get; set; }
        
        [ForeignKey("FoodItem")]
        public int FoodItemId { get; set; }

        public virtual FoodItem FoodItem { get; set; }
    }
}
