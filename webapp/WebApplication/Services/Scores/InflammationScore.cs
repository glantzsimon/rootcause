using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class InflammationScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.AntiInflammatory)
            {
                return hq.GetInflammationScore() / 10;
            }

            return 0;
        }
    }
}