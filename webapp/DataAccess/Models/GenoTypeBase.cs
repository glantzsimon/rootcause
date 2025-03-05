using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    public abstract class GenoTypeBase : ObjectBase, IObjectBase, IGenoTypeBase
    {
        [NotMapped]
        public int Score { get; set; }

        [NotMapped]
        public int RelativeScore { get; set; }

        public EScore GetScore()
        {
            if (Score < 0)
            {
                return EScore.Negative;
            }

            if (RelativeScore >= 80)
            {
                return EScore.VeryHigh;
            }

            if (RelativeScore >= 70)
            {
                return EScore.High;
            }

            if (RelativeScore >= 60)
            {
                return EScore.High;
            }

            if (RelativeScore >= 44)
            {
                return EScore.Medium;
            }

            if (RelativeScore <= 22)
            {
                return EScore.Low;
            }

            if (RelativeScore <= 7)
            {
                return EScore.VeryLow;
            }

            return EScore.Medium;
        }

        public string GetRelativeScoreHtml()
        {
            var score = GetScore();

            if (score == EScore.VeryHigh)
            {
                return "<i class=\"fa fa-heart food-item-info\"></i><i class=\"fa fa-heart food-item-info\"></i>";
            }

            if (score == EScore.High)
            {
                return "<i class=\"fa fa-heart food-item-info\"></i>";
            }

            return "";
        }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesVataLabel)]
        public bool VataDosha { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesPittaLabel)]
        public bool PittaDosha { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesKaphaLabel)]
        public bool KaphaDosha { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Hunter)]
        public bool Hunter { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Gatherer)]
        public bool Gatherer { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Teacher)]
        public bool Teacher { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Explorer)]
        public bool Explorer { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Warrior)]
        public bool Warrior { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Nomad)]
        public bool Nomad { get; set; }

        [UIHint("CompatibilityLevel")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.HunterCompatibilityLevel)]
        public ECompatibilityLevel HunterCompatibilityLevel { get; set; }

        [UIHint("CompatibilityLevel")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.GathererCompatibilityLevel)]
        public ECompatibilityLevel GathererCompatibilityLevel { get; set; }

        [UIHint("CompatibilityLevel")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TeacherCompatibilityLevel)]
        public ECompatibilityLevel TeacherCompatibilityLevel { get; set; }

        [UIHint("CompatibilityLevel")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ExplorerCompatibilityLevel)]
        public ECompatibilityLevel ExplorerCompatibilityLevel { get; set; }

        [UIHint("CompatibilityLevel")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.WarriorCompatibilityLevel)]
        public ECompatibilityLevel WarriorCompatibilityLevel { get; set; }

        [UIHint("CompatibilityLevel")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.NomadCompatibilityLevel)]
        public ECompatibilityLevel NomadCompatibilityLevel { get; set; }

        /// <summary>
        /// True when the fooditem has been marked NOT OK by the client as unsuitable
        /// </summary>
        [NotMapped]
        public bool IsDemoted { get; set; }

        /// <summary>
        /// True when the fooditem has been marked as OK by the client as unsuitable
        /// </summary>
        [NotMapped]
        public bool IsPromoted { get; set; }

        [UIHint("FoodFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.HunterFrequency)]
        public EFoodFrequency? HunterFrequency { get; set; }

        [UIHint("FoodFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.GathererFrequency)]
        public EFoodFrequency? GathererFrequency { get; set; }

        [UIHint("FoodFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.TeacherFrequency)]
        public EFoodFrequency? TeacherFrequency { get; set; }

        [UIHint("FoodFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.ExplorerFrequency)]
        public EFoodFrequency? ExplorerFrequency { get; set; }

        [UIHint("FoodFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.WarriorFrequency)]
        public EFoodFrequency? WarriorFrequency { get; set; }

        [UIHint("FoodFrequency")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.NomadFrequency)]
        public EFoodFrequency? NomadFrequency { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesWaterLabel)]
        public bool Water { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesEarthLabel)]
        public bool Earth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesTreeLabel)]
        public bool Tree { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesMetalLabel)]
        public bool Metal { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BalancesFireLabel)]
        public bool Fire { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Cbs)]
        public bool Cbs { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Immunity)]
        public bool Immunity { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.NeurologicalHealth)]
        public bool NeurologicalHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.DigestiveHealth)]
        public bool DigestiveHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.CardiovascularHealth)]
        public bool CardioVascularHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.UrologicalHealth)]
        public bool UrologicalHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.HormoneBalance)]
        public bool HormoneBalance { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.AntiInflammatory)]
        public bool AntiInflammatory { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.AntiOxidant)]
        public bool AntiOxidant { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Longevity)]
        public bool Longevity { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.CellularHealth)]
        public bool CellularHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.StressRelief)]
        public bool StressRelief { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Detoxification)]
        public bool Detoxification { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.DentalHealth)]
        public bool DentalHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.MensHealth)]
        public bool MensHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.WomensHealth)]
        public bool WomensHealth { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Pregnancy)]
        public bool Pregnancy { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Vitality)]
        public bool Vitality { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Fertility)]
        public bool Fertility { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Mood)]
        public bool Mood { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Cognition)]
        public bool Cognition { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Sleep)]
        public bool Sleep { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BloodBuilding)]
        public bool BloodBuilding { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.Restorative)]
        public bool Restorative { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DNAIntegrity)]
        public bool DNAIntegrity { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.WeightLoss)]
        public bool WeightLoss { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Vegetarian)]
        public bool Vegetarian { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Vegan)]
        public bool Vegan { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Fruitarian)]
        public bool Fruitarian { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Carnivore)]
        public bool Carnivore { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Pescatarian)]
        public bool Pescatarian { get; set; }
    }
}
