using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class CardiovascularScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            if (item.CardioVascularHealth)
            {
                return hq.GetCardiovascularScore() / 10;
            }

            return 0;
        }
    }
}