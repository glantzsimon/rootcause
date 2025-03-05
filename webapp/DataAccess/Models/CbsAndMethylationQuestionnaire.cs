using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace K9.DataAccessLayer.Models
{
    public partial class HealthQuestionnaire
    {

        #region Cbs

        [Score(Cbs = true, ScoreFactor = 2, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WhiteSpotsOnNails, Description = Strings.Labels.WhiteSpotsOnNailsLabel)]
        public bool WhiteSpotsOnNails { get; set; }

        [Score(Cbs = true, ScoreFactor = 2, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WhiteSpotsOnSkin)]
        public bool WhiteSpotsOnSkin { get; set; }

        [Score(Cbs = true, ScoreFactor = 2)]
        [Score(CardioVascularHealth = true, BloodBuilding = true, Restorative = true, IsYin = true, VataDosha = true, IsOxalateIntolerance = true, IsPhytateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Anaemia, Description = Strings.Labels.AnaemiaLabel)]
        public bool Anaemia { get; set; }

        [Score(Restorative = true, VataDosha = true, IsYin = true, Cbs = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PostExertionalMalaise, Description = Strings.Labels.PostExertionalMalaiseLabel)]
        public bool PostExertionalMalaise { get; set; }

        [Score(Restorative = true, IsOxalateIntolerance = true, VataDosha = true, Cbs = true, IsPhytateIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CrampsTremorsTwitches)]
        public bool CrampsTremorsTwitches { get; set; }

        [Score(Cbs = true, ScoreFactor = 2)]
        [Score(VataDosha = true, IsYin = true, IsOxalateIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HistoryOfchronicFatigueOrFibromyalgiaLabel)]
        public EYesNo? HistoryOfchronicFatigueOrFibromyalgia { get; set; }

        [Score(Cbs = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Migraines)]
        public bool Migraines { get; set; }

        [Score(Cbs = true, ScoreFactor = 2)]
        [Score(Restorative = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.POTS, Description = Strings.Labels.POTSLabel)]
        public bool POTS { get; set; }

        [Score(Cbs = true, ScoreFactor = 2)]
        [Score(Cognition = true, NeurologicalHealth = true, Mood = true, HormoneBalance = true, AntiInflammatory = true, Vitality = true, Restorative = true,
            IsYin = true, VataDosha = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DepressionAnxiety)]
        public bool DepressionAnxiety { get; set; }

        [Score(Cognition = true, NeurologicalHealth = true, Mood = true, Detoxification = true, StressRelief = true, CellularHealth = true, VataDosha = true, IsYin = true, ScoreFactor = 2, IsOxalateIntolerance = true, Cbs = true, IsPhytateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MemoryProblems)]
        public bool MemoryProblems { get; set; }

        [Score(Cognition = true, NeurologicalHealth = true, Mood = true, IsOxalateIntolerance = true, Cbs = true, IsPhytateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ConcentrationProblems)]
        public bool ConcentrationProblems { get; set; }

        [Score(Cbs = true, ScoreFactor = 2)]
        [Score(Cognition = true, NeurologicalHealth = true, Mood = true, VataDosha = true, IsYin = true, IsOxalateIntolerance = true, IsPhytateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BrainFog)]
        public bool BrainFog { get; set; }

        [Score(Sleep = true, StressRelief = true, Cognition = true, NeurologicalHealth = true, VataDosha = true, IsOxalateIntolerance = true, Cbs = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Insomnia)]
        public bool Insomnia { get; set; }

        [Score(Restorative = true, Detoxification = true, UrologicalHealth = true, Immunity = true, IsYin = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NightSweats)]
        public bool NightSweats { get; set; }

        [Score(HormoneBalance = true, StressRelief = true, Restorative = true, Sleep = true, VataDosha = true, IsYin = true, Cbs = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LowMorningEnergy)]
        public bool LowMorningEnergy { get; set; }

        [Score(Cognition = true, Mood = true, Sleep = true, NeurologicalHealth = true, IsOxalateIntolerance = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RacingThoughts)]
        public bool RacingThoughts { get; set; }

        [Score(Cognition = true, Mood = true, Sleep = true, NeurologicalHealth = true, IsOxalateIntolerance = true, Cbs = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.InnerTension)]
        public bool InnerTension { get; set; }

        [Score(NeurologicalHealth = true, VataDosha = true, Cbs = true, ScoreFactor = 2, IsLectinIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LoudNoisesBrightLightsLabel)]
        public EYesNo? LoudNoisesBrightLights { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Score(Cbs = true)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SulfiteSensitivityLabel)]
        public EYesNo? SulfiteSensitivity { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Score(Cbs = true)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MsgSensitivityLabel)]
        public EYesNo? MsgSensitivity { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Score(Cbs = true, ScoreFactor = 2)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CoarseThinEyebrowsLabel)]
        public EYesNo? CoarseThinEyebrows { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Score(Cbs = true, ScoreFactor = 3)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AmmoniaSmellLabel)]
        public EYesNo? AmmoniaSmell { get; set; }

        [Score(Restorative = true, HormoneBalance = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SugarCrashes, Description = Strings.Labels.SugarCrashesLabel)]
        public bool SugarCrashes { get; set; }

        [Score(Cbs = true, ScoreFactor = 3)]
        [Score(Cognition = true, Mood = true, Sleep = true, NeurologicalHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.OCDLabel)]
        public EYesNo? OCD { get; set; }

        [Score(Immunity = true, Detoxification = true, Cbs = true, IsOxalateIntolerance = true, IsPhytateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ChronicViralInfections)]
        public bool ChronicViralInfections { get; set; }

        [Score(CardioVascularHealth = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SpiderVeins)]
        public bool SpiderVeins { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Score(Cbs = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StretchMarks)]
        public bool StretchMarks { get; set; }

        [Score(Restorative = true, UrologicalHealth = true, StressRelief = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FrequentNighttimeUrination)]
        public bool FrequentNighttimeUrination { get; set; }

        [Score(Immunity = true, IsYang = true, PittaDosha = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Herpes)]
        public bool Herpes { get; set; }

        [Score(Cognition = true, Mood = true, Sleep = true, NeurologicalHealth = true, PittaDosha = true, IsYang = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Irritability)]
        public bool Irritability { get; set; }

        [Score(Cbs = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.Cbs)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PMS)]
        public bool PMS { get; set; }

        public List<PropertyInfo> GetCbsProperties() => this
            .GetProperties().Where(e => e.GetAttribute<QuestionCategoryAttribute>()?.Category == EQuestionCategory.Cbs).ToList();

        public int GetCbsScore() => GetScore(e => e.Cbs);

        #endregion

        public bool IsCbsAndMethylationComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.Cbs);
    }
}