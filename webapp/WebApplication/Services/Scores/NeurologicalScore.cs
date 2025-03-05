using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class NeurologicalScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.NeurologicalHealth)
            {
                return hq.GetNeurologicalScore() / 10;
            }

            return 0;
        }
    }
}