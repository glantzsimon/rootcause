using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.SharedLibrary.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ProtocolDietaryRecommendations, PluralName = Globalisation.Strings.Names.ProtocolDietaryRecommendations, Name = Globalisation.Strings.Names.ProtocolDietaryRecommendation)]
    public class ProtocolDietaryRecommendation : ScorableBase
    {
        [UIHint("Protocol")]
        [ForeignKey("Protocol")]
        public int ProtocolId { get; set; }

        public virtual Protocol Protocol { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ProtocolLabel)]
        [LinkedColumn(LinkedTableName = "Protocol", LinkedColumnName = "Name")]
        public string ProtocolName { get; set; }

        [UIHint("DietaryRecommendation")]
        [ForeignKey("DietaryRecommendation")]
        public int DietaryRecommendationId { get; set; }

        public virtual DietaryRecommendation DietaryRecommendation { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DietaryRecommendationLabel)]
        [LinkedColumn(LinkedTableName = "DietaryRecommendation", LinkedColumnName = "Name")]
        public string DietaryRecommendationName { get; set; }
    }
}
