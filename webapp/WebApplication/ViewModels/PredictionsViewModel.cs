using System.Web.Script.Serialization;
using K9.WebApplication.Models;

namespace K9.WebApplication.ViewModels
{
    public class PredictionsViewModel
    {
        [ScriptIgnore]
        public NineStarKiModel NineStarKiModel { get; }

        public NineStarKiSummaryModel PersonalChartModel { get; }
        
        public GetToTheRootKiPredictionsSummaryModel PredictionsSummaryModel { get; }

        [ScriptIgnore]
        public GetToTheRootKiSummaryViewModel GetToTheRootKiSummaryViewModel { get; }

        public PredictionsViewModel(NineStarKiModel nineStarKiModel, GetToTheRootKiSummaryViewModel GetToTheRootKiSummaryViewModel)
        {
            nineStarKiModel = nineStarKiModel;
            PersonalChartModel = new NineStarKiSummaryModel(nineStarKiModel);
            PredictionsSummaryModel = new GetToTheRootKiPredictionsSummaryModel(nineStarKiModel);
            GetToTheRootKiSummaryViewModel = GetToTheRootKiSummaryViewModel;
        }
    }
}