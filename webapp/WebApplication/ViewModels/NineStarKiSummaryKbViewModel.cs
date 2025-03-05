using K9.WebApplication.Models;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.ViewModels
{
    public class GetToTheRootKiSummaryKbViewModel
    {
        public List<GetToTheRootKiEnergySummary> CharacterEnergies { get; set; }
        public List<GetToTheRootKiEnergySummary> MainEnergies { get; set; }
        public GetToTheRootKiModalitySummaryViewModel DynamicEnergies { get; set; }
        public GetToTheRootKiModalitySummaryViewModel StableEnergies { get; set; }
        public GetToTheRootKiModalitySummaryViewModel FlexibleEnergies { get; set; }

        public GetToTheRootKiSummaryKbViewModel(GetToTheRootKiSummaryViewModel model)
        {
            CharacterEnergies =
                new List<GetToTheRootKiEnergySummary>(model.CharacterEnergies.Select(e => new GetToTheRootKiEnergySummary(e)));
            MainEnergies =
                new List<GetToTheRootKiEnergySummary>(model.MainEnergies.Select(e => new GetToTheRootKiEnergySummary(e)));
            DynamicEnergies = model.DynamicEnergies;
            StableEnergies = model.StableEnergies;
            FlexibleEnergies = model.FlexibleEnergies;
        }
    }
}