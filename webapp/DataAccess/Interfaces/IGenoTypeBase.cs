using K9.DataAccessLayer.Enums;

namespace K9.DataAccessLayer.Models
{
    public interface IGenoTypeBase 
    {
        bool VataDosha { get; set; }

        bool PittaDosha { get; set; }

        bool KaphaDosha { get; set; }

        bool Hunter { get; set; }

        bool Gatherer { get; set; }

        bool Teacher { get; set; }

        bool Explorer { get; set; }

        bool Warrior { get; set; }

        bool Nomad { get; set; }
        
        bool Water { get; set; }

        bool Earth { get; set; }

        bool Tree { get; set; }

        bool Metal { get; set; }

        bool Fire { get; set; }

        bool Cbs { get; set; }

        bool Immunity { get; set; }

        bool NeurologicalHealth { get; set; }

        bool DigestiveHealth { get; set; }

        bool CardioVascularHealth { get; set; }

        bool UrologicalHealth { get; set; }

        bool HormoneBalance { get; set; }

        bool AntiInflammatory { get; set; }
        
        bool AntiOxidant { get; set; }

        bool CellularHealth { get; set; }

        bool StressRelief { get; set; }

        bool Detoxification { get; set; }

        bool BloodBuilding { get; set; }

        bool Restorative { get; set; }

        bool DentalHealth { get; set; }

        bool MensHealth { get; set; }

        bool WomensHealth { get; set; }

        bool Pregnancy { get; set; }

        bool Vitality { get; set; }

        bool Fertility { get; set; }

        bool Mood { get; set; }

        bool Cognition { get; set; }

        bool Sleep { get; set; }
        
        bool DNAIntegrity { get; set; }

        bool Vegetarian { get; set; }
        
        bool Vegan { get; set; }

        bool Fruitarian { get; set; }

        bool Carnivore { get; set; }

        bool Pescatarian { get; set; }
    }
}
