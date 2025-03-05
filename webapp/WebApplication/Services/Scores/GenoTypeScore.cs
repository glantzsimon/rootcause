using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public class GenoTypeScore : IScore
    {
        public int GetScore(HealthQuestionnaire hq, EGenoType genoType, GenoTypeBase item)
        {
            var score = 0;

            if (genoType == EGenoType.Hunter)
            {
                if (item.Hunter)
                {
                    score += 7;

                    if (item.HunterCompatibilityLevel == ECompatibilityLevel.Excellent)
                    {
                        score += 5;
                    }
                }
                else if (item.AntiInflammatory || item.StressRelief || item.AntiOxidant)
                {
                    score += 3;
                }
            }


            if (genoType == EGenoType.Gatherer)
            {
                if (item.Gatherer)
                {
                    score += 7;

                    if (item.GathererCompatibilityLevel == ECompatibilityLevel.Excellent)
                    {
                        score += 5;
                    }
                }
            }

            if (genoType == EGenoType.Teacher)
            {
                if (item.Teacher)
                {
                    score += 7;

                    if (item.TeacherCompatibilityLevel == ECompatibilityLevel.Excellent)
                    {
                        score += 5;
                    }
                }
            }

            if (genoType == EGenoType.Explorer)
            {
                if (item.Explorer)
                {
                    score += 7;

                    if (item.ExplorerCompatibilityLevel == ECompatibilityLevel.Excellent)
                    {
                        score += 5;
                    }
                }
                else if (item.BloodBuilding || item.Restorative || item.Detoxification)
                {
                    score += 3;
                }
            }

            if (genoType == EGenoType.Warrior)
            {
                if (item.Warrior)
                {
                    score += 7;

                    if (item.WarriorCompatibilityLevel == ECompatibilityLevel.Excellent)
                    {
                        score += 5;
                    }
                }
                else if (item.AntiOxidant)
                {
                    score += 3;
                }
            }

            if (genoType == EGenoType.Nomad)
            {
                if (item.Nomad)
                {
                    score += 7;

                    if (item.NomadCompatibilityLevel == ECompatibilityLevel.Excellent)
                    {
                        score += 5;
                    }
                }
                else if (item.Immunity)
                {
                    score += 3;
                }
            }

            return score;
        }
    }
}