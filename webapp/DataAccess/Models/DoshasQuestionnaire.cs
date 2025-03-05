using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace K9.DataAccessLayer.Models
{
    public partial class HealthQuestionnaire
    {
        #region Skin

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true, IsYin = true, IsOxalateIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.DrySkin)]
        public bool DrySkin { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.ThickSkin)]
        public bool ThickSkin { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true, IsYang = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.OilySkin)]
        public bool OilySkin { get; set; }

        #endregion

        #region Sleep

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.LightSleep)]
        public bool LightSleep { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.ModerateSleep)]
        public bool ModerateSleep { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.DeepSleep)]
        public bool DeepSleep { get; set; }

        #endregion

        #region Weight

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.WeightGainDifficult)]
        public bool WeightGainDifficult { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.WeightGainModerate)]
        public bool WeightGainModerate { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.WeightGainEasy)]
        public bool WeightGainEasy { get; set; }

        #endregion

        #region Temperature

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.BodyColdLabel)]
        public bool BodyCold { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.BodyHotLabel)]
        public bool BodyHot { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.BodyColdMoistLabel)]
        public bool BodyColdMoist { get; set; }

        #endregion

        #region Stress

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.StressAnxiousLabel)]
        public bool StressAnxious { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.StressIrritableLabel)]
        public bool StressIrritable { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.StressReclusiveLabel)]
        public bool StressReclusive { get; set; }

        #endregion

        #region Eyes

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.SmallEyesLabel)]
        public bool SmallEyes { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.PenetratingEyesLabel)]
        public bool PenetratingEyes { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.LargeEyesLabel)]
        public bool LargeEyes { get; set; }

        #endregion

        #region Disposition

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.LivelyDispositionLabel)]
        public bool LivelyDisposition { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.DrivenDispositionLabel)]
        public bool DrivenDisposition { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.EvenDispositionLabel)]
        public bool EvenDisposition { get; set; }

        #endregion

        #region Hair

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Vata)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.FrizzyHairTypeLabel)]
        public bool FrizzyHairType { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Pitta)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.FineHairTypeLabel)]
        public bool FineHairType { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Doshas, Dosha = EDosha.Kapha)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.FullHairTypeLabel)]
        public bool FullHairType { get; set; }

        #endregion

        #region Signs Of Vata Dosha Imbalance

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsYin = true, IsOxalateIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.NervousnessLabel)]
        public EYesNo? Nervousness { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsOxalateIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.SpasmsLabel)]
        public EYesNo? Spasms { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsYin = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.LowVigourLabel)]
        public EYesNo? LowVigour { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsYin = true, IsMycotoxinIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.WeightLossLabel)]
        public EYesNo? WeightLoss { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsOxalateIntolerance = true, IsMycotoxinIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.NeuropathicPainLabel)]
        public EYesNo? NeuropathicPain { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsOxalateIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.DryFlakySkinLabel)]
        public EYesNo? DryFlakySkin { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true, IsOxalateIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.IrregularDigestionLabel)]
        public EYesNo? IrregularDigestion { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.TroubleFallingAsleepLabel)]
        public EYesNo? TroubleFallingAsleep { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(VataDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.FrettingLabel)]
        public EYesNo? Fretting { get; set; }

        #endregion

        #region Signs of Pitta Dosha Imbalance

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true, IsOxalateIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UlcersAcidRefluxLabel)]
        public EYesNo? UlcersAcidReflux { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true, IsPhytateIntolerance = true, IsLectinIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.NauseaLabel)]
        public EYesNo? Nausea { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.AcneLabel)]
        public EYesNo? Acne { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.HyperMetabolismLabel)]
        public EYesNo? HyperMetabolism { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.ExcessBodyHeatLabel)]
        public EYesNo? ExcessBodyHeat { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.AngerFrustrationLabel)]
        public EYesNo? AngerFrustration { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.ImpatientIdealisticLabel)]
        public EYesNo? ImpatientIdealistic { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.RedEyesLabel)]
        public EYesNo? RedEyes { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true, IsMycotoxinIntolerance = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.LightSensitiveLabel)]
        public EYesNo? SensitiveToLight { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(PittaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.HighLibidoLabel)]
        public EYesNo? HighLibido { get; set; }

        #endregion

        #region Signs of Kapha Dosha Imbalance

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.ExcessMucousLabel)]
        public EYesNo? ExcessMucous { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.SlowInfrequentBowelMovementsLabel)]
        public EYesNo? SlowInfrequentBowelMovements { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.MoodDrivenEatingLabel)]
        public EYesNo? MoodDrivenEating { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.EasyWeightGainLabel)]
        public EYesNo? EasyWeightGain { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.HardToWakeUpLabel)]
        public EYesNo? HardToWakeUp { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.LethargicLabel)]
        public EYesNo? Lethargic { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.PossessiveLabel)]
        public EYesNo? Possessive { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Doshas)]
        [Score(KaphaDosha = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.ObstinacyLabel)]
        public EYesNo? Obstinacy { get; set; }

        #endregion

        public bool IsDoshasComplete()
        {
            return IsCategoryComplete(e => e.Category == EQuestionCategory.Doshas);
        }

        public Doshas GetPrakrutiDoshas(HealthQuestionnaire hq)
        {
            var results = new List<EDosha>();

            if (DrySkin)
            {
                results.Add(GetType().GetProperty(nameof(DrySkin)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (OilySkin)
            {
                results.Add(GetType().GetProperty(nameof(OilySkin)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (ThickSkin)
            {
                results.Add(GetType().GetProperty(nameof(ThickSkin)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (LightSleep)
            {
                results.Add(GetType().GetProperty(nameof(LightSleep)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (ModerateSleep)
            {
                results.Add(GetType().GetProperty(nameof(ModerateSleep)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (DeepSleep)
            {
                results.Add(GetType().GetProperty(nameof(DeepSleep)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (WeightGainDifficult)
            {
                results.Add(GetType().GetProperty(nameof(WeightGainDifficult)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (WeightGainModerate)
            {
                results.Add(GetType().GetProperty(nameof(WeightGainModerate)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (WeightGainEasy)
            {
                results.Add(GetType().GetProperty(nameof(WeightGainEasy)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (BodyCold)
            {
                results.Add(GetType().GetProperty(nameof(BodyCold)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (BodyHot)
            {
                results.Add(GetType().GetProperty(nameof(BodyHot)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (BodyColdMoist)
            {
                results.Add(GetType().GetProperty(nameof(BodyColdMoist)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (SmallEyes)
            {
                results.Add(GetType().GetProperty(nameof(SmallEyes)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (PenetratingEyes)
            {
                results.Add(GetType().GetProperty(nameof(PenetratingEyes)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (LargeEyes)
            {
                results.Add(GetType().GetProperty(nameof(LargeEyes)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (LivelyDisposition)
            {
                results.Add(GetType().GetProperty(nameof(LivelyDisposition)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (DrivenDisposition)
            {
                results.Add(GetType().GetProperty(nameof(DrivenDisposition)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (EvenDisposition)
            {
                results.Add(GetType().GetProperty(nameof(EvenDisposition)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (FrizzyHairType)
            {
                results.Add(GetType().GetProperty(nameof(FrizzyHairType)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (FineHairType)
            {
                results.Add(GetType().GetProperty(nameof(FineHairType)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (FullHairType)
            {
                results.Add(GetType().GetProperty(nameof(FullHairType)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (StressAnxious)
            {
                results.Add(GetType().GetProperty(nameof(StressAnxious)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (StressIrritable)
            {
                results.Add(GetType().GetProperty(nameof(StressIrritable)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            if (StressReclusive)
            {
                results.Add(GetType().GetProperty(nameof(StressReclusive)).GetAttribute<QuestionCategoryAttribute>().Dosha);
            }

            var totalDoshas = results.Count;
            var vataCount = results.Count(e => e == EDosha.Vata);
            var pittaCount = results.Count(e => e == EDosha.Pitta);
            var kaphaCount = results.Count(e => e == EDosha.Kapha);

            return new Doshas
            {
                VataDoshaScore = vataCount > 0 ? (double)vataCount / totalDoshas * 100 : 0,
                PittaDoshaScore = pittaCount > 0 ? (double)pittaCount / totalDoshas * 100 : 0,
                KaphaDoshaScore = kaphaCount > 0 ? (double)kaphaCount / totalDoshas * 100 : 0
            };
        }

        public Doshas GetVikrutiDoshas()
        {
            var vataDoshaScore = GetScore(e => e.VataDosha);
            var pittaDoshaScore = GetScore(e => e.PittaDosha);
            var kaphaDoshaScore = GetScore(e => e.KaphaDosha);

            return new Doshas
            {
                VataDoshaScore = vataDoshaScore,
                PittaDoshaScore = pittaDoshaScore,
                KaphaDoshaScore = kaphaDoshaScore
            };
        }

    }
}
