using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class ImmunityScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.Immunity)
            {
                return hq.GetImmunityIssuesScore() / 10;
            }

            return 0;
        }
    }
}