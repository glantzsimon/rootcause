using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class CbsScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.Cbs)
            {
                return hq.GetCbsScore() / 10;
            }

            return 0;
        }
    }
}