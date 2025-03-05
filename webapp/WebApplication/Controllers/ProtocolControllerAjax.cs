using System;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public partial class ProtocolController
    {
        public JsonResult UpdateProtocolWellnessLevelForClient(int clientId, int value)
        {
            try
            {
                var hq = _healthQuestionnaireService.GetHealthQuestionnaireForClient(clientId);
                hq.CurrentHealthLevel = value;
                var threshold = hq.GetScoreThreshold();

                hq.IsLowOxalate = hq.GetOxalateScore() > threshold;
                hq.IsLowLectin = hq.GetLectinsScore() > threshold;
                hq.IsLowPhytate = hq.GetPhytateScore() > threshold;
                hq.IsLowHistamine = hq.GetHistamineScore() > threshold;
                hq.IsLowMycotoxin = hq.GetMycotoxinScore() > threshold;
                hq.IsLowOmega6 = hq.GetOmega6Score() > threshold;

                _healthQuestionnaireService.Save(hq);
                _healthQuestionnaireService.ClearCache();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

      
    }
}