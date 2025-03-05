using K9.DataAccessLayer.Models;
using System.Threading.Tasks;

namespace K9.WebApplication.Services
{
    public interface IHealthQuestionnaireServiceAsync : ICacheableService
    {
        Task AutoGenerateProtocolFromGeneticProfileAsync(HealthQuestionnaire hq);
        void AutoGenerateProtocolFromGeneticProfile(HealthQuestionnaire hq);
    }
}