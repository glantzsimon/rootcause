using DataAnnotationsExtensions;
using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Enums;
using K9.Base.DataAccessLayer.Extensions;
using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Constants;
using K9.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Enums;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Dictionary), ListName = Strings.Names.HealthQuestionnaires, PluralName = Strings.Names.HealthQuestionnaires, Name = Strings.Names.HealthQuestionnaire)]
    public partial class HealthQuestionnaire : ObjectBase
    {
        public HealthQuestionnaire()
        {
            CurrentSeason = GetCurrentSeasonInNorthernHemisphere();
        }

        [Required]
        public Guid ExternalId { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DietaryPreference)]
        public ESeason CurrentSeason { get; set; }

        public ESeason GetCurrentSeasonInNorthernHemisphere()
        {
            var today = DateTime.Today;
            if (today.Month >= 1 && today.Month <= 2)
            {
                return ESeason.Winter;
            }
            else if (today.Month == 3)
            {
                if (today.Day < 21)
                {
                    return ESeason.Winter;
                }
                else
                {
                    return ESeason.Spring;
                }
            }
            else if (today.Month >= 4 && today.Month <= 5)
            {
                return ESeason.Spring;
            }
            else if (today.Month == 6)
            {
                if (today.Day < 21)
                {
                    return ESeason.Spring;
                }
                else
                {
                    return ESeason.Summer;
                }
            }
            else if (today.Month >= 7 && today.Month <= 8)
            {
                return ESeason.Summer;
            }
            else if (today.Month == 9)
            {
                if (today.Day < 21)
                {
                    return ESeason.LateSummer;
                }
                else
                {
                    return ESeason.Autumn;
                }
            }
            else if (today.Month >= 10 && today.Month <= 11)
            {
                return ESeason.Autumn;
            }
            else if (today.Month == 12)
            {
                return ESeason.Winter;
            }

            return ESeason.Spring;
        }

        public bool IsComplete()
        {
            return IsGeneralHealthComplete() &&
                   IsPersonalInformationComplete() &&
                   IsDietaryPreferencesComplete() &&
                   IsFamilyHistoryComplete() &&
                   IsDentitionComplete() &&
                   IsAcetylationStatusComplete() &&
                   IsBiometricsComplete() &&
                   IsBloodAnalysisComplete() &&
                   IsDermatoglyphicsComplete() &&
                   IsCbsAndMethylationComplete() &&
                   IsDoshasComplete() &&
                   IsTasterStatusComplete();
        }

        public bool IsStarted()
        {
            return IsGeneralHealthComplete() ||
                   IsPersonalInformationComplete() ||
                   IsDietaryPreferencesComplete() ||
                   IsFamilyHistoryComplete() ||
                   IsAcetylationStatusComplete() ||
                   IsDentitionComplete() ||
                   IsBiometricsComplete() ||
                   IsBloodAnalysisComplete() ||
                   IsFamilyHistoryComplete() ||
                   IsDermatoglyphicsComplete() ||
                   IsTasterStatusComplete();
        }

        public EDisplayColor GetColorFromScore(int score)
        {
            double factor = 100f / 7f;
            double index = 0;

            if (score == 0)
            {
                index = 0;
            }
            else if (score == 100)
            {
                index = 6;
            }
            else
            {
                index = (int)Math.Floor(score / factor);
            }

            return (EDisplayColor)index;
        }

        #region Scores

        public int GetScore(Func<ScoreAttribute, bool> condition, Func<bool> condition2 = null, int condition2ScoreFactor = 1)
        {
            var scoreProperties = GetPropertiesWithScoreAttribute()
                .Select(e => new
                {
                    PropertyInfo = e,
                    ScoreAttributes = e.GetCustomAttributes<ScoreAttribute>().ToList()
                })
                .Where(e => e.ScoreAttributes.Any(s => condition(s)))
                .ToList();

            var answers = scoreProperties
                .Select(e =>
                {
                    var propertyValue = this.GetProperty(e.PropertyInfo);
                    var value = e.PropertyInfo.PropertyType == typeof(EYesNo?)
                            ? (EYesNo?)propertyValue == EYesNo.Yes
                            : (bool)propertyValue;

                    return new
                    {
                        value,
                        scoreFactor = e.ScoreAttributes.Sum(s => s.ScoreFactor)
                    };
                }).ToList();

            var yesses = answers.Where(e => e.value).ToList();
            var totalAnswers = answers.Sum(e => e.scoreFactor);
            var totalYesses = yesses.Sum(e => e.scoreFactor);

            if (condition2 != null)
            {
                totalAnswers += condition2ScoreFactor;

                if (condition2())
                {
                    totalYesses += condition2ScoreFactor;
                }
            }

            var yessesCount = yesses.Count;
            var score = yessesCount > 0 ? (((double)totalYesses / (double)totalAnswers)) : 0;

            // Up the score for a high yes count
            if (yessesCount >= 5)
            {
                score = IncreaseTowardsMaxScore(score, 1 / 4);
            }
            if (yessesCount >= 9)
            {
                score = IncreaseTowardsMaxScore(score, 1 / 2);
            }
            if (yessesCount >= 12)
            {
                score = IncreaseTowardsMaxScore(score, 3 / 4);
            }

            return (int)Math.Ceiling((double)score * 100);
        }

        public int GetDentalHealthScore()
        {
            return GetScore(e => e.DentalHealth,
                () => RootCanals, 3);
        }

        public int GetNeurologicalScore()
        {
            return GetScore(e => e.NeurologicalHealth);
        }

        public int GetCardiovascularScore()
        {
            return GetScore(e => e.CardioVascularHealth);
        }

        public int GetInflammationScore()
        {
            return GetScore(e => e.IsInflammation, () => (GetCbsScore() > 50 || GetNeurologicalScore() > 50 || GetDigestionIssuesScore() > 50), 3);
        }

        public int GetOxalateScore()
        {
            return GetScore(e => e.IsOxalateIntolerance, () => GetCbsScore() > 50, 5);
        }

        public int GetHistamineScore()
        {
            return GetScore(e => e.IsHistamineIntolerance);
        }

        public int GetPhytateScore()
        {
            return GetScore(e => e.IsPhytateIntolerance, () => GetImmunityIssuesScore() > 50, 5);
        }

        public int GetLectinsScore()
        {
            return GetScore(e => e.IsLectinIntolerance, () => GetImmunityIssuesScore() > 50, 5);
        }
 
        public int GetMycotoxinScore()
        {
            return GetScore(e => e.IsMycotoxinIntolerance, () => WeightGainEasy);
        }

        public int GetOmega6Score()
        {
            return GetScore(e => e.IsOmega6Intolerance, () => GetCardiovascularScore() > 30);
        }

        #endregion

        #region Css

        public bool IsPersonalDetailsActive() => !IsPersonalInformationComplete();

        public bool IsBloodAnalysisEnabled() => IsPersonalInformationComplete();

        public bool IsBloodAnalysisActive() => IsBloodAnalysisEnabled() &&
                                               !IsBloodAnalysisComplete();

        public bool IsAcetylationEnabled() => IsPersonalInformationComplete() &&
                                             IsBloodAnalysisComplete();

        public bool IsAcetylationActive() => IsAcetylationEnabled() &&
                                           !IsAcetylationStatusComplete();

        public bool IsBiometricsEnabled() => IsPersonalInformationComplete() &&
                                            IsBloodAnalysisComplete() &&
                                            IsAcetylationStatusComplete();

        public bool IsBiometricsActive() => IsBiometricsEnabled() &&
                                          !IsBiometricsComplete();

        public bool IsDermatoglyphicsEnabled() => IsPersonalInformationComplete() &&
                                                 IsBloodAnalysisComplete() &&
                                                 IsAcetylationStatusComplete() &&
                                                 IsBiometricsComplete();

        public bool IsDermatoglyphicsActive() => IsDermatoglyphicsEnabled() &&
                                               !IsDermatoglyphicsComplete();

        public bool IsDentitionEnabled() => IsPersonalInformationComplete() &&
                                           IsBloodAnalysisComplete() &&
                                           IsAcetylationStatusComplete() &&
                                           IsBiometricsComplete() &&
                                           IsDermatoglyphicsComplete();

        public bool IsDentitionActive() => IsDentitionEnabled() &&
                                         !IsDentitionComplete();

        public bool IsTasterStatusEnabled() => IsPersonalInformationComplete() &&
                                              IsBloodAnalysisComplete() &&
                                              IsAcetylationStatusComplete() &&
                                              IsBiometricsComplete() &&
                                              IsDermatoglyphicsComplete() &&
                                              IsDentitionComplete();

        public bool IsTasterStatusActive() => IsTasterStatusEnabled() &&
                                            !IsTasterStatusComplete();

        public bool IsFamilyHistoryEnabled() => IsPersonalInformationComplete() &&
                                               IsBloodAnalysisComplete() &&
                                               IsAcetylationStatusComplete() &&
                                               IsBiometricsComplete() &&
                                               IsDermatoglyphicsComplete() &&
                                               IsDentitionComplete() &&
                                               IsTasterStatusComplete();

        public bool IsFamilyHistoryActive() => IsFamilyHistoryEnabled() &&
                                             !IsFamilyHistoryComplete();


        public bool IsCbsAndMethylationEnabled() => IsPersonalInformationComplete() &&
                                                   IsBloodAnalysisComplete() &&
                                                   IsAcetylationStatusComplete() &&
                                                   IsBiometricsComplete() &&
                                                   IsDermatoglyphicsComplete() &&
                                                   IsDentitionComplete() &&
                                                   IsTasterStatusComplete() &&
                                                   IsFamilyHistoryComplete();

        public bool IsCbsAndMethylationActive() => IsCbsAndMethylationEnabled() &&
                                                   !IsCbsAndMethylationComplete();

        public bool IsDoshasEnabled() => IsPersonalInformationComplete() &&
                                        IsBloodAnalysisComplete() &&
                                        IsAcetylationStatusComplete() &&
                                        IsBiometricsComplete() &&
                                        IsDermatoglyphicsComplete() &&
                                        IsDentitionComplete() &&
                                        IsTasterStatusComplete() &&
                                        IsFamilyHistoryComplete() &&
                                        IsCbsAndMethylationComplete();

        public bool IsDoshasActive() => IsDoshasEnabled() &&
                                        !IsDoshasComplete();

        public bool IsGeneralHealthEnabled() => IsPersonalInformationComplete() &&
                                                IsBloodAnalysisComplete() &&
                                                IsAcetylationStatusComplete() &&
                                                IsBiometricsComplete() &&
                                                IsDermatoglyphicsComplete() &&
                                                IsDentitionComplete() &&
                                                IsTasterStatusComplete() &&
                                                IsCbsAndMethylationComplete() &&
                                                IsDoshasComplete() &&
                                                IsFamilyHistoryComplete();

        public bool IsGeneralHealthActive() => IsGeneralHealthEnabled() &&
                                               !IsGeneralHealthComplete();


        #region Personal Details

        public string GetPersonalDetailsActivePanelClass() =>
            IsPersonalDetailsActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetPersonalDetailsActiveTabClass() =>
            IsPersonalDetailsActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetPersonalDetailsCompleteHtml() =>
            IsPersonalInformationComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Blood Analysis

        public string GetBloodAnalysisActivePanelClass() =>
            IsBloodAnalysisActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetBloodAnalysisActiveTabClass() =>
            IsBloodAnalysisActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetBloodAnalysisEnabledClass() =>
            IsBloodAnalysisEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetBloodAnalysisToggle() =>
            IsBloodAnalysisEnabled() ? Strings.CssClasses.Pill : "";

        public string GetBloodAnalysisCompleteHtml() =>
            IsBloodAnalysisComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Acetylation

        public string GetAcetylationActivePanelClass() =>
            IsAcetylationActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetAcetylationActiveTabClass() =>
            IsAcetylationActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetAcetylationEnabledClass() =>
            IsAcetylationEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetAcetylationToggle() =>
            IsAcetylationEnabled() ? Strings.CssClasses.Pill : "";

        public string GetAcetylationCompleteHtml() =>
            IsAcetylationStatusComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Biometrics

        public string GetBiometricsActivePanelClass() =>
        IsBiometricsActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetBiometricsActiveTabClass() =>
            IsBiometricsActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetBiometricsEnabledClass() =>
            IsBiometricsEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetBiometricsToggle() =>
            IsBiometricsEnabled() ? Strings.CssClasses.Pill : "";

        public string GetBiometricsCompleteHtml() =>
            IsBiometricsComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Dermatoglyphics

        public string GetDermatoglyphicsActivePanelClass() =>
            IsDermatoglyphicsActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetDermatoglyphicsActiveTabClass() =>
            IsDermatoglyphicsActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetDermatoglyphicsEnabledClass() =>
            IsDermatoglyphicsEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetDermatoglyphicsToggle() =>
            IsDermatoglyphicsEnabled() ? Strings.CssClasses.Pill : "";

        public string GetDermatoglyphicsCompleteHtml() =>
            IsDermatoglyphicsComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Dentition

        public string GetDentitionActivePanelClass() =>
            IsDentitionActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetDentitionActiveTabClass() =>
            IsDentitionActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetDentitionEnabledClass() =>
            IsDentitionEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetDentitionToggle() =>
            IsDentitionEnabled() ? Strings.CssClasses.Pill : "";

        public string GetDentitionCompleteHtml() =>
            IsDentitionComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Taster Status

        public string GetTasterStatusActivePanelClass() =>
            IsTasterStatusActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetTasterStatusActiveTabClass() =>
            IsTasterStatusActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetTasterStatusEnabledClass() =>
            IsTasterStatusEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetTasterStatusToggle() =>
            IsTasterStatusEnabled() ? Strings.CssClasses.Pill : "";

        public string GetTasterStatusCompleteHtml() =>
            IsTasterStatusComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Family History

        public string GetFamilyHistoryActivePanelClass() =>
            IsFamilyHistoryActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetFamilyHistoryActiveTabClass() =>
            IsFamilyHistoryActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetFamilyHistoryEnabledClass() =>
            IsFamilyHistoryEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetFamilyHistoryToggle() =>
            IsFamilyHistoryEnabled() ? Strings.CssClasses.Pill : "";

        public string GetFamilyHistoryCompleteHtml() =>
            IsFamilyHistoryComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region General Health

        public string GetGeneralHealthActivePanelClass() =>
            IsGeneralHealthActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetGeneralHealthActiveTabClass() =>
            IsGeneralHealthActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetGeneralHealthEnabledClass() =>
            IsGeneralHealthEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetGeneralHealthToggle() =>
            IsGeneralHealthEnabled() ? Strings.CssClasses.Pill : "";

        public string GetGeneralHealthCompleteHtml() =>
            IsGeneralHealthComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Cbs And Methylation

        public string GetCbsAndMethylationActivePanelClass() =>
            IsCbsAndMethylationActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetCbsAndMethylationActiveTabClass() =>
            IsCbsAndMethylationActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetCbsAndMethylationEnabledClass() =>
            IsCbsAndMethylationEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetCbsAndMethylationToggle() =>
            IsCbsAndMethylationEnabled() ? Strings.CssClasses.Pill : "";

        public string GetCbsAndMethylationCompleteHtml() =>
            IsCbsAndMethylationComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #region Doshas

        public string GetDoshasActivePanelClass() =>
            IsDoshasActive() ? Strings.CssClasses.ActivePanelClass : "";

        public string GetDoshasActiveTabClass() =>
            IsDoshasActive() ? Strings.CssClasses.ActiveTabClass : "";

        public string GetDoshasEnabledClass() =>
            IsDoshasEnabled() ? "" : Strings.CssClasses.DisabledClass;

        public string GetDoshasToggle() =>
            IsDoshasEnabled() ? Strings.CssClasses.Pill : "";

        public string GetDoshasCompleteHtml() =>
            IsDoshasComplete() ? "<i class=\"fa fa-check-circle green\"></i>" : "";

        #endregion

        #endregion

        #region Personal Details

        [UIHint("Client")]
        [ForeignKey("Client")]
        [Required]
        [Index(IsUnique = true)]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        [QuestionCategory(Category = EQuestionCategory.PersonalDetails)]
        [UIHint("Gender")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.GenderLabel)]
        public EGender? Gender { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.PersonalDetails, AllowNull = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IsLGBTQPlusLabel)]
        public EYesNo? IsIsLGBTQPlus { get; set; }

        [QuestionCategory(Category = EQuestionCategory.PersonalDetails)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DateOfBirthLabel)]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Base.Globalisation.Dictionary), Name = Base.Globalisation.Strings.Labels.LanguageLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string GenderName => Gender?.GetLocalisedLanguageName();

        public string GenderPossessivePronoun => Gender == EGender.Male ? Dictionary.His : Gender == EGender.Female ? Dictionary.Hers : Dictionary.Theirs;

        public string GenderPronoun => Gender == EGender.Male ? Dictionary.He : Gender == EGender.Female ? Dictionary.She : Dictionary.They;

        public int? YearsOld => GetYearsOld();

        public bool IsAdult() => YearsOld >= 18;

        public bool IsSixteenOrOver() => YearsOld >= 16;

        public NineStarKiModel GetNineStarKiModel()
        {
            var client = new System.Net.WebClient();
            if (DateOfBirth.HasValue)
            {
                var requestString =
                    $"https://9starki.org/api/calculate?dateOfBirth={DateOfBirth.Value.ToString(FormatConstants.ApiDateTimeFormat)}&gender={Gender}";
                var nineStarKiJson = client.DownloadString(requestString);
                var serializer = new JavaScriptSerializer();

                return serializer.Deserialize<NineStarKiModel>(nineStarKiJson);
            }

            return null;
        }

        public ZodiacModel GetZodiacModel()
        {
            return DateOfBirth.HasValue
                ? new ZodiacModel(DateOfBirth.Value)
                : null;
        }

        private int? GetYearsOld()
        {
            return DateOfBirth.HasValue ? (DateTime.Now.Year - DateOfBirth.Value.Year) - (DateTime.Now.DayOfYear < DateOfBirth.Value.DayOfYear ? 1 : 0) : (int?)null;
        }

        public bool IsPersonalInformationComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.PersonalDetails);

        #endregion

        #region Dietary Preferences

        [QuestionCategory(Category = EQuestionCategory.PersonalDetails, AllowNull = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DietaryPreference)]
        public EDietaryPreference? DietaryPreference { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.RedMeat)]
        public bool EatsRedMeat { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.Poultry)]
        public bool EatsPoultry { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.FishAndSeafood)]
        public bool EatsFishAndSeafood { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.EggsAndRoes)]
        public bool EatsEggsAndRoes { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.Dairy)]
        public bool EatsDairy { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.Vegetables)]
        public bool EatsVegetables { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.VegetableProtein)]
        public bool EatsVegetableProtein { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.Fungi)]
        public bool EatsFungi { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.Fruit)]
        public bool EatsFruit { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.Grains)]
        public bool EatsGrains { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowOxalateLabel)]
        public bool IsLowOxalate { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowLectinLabel)]
        public bool IsLowLectin { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowPhytateLabel)]
        public bool IsLowPhytate { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowHistamineLabel)]
        public bool IsLowHistamine { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowMycotoxinLabel)]
        public bool IsLowMycotoxin { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowOmega6Label)]
        public bool IsLowOmega6 { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsBulletProofLabel)]
        public bool IsBulletProof { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsSattvicLabel)]
        public bool IsSattvic { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowSulphur)]
        public bool IsLowSulphur { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsLowGoitrogen)]
        public bool IsLowGoitrogen { get; set; }

        [QuestionCategory(Category = EQuestionCategory.DietaryPreferences)]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsKeto)]
        public bool IsKeto { get; set; }

        public bool IsDietaryPreferencesComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.DietaryPreferences);

        public List<PropertyInfo> GeneralDietaryPreferencesProperties() => this
            .GetProperties().Where(e => e.GetAttribute<QuestionCategoryAttribute>()?.Category == EQuestionCategory.DietaryPreferences).ToList();

        #endregion

        #region Cardiovascular Health

        [Score(CardioVascularHealth = true, PittaDosha = true, IsYang = true, IsInflammation = true, IsOxalateIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HighBloodPressure)]
        public bool HighBloodPressure { get; set; }

        [Score(CardioVascularHealth = true, VataDosha = true, IsYin = true, IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LowBloodPressure)]
        public bool LowBloodPressure { get; set; }

        [Score(CardioVascularHealth = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ChestPain)]
        public bool ChestPain { get; set; }

        [Score(CardioVascularHealth = true, IsOxalateIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Palpitations)]
        public bool Palpitations { get; set; }

        [Score(CardioVascularHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EasilyOutOfBreath)]
        public bool EasilyOutOfBreath { get; set; }

        [Score(CardioVascularHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Stroke, Description = Strings.Labels.StrokeLabel)]
        public bool Stroke { get; set; }

        [Score(CardioVascularHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HeartAttack, Description = Strings.Labels.HeartAttackLabel)]
        public bool HeartAttack { get; set; }

        #endregion

        #region Digestive Health

        [Score(DigestiveHealth = true, VataDosha = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Bloating)]
        public bool Bloating { get; set; }

        [Score(DigestiveHealth = true, PittaDosha = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IrritableBowel)]
        public bool IBS { get; set; }

        [Score(DigestiveHealth = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Hiccups)]
        public bool Hiccups { get; set; }

        [Score(DigestiveHealth = true, VataDosha = true, IsOxalateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Gas)]
        public bool Gas { get; set; }

        [Score(DigestiveHealth = true, PittaDosha = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LooseStool)]
        public bool LooseStool { get; set; }

        [Score(DigestiveHealth = true, VataDosha = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true, IsMycotoxinIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Constipation)]
        public bool Constipation { get; set; }

        [Score(DigestiveHealth = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AbdominalPainOrCramping)]
        public bool AbdominalPainOrCramping { get; set; }

        [Score(DigestiveHealth = true, IsYang = true, PittaDosha = true, IsOmega6Intolerance = true, IsLectinIntolerance = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SkinIssues)]
        public bool SkinIssues { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SkinIssuesDetailsLabel)]
        [StringLength(1111)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth, AllowNull = true)]
        [DataType(DataType.MultilineText)]
        public string SkinIssuesDetails { get; set; }

        [Score(DigestiveHealth = true, Immunity = true, Detoxification = true, DentalHealth = true, KaphaDosha = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CoatedTongue, Description = Strings.Labels.CoatedTongueLabel)]
        public bool CoatedTongue { get; set; }

        [Score(DigestiveHealth = true, Immunity = true, Detoxification = true, IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DiverticulitisLabel)]
        public bool Diverticulitis { get; set; }

        [Score(DigestiveHealth = true, Immunity = true, Detoxification = true, IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeakyGut)]
        public bool LeakyGut { get; set; }

        [Score(Restorative = true, Immunity = true, IsYin = true, IsOxalateIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CandidaFungus)]
        public bool CandidaAndFungus { get; set; }

        [Score(DigestiveHealth = true, IsPhytateIntolerance = true, IsOxalateIntolerance = true, Cbs = true, IsYin = true, VataDosha = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LossOfAppetite)]
        public bool LossOfAppetite { get; set; }

        [Score(DigestiveHealth = true, IsHistamineIntolerance = true, IsPhytateIntolerance = true, IsOxalateIntolerance = true, Cbs = true, IsYin = true, VataDosha = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Vomiting)]
        public bool Vomiting { get; set; }

        [Score(DigestiveHealth = true, IsHistamineIntolerance = true, IsPhytateIntolerance = true, IsOxalateIntolerance = true, Cbs = true, IsYin = true, VataDosha = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Nausea)]
        public bool Nauseas { get; set; }

        public int GetDigestionIssuesScore()
        {
            return GetScore(e => e.DigestiveHealth);
        }

        #endregion

        #region Immune System

        [Score(Immunity = true)]
        [UIHint("Severity")]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.InfectionSeverityLabel)]
        public ESeverity? InfectionSeverity { get; set; }

        [Score(Immunity = true, Detoxification = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AllergiesAndSensitivitiesLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string AllergiesAndSensitivitiesDetails { get; set; }

        public int GetImmunityIssuesScore()
        {
            return GetScore(e => e.Immunity,
                () => GetDigestionIssuesScore() > 0);
        }

        #endregion

        #region Yin Disease

        [Score(Restorative = true, IsYin = true, VataDosha = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ColdExtremities)]
        public bool ColdExtremities { get; set; }

        [Score(Restorative = true, IsYin = true, VataDosha = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ColdIntolerant)]
        public bool ColdIntolerant { get; set; }

        public int GetYinImbalanceScore()
        {
            return GetScore(e => e.IsYin);
        }

        #endregion

        #region Respiratory Health

        [Score(Immunity = true, IsOxalateIntolerance = true, RespiratoryHealth = true, IsOmega6Intolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Asthma)]
        public bool Asthma { get; set; }

        [Score(Immunity = true, IsOxalateIntolerance = true, RespiratoryHealth = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BronchitisLabel)]
        public bool Bronchitis { get; set; }

        [Score(Immunity = true, IsOxalateIntolerance = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PulmonaryFibrosis, Description = Strings.Labels.PulmonaryFibrosisInfo)]
        public bool PulmonaryFibrosis { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.COPD, Description = Strings.Labels.COPDInfo)]
        public bool COPD { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CysticFibrosis, Description = Strings.Labels.CysticFibrosisInfo)]
        public bool CysticFibrosis { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Pneumonia)]
        public bool Pneumonia { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Tuberculosis)]
        public bool Tuberculosis { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LungCancer)]
        public bool LungCancer { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true, CardioVascularHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Dyspnea, Description = Strings.Labels.DyspneaDetails)]
        public bool Dyspnea { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true, KaphaDosha = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MucusInLungs)]
        public bool MucusInLungs { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Wheezing)]
        public bool Wheezing { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ChronicCough)]
        public bool ChronicCough { get; set; }

        [Score(Immunity = true, RespiratoryHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TightnessInChest)]
        public bool TightnessInChest { get; set; }

        public int GetRespiratoryHealthScore => GetScore(e => e.RespiratoryHealth);

        #endregion

        #region Inflammation

        [Score(AntiInflammatory = true, Immunity = true, Detoxification = true, IsYang = true, PittaDosha = true, IsInflammation = true, IsOxalateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.JointInflammation, Description = Strings.Labels.JointInflammationLabel)]
        public bool JointInflammation { get; set; }

        [Score(AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CarpelTunnel, Description = Strings.Labels.CarpelTunnelLabel)]
        public bool CarpelTunnel { get; set; }

        [Score(AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BackPain)]
        public bool BackPain { get; set; }

        [Score(AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MusclePain)]
        public bool MusclePain { get; set; }

        [Score(AntiInflammatory = true, Detoxification = true, Immunity = true, IsYang = true, PittaDosha = true, IsInflammation = true, IsOxalateIntolerance = true, IsLectinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AutoImmunity, Description = Strings.Labels.AutoImmunityLabel)]
        public bool Autoimmunity { get; set; }

        [Score(AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CataractsLabel)]
        public bool Cataracts { get; set; }

        [Score(Immunity = true, UrologicalHealth = true, Detoxification = true, IsYin = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.UTI)]
        public bool UTI { get; set; }

        [Score(Immunity = true, UrologicalHealth = true, Detoxification = true, IsYin = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.KidneyStones)]
        public bool KidneyStones { get; set; }

        [Score(Immunity = true, UrologicalHealth = true, Detoxification = true, IsYin = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DifficultyUrinating)]
        public bool DifficultyUrinating { get; set; }

        [Score(Immunity = true, UrologicalHealth = true, Detoxification = true, IsYin = true, IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.InterstitialCystitis, Description = Strings.Labels.InterstitialCystitisInfo)]
        public bool InterstitialCystitis { get; set; }

        [Score(Immunity = true, UrologicalHealth = true, Detoxification = true, IsYin = true, IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EndometriosisLabel)]
        public bool Endometriosis { get; set; }

        [Score(AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true, IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EyePainLabel)]
        public bool EyePain { get; set; }

        [Score(DigestiveHealth = true, AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MastCellActivation)]
        public bool MastCellActivation { get; set; }

        public int GetYangImbalanceScore()
        {
            return GetScore(e => e.IsYang);
        }

        #endregion

        #region Other

        [Score(IsOmega6Intolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Type2Diabetes)]
        public bool Type2Diabetes { get; set; }

        [Score(IsOmega6Intolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MetabolicSyndrome, Description = Strings.Labels.MetabolicSyndromeInfo)]
        public bool MetabolicSyndroms { get; set; }

        [Score(IsMycotoxinIntolerance = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Tinnitus, Description = Strings.Labels.TinnitusInfo)]
        public bool Tinnitus { get; set; }

        [Score(IsMycotoxinIntolerance = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MetalicTasteInMouth)]
        public bool MetalicTasteInMouth { get; set; }

        [Score(PittaDosha = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IncreasedThirst)]
        public bool IncreasedThirst { get; set; }

        [Score(IsOxalateIntolerance = true, IsLectinIntolerance = true, IsPhytateIntolerance = true, Cbs = true, VataDosha = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FrequentlySick, Description = Strings.Labels.FrequentlySickInfo)]
        public bool FrequentlySick { get; set; }

        [Score(IsOxalateIntolerance = true, IsLectinIntolerance = true, IsPhytateIntolerance = true, Cbs = true, VataDosha = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Numbness)]
        public bool Numbness { get; set; }

        [Score(IsOxalateIntolerance = true, IsLectinIntolerance = true, IsPhytateIntolerance = true, Cbs = true, IsYin = true, VataDosha = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MuscleWeakness)]
        public bool MuscleWeakness { get; set; }

        [Score(IsPhytateIntolerance = true, IsOxalateIntolerance = true, IsLectinIntolerance = true, HormoneBalance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HormoneImbalance)]
        public bool HormoneImbalance { get; set; }

        [Score(IsPhytateIntolerance = true, IsOxalateIntolerance = true, DigestiveHealth = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true, IsOmega6Intolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Rashes)]
        public bool Rashes { get; set; }

        [Score(IsPhytateIntolerance = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Impotency)]
        public bool Impotency { get; set; }

        [Score(IsHistamineIntolerance = true, IsPhytateIntolerance = true, IsOxalateIntolerance = true, Cbs = true, IsYin = true, VataDosha = true, IsLectinIntolerance = true, IsMycotoxinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Fatigue)]
        public bool Fatigue { get; set; }

        [Score(IsHistamineIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Allergies)]
        public bool Allergies { get; set; }

        [Score(IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LackOfCoordination)]
        public bool LackOfCoordination { get; set; }

        [Score(IsOxalateIntolerance = true, IsOmega6Intolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.VisionProblems)]
        public bool VisionProblems { get; set; }

        [Score(IsHistamineIntolerance = true, NeurologicalHealth = true, IsMycotoxinIntolerance = true, Cbs = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Dizziness)]
        public bool Dizziness { get; set; }

        [Score(IsHistamineIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AbdominalDistention)]
        public bool AbdominalDistention { get; set; }

        [Score(IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Edema)]
        public bool Edema { get; set; }

        [Score(IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Hives)]
        public bool Hives { get; set; }

        [Score(Cognition = true, Mood = true, NeurologicalHealth = true, IsHistamineIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ADHD, Description = Strings.Labels.ADHDLabel)]
        public bool ADHD { get; set; }

        [Score(IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IrregularMenstruation)]
        public bool IrregularMenstruation { get; set; }

        [Score(IsHistamineIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NasalCongestion)]
        public bool NasalCongestion { get; set; }

        [Score(IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FlushingAndRedness)]
        public bool FlushingAndRedness { get; set; }

        [Score(IsHistamineIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Itching)]
        public bool Itching { get; set; }

        [Score(IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HypoThyroidism)]
        public bool HypoThyroidism { get; set; }

        [Score(IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SandyEye)]
        public bool SandyEye { get; set; }

        [Score(IsOxalateIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PCOS, Description = Strings.Labels.PCOSInfo)]
        public bool PCOS { get; set; }

        [Score(IsOxalateIntolerance = true, DigestiveHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.GrainyStools)]
        public bool GrainyStools { get; set; }

        [Score(IsOxalateIntolerance = true, IsYin = true, VataDosha = true, IsPhytateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Osteoporosis, Description = Strings.Labels.OsteoporosisInfo)]
        public bool Osteoporosis { get; set; }

        [Score(IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Hemorrhoids)]
        public bool Hemorrhoids { get; set; }

        [Score(IsOxalateIntolerance = true, IsPhytateIntolerance = true, IsMycotoxinIntolerance = true, IsLectinIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HairLossLabel)]
        public bool HairLoss { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SurgeryDetailsLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string SurgeryDetails { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PrescriptionMedicationLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string PrescriptionMedication { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SupplementsLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string Supplements { get; set; }

        [Score(Detoxification = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SmokeLabel)]
        public EYesNo? Smoke { get; set; }

        [Score(Cbs = true, IsYin = true, Restorative = true, VataDosha = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CovidVaccine)]
        public EYesNo? CovidVaccine { get; set; }

        [Score(Cbs = true, IsYin = true, Restorative = true, VataDosha = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LongCovid)]
        public EYesNo? LongCovid { get; set; }

        [Score(NeurologicalHealth = true, Mood = true, Cognition = true, StressRelief = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DrinksAlcoholLabel)]
        public EYesNo? DrinksAlcohol { get; set; }

        [FileSourceInfo("upload/health", Filter = EFilesSourceFilter.Images)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PhotoOfEyesLabel)]
        public FileSource EyesFileSource { get; set; }

        #endregion

        #region Trauma

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PhysicalTraumaLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string PhysicalTraumaDetails { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EmotionalTrauma)]
        public EYesNo? EmotionalTrauma { get; set; }

        #endregion

        #region Oral Health

        [Score(AntiInflammatory = true, IsInflammation = true, IsOxalateIntolerance = true, DentalHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BurningMouth)]
        public bool BurningMouth { get; set; }

        [Score(Detoxification = true, DentalHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AmalgamFillings,  Description = Strings.Labels.AmalgamFillingsLabel)]
        public bool AmalgamFillings { get; set; }

        [Score(Detoxification = true, DentalHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AmalgamFillingsHistory, Description = Strings.Labels.AmalgamFillingsHistoryLabel)]
        public bool AmalgamFillingsHistory { get; set; }

        [Score(Immunity = true, DentalHealth = true, Restorative = true, IsPhytateIntolerance = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ToothPain)]
        public bool ToothPain { get; set; }

        [Score(Immunity = true, DentalHealth = true,  IsPhytateIntolerance = true, IsOxalateIntolerance = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TMJ)]
        public bool TMJ { get; set; }

        [Score(Immunity = true, DentalHealth = true, Restorative = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CrackedTeeth)]
        public bool CrackedTeeth { get; set; }

        [Score(Immunity = true, DentalHealth = true, Restorative = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.Cavities)]
        public bool Cavities { get; set; }

        [Score(Immunity = true, DentalHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RootCanals)]
        public bool RootCanals { get; set; }

        [Score(Immunity = true, DentalHealth = true)]
        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DentalAbsesses)]
        public bool DentalAbcsesses { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.DentalIssuesLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string DentalIssues { get; set; }

        #endregion

        #region General Health 

        public List<PropertyInfo> GeneralHealthProperties() => this
            .GetProperties().Where(e => e.GetAttribute<QuestionCategoryAttribute>()?.Category == EQuestionCategory.GeneralHealth).ToList();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CurrentHealthIssuesLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string CurrentHealthIssues { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HealthGoalsLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string HealthGoals { get; set; }

        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [UIHint("Range")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CurrentHealthLevelLabel)]
        [Min(1)]
        [Max(10)]
        public int? CurrentHealthLevel { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AutomaticallyFilterFoodsLabel)]
        public bool AutomaticallyFilterFoods { get; set; }

        /// <summary>
        /// Enables users to use the health level slider and automatically filter out problematic foods
        /// </summary>
        /// <returns></returns>
        public int GetScoreThreshold() => (int)CurrentHealthLevel * 10;

        public int GetCurrentHealthScore() => CurrentHealthLevel.HasValue ? CurrentHealthLevel.Value * 10 : 0;

        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [UIHint("Range")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NutritionExpertiseLevelLabel)]
        [Min(1)]
        [Max(10)]
        public int? NutritionExpertiseLevel { get; set; }

        public int GetNutritionExpertiseScore() => NutritionExpertiseLevel.HasValue ? NutritionExpertiseLevel.Value * 10 : 0;

        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EnjoysCookingLabel)]
        public EYesNo? EnjoysCooking { get; set; }

        [QuestionCategory(Category = EQuestionCategory.GeneralHealth)]
        [UIHint("Frequency")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CookingFrequencyLabel)]
        public EFrequency? CookingFrequency { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ExerciseLabel)]
        [StringLength(1111)]
        [DataType(DataType.MultilineText)]
        public string Exercise { get; set; }

        public bool IsGeneralHealthComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.GeneralHealth);

        #endregion

        public static List<PropertyInfo> GetPropertiesWithScoreAttribute()
        {
            return typeof(HealthQuestionnaire).GetProperties().Where(e => e.GetAttribute<ScoreAttribute>() != null && e.PropertyType == typeof(EYesNo?) || e.PropertyType == typeof(EYesNo) || e.PropertyType == typeof(bool)).ToList();
        }

        public static List<PropertyInfo> GetPropertiesWithQuestionCategoryAttribute()
        {
            return typeof(HealthQuestionnaire).GetProperties().Where(e => e.GetAttribute<QuestionCategoryAttribute>() != null).ToList();
        }

        private bool IsCategoryComplete(Func<QuestionCategoryAttribute, bool> condition)
        {
            var categoryQuestions = GetPropertiesWithQuestionCategoryAttribute()
                .Select(e => new
                {
                    PropertyInfo = e,
                    QuestionCategoryAttribute = e.GetAttribute<QuestionCategoryAttribute>()
                })
                .Where(e => condition(e.QuestionCategoryAttribute)).ToList();

            return categoryQuestions
                .All(e =>
                       {
                           var value = this.GetProperty(e.PropertyInfo);
                           var valueIsNull = value == null;
                           var isComplete = !valueIsNull
                                    && (e.QuestionCategoryAttribute.MustBeGreaterThanZero
                                        ? double.Parse(value.ToString()) > 0
                                        : true)
                                    || e.QuestionCategoryAttribute.AllowNull;
                           
                           return isComplete;
                       });
        }

        private static double IncreaseTowardsMaxScore(double initialValue, double fraction)
        {
            if (initialValue < 0 || initialValue > 100)
            {
                throw new ArgumentOutOfRangeException("InitialValue must be between 0 and 100.");
            }

            var remainingDistance = 100 - initialValue;
            var increase = remainingDistance * fraction;
            var newValue = initialValue + increase;

            return Math.Min(newValue, 100);
        }

    }
}
