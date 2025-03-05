using K9.WebApplication.Models;

namespace K9.WebApplication.ViewModels
{
    public class GetToTheRootKiAjaxModel
    {
        public EGetToTheRootKiEnergy MainEnergy { get; set; }
        public EGetToTheRootKiEnergy CharacterEnergy { get; set; }
        public EGetToTheRootKiEnergy SurfaceEnergy { get; set; }
        public EGetToTheRootKiEnergy YearlyCycleEnergy { get; set; }
        public EGetToTheRootKiEnergy MonthlyCycleEnergy { get; set; }

        public string HealthAdvice { get; set; }
    }
}