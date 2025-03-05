using K9.DataAccessLayer.Models;
using System;

namespace K9.DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ScoreAttribute : Attribute, IGenoTypeBase
    {
        public bool VataDosha { get; set; }
        public bool PittaDosha { get; set; }
        public bool KaphaDosha { get; set; }
        public bool Hunter { get; set; }
        public bool Gatherer { get; set; }
        public bool Teacher { get; set; }
        public bool Explorer { get; set; }
        public bool Warrior { get; set; }
        public bool Nomad { get; set; }
        public bool Water { get; set; }
        public bool Earth { get; set; }
        public bool Tree { get; set; }
        public bool Metal { get; set; }
        public bool Fire { get; set; }
        public bool Cbs { get; set; }
        public bool Immunity { get; set; }
        public bool NeurologicalHealth { get; set; }
        public bool DigestiveHealth { get; set; }
        public bool CardioVascularHealth { get; set; }
        public bool UrologicalHealth { get; set; }
        public bool HormoneBalance { get; set; }
        public bool AntiInflammatory { get; set; }
        public bool AntiOxidant { get; set; }
        public bool CellularHealth { get; set; }
        public bool StressRelief { get; set; }
        public bool Detoxification { get; set; }
        public bool DentalHealth { get; set; }
        public bool MensHealth { get; set; }
        public bool WomensHealth { get; set; }
        public bool Pregnancy { get; set; }
        public bool Vitality { get; set; }
        public bool Fertility { get; set; }
        public bool Mood { get; set; }
        public bool Cognition { get; set; }
        public bool Sleep { get; set; }
        public bool BloodBuilding { get; set; }
        public bool Restorative { get; set; }
        public bool DNAIntegrity { get; set; }
        public bool Vegetarian { get; set; }
        public bool Vegan { get; set; }
        public bool Fruitarian { get; set; }
        public bool Carnivore { get; set; }
        public bool Pescatarian { get; set; }

        public int ScoreFactor { get; set; } = 1;

        public bool IsYin { get; set; }
        public bool IsYang { get; set; }
        public bool IsInflammation { get; set; }
        public bool IsAcetylation { get; set; }
        public bool IsFamilyHistory { get; set; }

        public bool IsOxalateIntolerance{ get; set; }
        public bool IsLectinIntolerance{ get; set; }
        public bool IsPhytateIntolerance{ get; set; }
        public bool IsHistamineIntolerance{ get; set; }
        public bool IsMycotoxinIntolerance{ get; set; }
        public bool IsOmega6Intolerance{ get; set; }

        public bool RespiratoryHealth{ get; set; }
    }

}