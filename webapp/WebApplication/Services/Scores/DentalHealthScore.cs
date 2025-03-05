using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class DentalHealthScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.DentalHealth)
            {
                return hq.GetDentalHealthScore() / 10;
            }

            return 0;
        }
    }
}