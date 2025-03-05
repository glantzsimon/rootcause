using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class DoshasScore
    {
        public int GetScore(HealthQuestionnaire hq, FoodItem foodItem)
        {
            var score = 0;
            var threshold = 33;
            var prakrutiDoshas = hq.GetPrakrutiDoshas(hq);
            var prakruti = prakrutiDoshas.GetDosha();
            var vikruti = hq.GetVikrutiDoshas();

            if (foodItem.VataDosha)
            {
                if (prakruti == EDosha.Vata || prakruti == EDosha.KaphaVata || prakruti == EDosha.VataPitta)
                {
                    score += 7;
                }

                if (vikruti.VataDoshaScore > threshold)
                {
                    score += 7;
                }
            }

            if (foodItem.PittaDosha)
            {
                if (prakruti == EDosha.Pitta || prakruti == EDosha.PittaKapha || prakruti == EDosha.VataPitta)
                {
                    score += 7;
                }

                if (vikruti.PittaDoshaScore > threshold)
                {
                    score += 7;
                }
            }

            if (foodItem.KaphaDosha)
            {
                if (prakruti == EDosha.Kapha || prakruti == EDosha.KaphaVata || prakruti == EDosha.PittaKapha)
                {
                    score += 7;
                }

                if (vikruti.KaphaDoshaScore > threshold)
                {
                    score += 7;
                }
            }

            // Aggravating
            if (foodItem.IsAggravatesVata)
            {
                if (prakruti == EDosha.Vata || prakruti == EDosha.KaphaVata || prakruti == EDosha.VataPitta)
                {
                    score -= 7;
                }

                if (vikruti.VataDoshaScore > threshold)
                {
                    score -= 7;
                }
            }

            if (foodItem.IsAggravatesPitta)
            {
                if (prakruti == EDosha.Pitta || prakruti == EDosha.PittaKapha || prakruti == EDosha.VataPitta)
                {
                    score -= 7;
                }

                if (vikruti.PittaDoshaScore > threshold)
                {
                    score -= 7;
                }
            }

            if (foodItem.IsAggravatesKapha)
            {
                if (prakruti == EDosha.Kapha || prakruti == EDosha.KaphaVata || prakruti == EDosha.PittaKapha)
                {
                    score -= 7;
                }

                if (vikruti.KaphaDoshaScore > threshold)
                {
                    score -= 7;
                }
            }

            return score;
        }
    }
}