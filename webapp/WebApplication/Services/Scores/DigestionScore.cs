using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class DigestionScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.DigestiveHealth)
            {
                return hq.GetDigestionIssuesScore() / 10;
            }

            return 0;
        }
    }
}