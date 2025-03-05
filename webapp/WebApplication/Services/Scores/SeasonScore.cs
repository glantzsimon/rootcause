using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class SeasonScore 
    {
        public int GetScore(HealthQuestionnaire hq, ESeason season, FoodItem foodItem)
        {
            var score = 0;

            switch (season)
            {
                case ESeason.Spring:
                    if (foodItem.IsSpring)
                    {
                        score += 7;
                    }
                    break;

                case ESeason.Summer:
                    if (foodItem.IsSummer)
                    {
                        score += 7;
                    }
                    break;

                case ESeason.LateSummer:
                    if (foodItem.IsLateSummer)
                    {
                        score += 7;
                    }
                    break;

                case ESeason.Autumn:
                    if (foodItem.IsAutumn)
                    {
                        score += 7;
                    }
                    break;

                case ESeason.Winter:
                    if (foodItem.IsWinter)
                    {
                        score += 7;
                    }
                    break;
            }

            return score;
        }
    }
}