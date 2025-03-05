using K9.WebApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace K9.WebApplication.ViewModels
{
    public class GetToTheRootKiSummaryViewModel
    {
        [ScriptIgnore]
        public List<NineStarKiEnergy> CharacterEnergies { get; set; }

        [ScriptIgnore]
        public List<NineStarKiEnergy> MainEnergies { get; set; }

        [ScriptIgnore]
        public GetToTheRootKiModalitySummaryViewModel DynamicEnergies { get; set; }

        [ScriptIgnore]
        public GetToTheRootKiModalitySummaryViewModel StableEnergies { get; set; }
        
        [ScriptIgnore]
        public GetToTheRootKiModalitySummaryViewModel FlexibleEnergies { get; set; }

        public GetToTheRootKiSummaryViewModel(
            List<NineStarKiModel> mainEnergies,
            List<NineStarKiModel> characterEnergies,
            List<NineStarKiEnergy> dynamicEnergies,
            List<NineStarKiEnergy> stableEnergies,
            List<NineStarKiEnergy> flexibleEnergies)
        {
            CharacterEnergies = characterEnergies.Select(e => e.CharacterEnergy).OrderBy(e => e.EnergyNumber).ToList();
            MainEnergies = mainEnergies.Select(e => e.MainEnergy).ToList();
            DynamicEnergies = new GetToTheRootKiModalitySummaryViewModel(EGetToTheRootKiModality.Dynamic, dynamicEnergies);
            StableEnergies = new GetToTheRootKiModalitySummaryViewModel(EGetToTheRootKiModality.Stable, stableEnergies); ;
            FlexibleEnergies = new GetToTheRootKiModalitySummaryViewModel(EGetToTheRootKiModality.Flexible, flexibleEnergies); ;
        }
    }
}