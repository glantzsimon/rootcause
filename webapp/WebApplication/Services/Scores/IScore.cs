using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public interface IScore
    {
        int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item);
    }
}