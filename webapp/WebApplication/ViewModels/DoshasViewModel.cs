using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Helpers;
using K9.DataAccessLayer.Models;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;

namespace K9.WebApplication.ViewModels
{
    public class DoshasViewModel
    {
        private readonly HealthQuestionnaire _healthQuestionnaire;
        private readonly EDosha _dosha;
        private readonly int _vataPercentage;
        private readonly int _pittaPercentage;
        private readonly int _kaphaPercentage;
        private readonly bool _invertColors;

        public DoshasViewModel(HealthQuestionnaire healthQuestionnaire, EDosha mainDosha, int vataPercentage, int pittaPercentage, int kaphaPercentage, bool invertColors = false)
        {
            _healthQuestionnaire = healthQuestionnaire;
            _dosha = mainDosha;
            _vataPercentage = vataPercentage;
            _pittaPercentage = pittaPercentage;
            _kaphaPercentage = kaphaPercentage;
            _invertColors = invertColors;
        }
        
        public string DoshaName => _dosha.GetAttribute<EnumDescriptionAttribute>().Name;

        public string VataColor => _healthQuestionnaire?.GetColorFromScore(_invertColors ? _vataPercentage.InvertScore() : _vataPercentage).GetAttribute<GenoTypeEnumMetaDataAttribute>().Color;

        public string PittaColor => _healthQuestionnaire?.GetColorFromScore(_invertColors ? _pittaPercentage.InvertScore() : _pittaPercentage).GetAttribute<GenoTypeEnumMetaDataAttribute>().Color;

        public string KaphaColor => _healthQuestionnaire?.GetColorFromScore(_invertColors ? _kaphaPercentage.InvertScore() : _kaphaPercentage).GetAttribute<GenoTypeEnumMetaDataAttribute>().Color;

        public string ActivePaneClass => $"{Strings.CssClasses.ActivePanelClass} padding-top-20";
        public string InactivePaneClass => "padding-bottom-0";
        public string ActiveTabClass => Strings.CssClasses.ActiveTabClass;
        public string InactiveTabClass => "";

        public bool VataActive => (_dosha == EDosha.Vata || _dosha == EDosha.KaphaVata || _dosha == EDosha.VataPitta || _dosha == EDosha.Tridoshic);
        public bool PittaActive => !VataActive && (_dosha == EDosha.Pitta || _dosha == EDosha.PittaKapha || _dosha == EDosha.VataPitta || _dosha == EDosha.Tridoshic);
        public bool KaphaActive => !PittaActive && (_dosha == EDosha.Kapha || _dosha == EDosha.PittaKapha || _dosha == EDosha.KaphaVata || _dosha == EDosha.Tridoshic);

        public string VataActiveTabClass => VataActive ? ActiveTabClass : InactiveTabClass;
        public string VataActivePaneClass => VataActive ? ActivePaneClass : ActiveTabClass;
        public string PittaActiveTabClass => PittaActive ? ActiveTabClass : InactiveTabClass;
        public string PittaActivePaneClass => PittaActive ? ActivePaneClass : ActiveTabClass;
        public string KaphaActiveTabClass => KaphaActive ? ActiveTabClass : InactiveTabClass;
        public string KaphaActivePaneClass => KaphaActive ? ActivePaneClass : ActiveTabClass;
    }
}