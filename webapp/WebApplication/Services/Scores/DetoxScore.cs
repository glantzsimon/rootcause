using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class DetoxScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.Detoxification)
            {
                var acetyationScore = hq.GetAcetulationScore() / 10;
                var cbsScore = hq.GetCbsScore() / 10;
                return (acetyationScore + cbsScore) / 2;
            }

            return 0;
        }
    }
}