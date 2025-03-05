using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class FiveElementScore 
    {
        public int GetScore(HealthQuestionnaire hq, FoodItem foodItem)
        {
            var score = 0;
            var nineStarKi = hq.GetNineStarKiModel();

            if (foodItem.Water && nineStarKi.MainEnergy == ENineStarKiEnergy.Water)
            {
                score += 7;
            }
            if (foodItem.Tree && nineStarKi.MainEnergy == ENineStarKiEnergy.Thunder || 
                             nineStarKi.MainEnergy == ENineStarKiEnergy.Wind)
            {
                score += 7;
            }
            if (foodItem.Earth && nineStarKi.MainEnergy == ENineStarKiEnergy.Soil || 
                              nineStarKi.MainEnergy == ENineStarKiEnergy.CoreEarth ||
                              nineStarKi.MainEnergy == ENineStarKiEnergy.Mountain)
            {
                score += 7;
            }
            if (foodItem.Metal && nineStarKi.MainEnergy == ENineStarKiEnergy.Heaven ||
                              nineStarKi.MainEnergy == ENineStarKiEnergy.Lake)
            {
                score += 7;
            }
            if (foodItem.Fire && nineStarKi.MainEnergy == ENineStarKiEnergy.Fire)
            {
                score += 7;
            }

            return score;
        }
    }
}