using K9.Base.DataAccessLayer.Enums;
using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.SharedLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace K9.DataAccessLayer.Models
{
    public partial class HealthQuestionnaire
    {
        public GenoTypeStrengthTestResult CalculateGenotype()
        {
            return GetGroupedStrengthTestResults()?
                .FilteredResults
                .FirstOrDefault();
        }

        public GenoTypeStrengthTestResults GetGroupedStrengthTestResults()
        {
            return new GenoTypeStrengthTestResults
            {
                GenoTypes = CalculateGenoTypes(),
                Results = GetGenoTypesStrengthTestResults()
                    .GroupBy(e => e).Select(group => new GenoTypeStrengthTestResult
                    {
                        GenoType = group.Key,
                        Count = group.Count(),
                    })
                    .OrderByDescending(e => e.Count)
                    .ToList()
            };
        }

        public List<EGenoType> GetGenoTypesStrengthTestResults()
        {
            var genoTypesStrengthTestResults = GetGenoTypeFromSpaceBetweenThighs()
                .Concat(GetGenoTypeFromTendonsAndSinewsAndMuscles())
                .Concat(GetGenoTypeFromGonialAngle())
                .Concat(GetGenoTypeFromHeadShape())
                .Concat(GetGenoTypeFromRhesusFactor())
                .Concat(GetGenoTypeFromTasterStatus())
                .Concat(GetGenoTypeFromChemicalSensitivity())
                .Concat(GetGenoTypeFromSomatoType())
                .Concat(GetGenoTypesFromFamilyHistory())
                .Concat(GetGenoTypeFromHandedness())
                .Concat(GetGenoTypesFromIncisorShovelling())
                .Concat(GetGenoTypeFromFingerprints());

            return genoTypesStrengthTestResults.ToList();
        }

        #region Blood Analysis

        [UIHint("BloodGroup")]
        [QuestionCategory(Category = EQuestionCategory.BloodAnalysis)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BloodGroupQuestionLabel)]
        public EBloodGroup? BloodGroup { get; set; }

        [UIHint("BloodGroup")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BloodGroupLabel)]
        public EBloodGroup? BloodGroupDisplay => BloodGroup;

        [UIHint("RhesusFactor")]
        [QuestionCategory(Category = EQuestionCategory.BloodAnalysis)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RhesusFactorQuestionLabel)]
        public ERhesusFactor? RhesusFactor { get; set; }

        [UIHint("RhesusFactor")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RhesusFactorLabel)]
        public ERhesusFactor? RhesusFactorDisplay => RhesusFactor;

        public List<EGenoType> GetGenoTypeFromRhesusFactor()
        {
            var results = new List<EGenoType>();

            if (RhesusFactor == ERhesusFactor.Negative)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            return results;
        }

        public bool IsBloodAnalysisComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.BloodAnalysis);

        #endregion

        #region Acetylation

        [Score(IsAcetylation = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Acetylation)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MedicationSensitivityLabel)]
        public EYesNo? SensitivityToMedications { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Acetylation)]
        [Score(IsAcetylation = true)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CaffeineSensitivityLabel)]
        public EYesNo? SensitiveToCaffeine { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Acetylation)]
        [Score(IsAcetylation = true)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CaffeineAffectsSleepLabel)]
        public EYesNo? CaffeineAffectsSleep { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Acetylation)]
        [Score(IsAcetylation = true)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SensitiveToMoldLabel)]
        public EYesNo? SensitiveToMold { get; set; }

        [QuestionCategory(Category = EQuestionCategory.Acetylation)]
        [Score(IsAcetylation = true)]
        [UIHint("YesNo")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SensitiveToEnvironmentalChemicalsLabel)]
        public EYesNo? SensitiveToEnvironmentalChemicals { get; set; }

        public List<EGenoType> GetGenoTypeFromChemicalSensitivity()
        {
            var results = new List<EGenoType>();

            if (SensitivityToMedications == EYesNo.Yes
                || SensitiveToCaffeine == EYesNo.Yes
                || CaffeineAffectsSleep == EYesNo.Yes
                || SensitiveToMold == EYesNo.Yes
                || SensitiveToEnvironmentalChemicals == EYesNo.Yes)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            if (SensitiveToCaffeine == EYesNo.No)
            {
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            return results;
        }

        public bool IsAcetylationStatusComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.Acetylation);

        public int GetAcetulationScore()
        {
            return GetScore(e => e.IsAcetylation);
        }

        #endregion

        #region Biometrics

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StandingHeightLabel)]
        public double StandingHeight { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SittingHeightLabel)]
        public double SittingHeight { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ChairHeightLabel)]
        public double ChairHeight { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TorsoLengthLabel)]
        public double TorsoLength => SittingHeight - ChairHeight;

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TotalLegLengthLabel)]
        public double TotalLegLength => StandingHeight - TorsoLength;

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LowerLegLengthLabel)]
        public double LowerLegLength { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.UpperLegLengthLabel)]
        public double UpperLegLength { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TorsoLengthGreatherThanLegLengthLabel)]
        public bool TorsoLengthIsGreaterThanLegLength => IsTorsoLengthGreaterThanLegLength();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TorsoLengthGreatherThanLegLengthLabel)]
        public bool IsTorsoLengthGreaterThanLegLength() => TorsoLength >= TotalLegLength;

        [Display(ResourceType = typeof(Dictionary),
            Name = Strings.Labels.IsLowerLegLengthGreaterThanUpperLegLengthLabel)]
        public bool LowerLegLengthIsGreaterThanUpperLegLength => IsLowerLegLengthGreaterThanUpperLegLength();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IsLowerLegLengthGreaterThanUpperLegLengthLabel)]
        public bool IsLowerLegLengthGreaterThanUpperLegLength() => LowerLegLength >= UpperLegLength;

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftIndexFingerLengthQuestionLabel)]
        public double IndexFingerLengthLeft { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightIndexFingerLengthQuestionLabel)]
        public double IndexFingerLengthRight { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftRingFingerLengthQuestionLabel)]
        public double RingFingerLengthLeft { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightRingFingerLengthQuestionLabel)]
        public double RingFingerLengthRight { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IsLeftIndexFingerLongerThanLeftRingFingerLabel)]
        public bool LeftIndexFingerIsLongerThanRingFinger => IsIndexFingerLongerThanRingFingerLeft();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IsLeftIndexFingerLongerThanLeftRingFingerLabel)]
        public bool IsIndexFingerLongerThanRingFingerLeft() => IndexFingerLengthLeft >= RingFingerLengthLeft;

        [Display(ResourceType = typeof(Dictionary),
            Name = Strings.Labels.IsRightIndexFingerLongerThanRightRingFingerLabel)]
        public bool RightIndexFingerIsLongerThanRingFinger => IsIndexFingerLongerThanRingFingerRight();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IsRightIndexFingerLongerThanRightRingFingerLabel)]
        public bool IsIndexFingerLongerThanRingFingerRight() => IndexFingerLengthRight >= RingFingerLengthRight;

        public bool IndexFingersAreLongerThanRingFingersOnBothHands =>
            IsIndexFingerLongerThanRingFingerLeft() && IsIndexFingerLongerThanRingFingerRight();

        public bool RingFingersAreLongerThanIndexFingersOnBothHands =>
            !IsIndexFingerLongerThanRingFingerLeft() && !IsIndexFingerLongerThanRingFingerRight();

        [UIHint("GapSize")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SpaceBetweenThighsLabel)]
        public EGapSize? SpaceBetweenThighs { get; set; }

        public List<EGenoType> GetGenoTypeFromSpaceBetweenThighs()
        {
            var results = new List<EGenoType>();
            if (SpaceBetweenThighs == EGapSize.Touching)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
            }
            else if (SpaceBetweenThighs == EGapSize.Small)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);

                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Warrior);
            }
            else if (SpaceBetweenThighs == EGapSize.Large)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);

                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);

                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Nomad);
            }

            return results;
        }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WaistSizeLabel)]
        public double WaistSize { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HipSizeLabel)]
        public double HipSize { get; set; }

        public ERatio GetWaistToHipRatio()
        {
            var ratio = WaistSize / HipSize;

            if (Gender == EGender.Male)
            {
                if (YearsOld < 50)
                {
                    if (ratio > 0.96)
                    {
                        return ERatio.High;
                    }

                    if (ratio >= 0.91)
                    {
                        return ERatio.Average;
                    }

                    return ERatio.Ideal;
                }
                else
                {
                    if (ratio > 0.99)
                    {
                        return ERatio.High;
                    }

                    if (ratio >= 0.93)
                    {
                        return ERatio.Average;
                    }

                    return ERatio.Ideal;
                }
            }
            else if (Gender == EGender.Female || Gender == EGender.Other)
            {
                if (YearsOld < 50)
                {
                    if (ratio > 0.79)
                    {
                        return ERatio.High;
                    }

                    if (ratio >= 0.71)
                    {
                        return ERatio.Average;
                    }

                    return ERatio.Ideal;
                }
                else
                {
                    if (ratio > 0.84)
                    {
                        return ERatio.High;
                    }

                    if (ratio >= 0.75)
                    {
                        return ERatio.Average;
                    }

                    return ERatio.Ideal;
                }
            }

            return ERatio.Average;
        }

        [UIHint("GonialAngle")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.GonialAngleQuestionLabel)]
        public EGonialAngle? GonialAngle { get; set; }

        public List<EGenoType> GetGenoTypeFromGonialAngle()
        {
            var results = new List<EGenoType>();
            if (GonialAngle == EGonialAngle.WideAngle)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);

                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);

                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }

            if (GonialAngle == EGonialAngle.NarrowAngle)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);

                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);

                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            return results;
        }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HeadShapeLabel)]
        public EHeadShape? GetHeadShape()
        {
            var cephalicIndex = GetCephalicIndex();
            if (cephalicIndex < 75)
            {
                return EHeadShape.Dolichocephalic;
            }
            if (cephalicIndex <= 80)
            {
                return EHeadShape.Mesocephalic;
            }

            return EHeadShape.Brachycephalic;
        }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HeadWidthQuestionLabel)]
        public double HeadWidth { get; set; }

        [UIHint("Measurement")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, MustBeGreaterThanZero = true )]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HeadLengthQuestionLabel)]
        public double HeadLength { get; set; }

        public double GetCephalicIndex()
        {
            return (HeadWidth / HeadLength) * 100;
        }

        public List<EGenoType> GetGenoTypeFromHeadShape()
        {
            var results = new List<EGenoType>();
            if (GetHeadShape() == EHeadShape.Dolichocephalic)
            {
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            if (GetHeadShape() == EHeadShape.Brachycephalic)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);

                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }

            return results;
        }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TendonsAndSinewsVisibleLabel)]
        public EYesNo? TendonsAndSinewsVisible { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WristsAndAnklesPaddedLabel)]
        public EYesNo? WristsAndAnklesLookPadded { get; set; }

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.GainsMuscleEasilyLabel)]
        public EYesNo? GainsMuscleEasily { get; set; }

        public List<EGenoType> GetGenoTypeFromTendonsAndSinewsAndMuscles()
        {
            var results = new List<EGenoType>();
            if (TendonsAndSinewsVisible == EYesNo.Yes)
            {
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);

                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Explorer);
            }

            if (WristsAndAnklesLookPadded == EYesNo.Yes)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);

                results.Add(EGenoType.Warrior);
            }

            if (GainsMuscleEasily == EYesNo.Yes)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            return results;
        }

        [UIHint("SomatoType")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics, AllowNull = true)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SomatoTypeQuestionLabel)]
        public ESomatoType? SomatoType { get; set; }

        [UIHint("WristCircumference")]
        [QuestionCategory(Category = EQuestionCategory.Biometrics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WristCircumferenceQuestionLabel)]
        public EWristCircumference? WristCircumference { get; set; }

        public List<EGenoType> GetGenoTypeFromSomatoType()
        {
            var results = new List<EGenoType>();

            var somatoType = SomatoType;

            if (SomatoType == ESomatoType.NotSure)
            {
                if (WristCircumference == EWristCircumference.DoNotTouch)
                {
                    somatoType = ESomatoType.Endomorph;
                }
                else if (WristCircumference == EWristCircumference.JustTouch)
                {
                    somatoType = ESomatoType.Mesomorph;
                }
                else if (WristCircumference == EWristCircumference.Overlap)
                {
                    somatoType = ESomatoType.Ectomorph;
                }
            }

            if (somatoType == ESomatoType.Ectomorph)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
            }

            if (somatoType == ESomatoType.Ectomesomorph || GetWaistToHipRatio() == ERatio.Ideal)
            {
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
            }

            if (somatoType == ESomatoType.Endomorph || GetWaistToHipRatio() == ERatio.High)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
            }

            if (somatoType == ESomatoType.Mesomorph || GetWaistToHipRatio() == ERatio.Ideal)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            if (SomatoType == ESomatoType.MesoEndomorph || GetWaistToHipRatio() == ERatio.High)
            {
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            return results;
        }

        public bool IsBiometricsComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.Biometrics);

        #endregion

        #region Dermatoglyphics

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NumberOfMatchingFingerprintsLabel)]
        public int NumberOfMatchingFingerprints => GetNumberOfMatchingFingerprints();

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NumberOfMatchingFingerprintsLabel)]
        public int GetNumberOfMatchingFingerprints()
        {
            var total = 0;

            if (LeftThumprintType == RightThumprintType && LeftThumprintType != EFingerprintType.NotSure)
            {
                total++;
            }
            if (LeftIndexFingerprintType == RightIndexFingerprintType && LeftIndexFingerprintType != EFingerprintType.NotSure)
            {
                total++;
            }
            if (LeftMiddleFingerprintType == RightMiddleFingerprintType && LeftMiddleFingerprintType != EFingerprintType.NotSure)
            {
                total++;
            }
            if (LeftRingFingerprintType == RightRingFingerprintType && LeftRingFingerprintType != EFingerprintType.NotSure)
            {
                total++;
            }
            if (LeftLittleFingerprintType == RightLittleFingerprintType && LeftLittleFingerprintType != EFingerprintType.NotSure)
            {
                total++;
            }

            return total;
        }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IndexFingersMatchLabel)]
        public bool IndexFingerprintsMatch => LeftIndexFingerprintType == RightIndexFingerprintType && LeftIndexFingerprintType != EFingerprintType.NotSure;

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftThumprintLabel)]
        public EFingerprintType? LeftThumprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftIndexFingerprintLabel)]
        public EFingerprintType? LeftIndexFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftMiddleFingerprintLabel)]
        public EFingerprintType? LeftMiddleFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftRingFingerprintLabel)]
        public EFingerprintType? LeftRingFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.LeftLittleFingerprintLabel)]
        public EFingerprintType? LeftLittleFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightThumprintLabel)]
        public EFingerprintType? RightThumprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightIndexFingerprintLabel)]
        public EFingerprintType? RightIndexFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightMiddleFingerprintLabel)]
        public EFingerprintType? RightMiddleFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightRingFingerprintLabel)]
        public EFingerprintType? RightRingFingerprintType { get; set; }

        [UIHint("FingerPrint")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.RightLittleFingerprintLabel)]
        public EFingerprintType? RightLittleFingerprintType { get; set; }

        [UIHint("Handedness")]
        [QuestionCategory(Category = EQuestionCategory.Dermatoglyphics)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HandednessLabel)]
        public EHandedness? Handedness { get; set; }

        public List<EGenoType> GetGenoTypeFromHandedness()
        {
            var results = new List<EGenoType>();

            if (Handedness == EHandedness.Left || Handedness == EHandedness.Ambidextrous)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            return results;
        }

        public List<EGenoType> GetGenoTypeFromFingerprints()
        {
            var results = new List<EGenoType>();

            if (GetNumberOfMatchingFingerprints() <= 2)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
            }
            else if (GetNumberOfMatchingFingerprints() >= 4)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);

                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }
            else
            {
                results.Add(EGenoType.Teacher);
            }

            if (IndexFingerprintsMatch)
            {
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            if (CountFingerprintsByType(EFingerprintType.Whorl) >= 5)
            {
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
            }

            if (CountFingerprintsByType(EFingerprintType.Arch) >= 2)
            {
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            if (CountFingerprintsByType(EFingerprintType.Loop) >= 8)
            {
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }

            return results;
        }

        private int CountFingerprintsByType(EFingerprintType type)
        {
            var fingerPrints = new List<EFingerprintType?>
            {
                LeftThumprintType,
                LeftIndexFingerprintType,
                LeftMiddleFingerprintType,
                LeftRingFingerprintType,
                LeftLittleFingerprintType,
                RightThumprintType,
                RightIndexFingerprintType,
                RightMiddleFingerprintType,
                RightRingFingerprintType,
                RightLittleFingerprintType
            };

            return fingerPrints.Count(e => e == type);
        }

        public bool IsDermatoglyphicsComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.Dermatoglyphics);

        #endregion

        #region Dentition

        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.Dentition)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IncisorShovellingLabel)]
        public EYesNo? IncisorShovelling { get; set; }

        public List<EGenoType> GetGenoTypesFromIncisorShovelling()
        {
            var results = new List<EGenoType>();

            if (IncisorShovelling == EYesNo.Yes)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);

                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }

            return results;
        }

        public bool IsDentitionComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.Dentition);

        #endregion

        #region Taster Status

        [UIHint("PropTaste")]
        [QuestionCategory(Category = EQuestionCategory.TasterStatus)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CruciferousVegetablesVeryBitterLabel)]
        public EPropTaste? CruciferousVegetablesTasteVeryBitter { get; set; }

        public List<EGenoType> GetGenoTypeFromTasterStatus()
        {
            var results = new List<EGenoType>();
            if (CruciferousVegetablesTasteVeryBitter == EPropTaste.NoticeablyBitter)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);

                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
                results.Add(EGenoType.Explorer);
            }

            else if (CruciferousVegetablesTasteVeryBitter == EPropTaste.NotBitter)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);

                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            else if (CruciferousVegetablesTasteVeryBitter == EPropTaste.SlightlyBitter)
            {
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);

                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }

            return results;
        }

        public bool IsTasterStatusComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.TasterStatus);

        #endregion

        #region Family History

        [Score(NeurologicalHealth = true, Cognition = true, IsFamilyHistory = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.FamilyHistory)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FamilyHistoryOfNeurologicalDiseaseLabel)]
        public EYesNo? FamilyHistoryOfNeurologicalDisease { get; set; }

        [Score(CardioVascularHealth = true, IsFamilyHistory = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.FamilyHistory)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FamilyHistoryOfHeartDiseaseStrokeOrDiabetesLabel)]
        public EYesNo? FamilyHistoryOfHeartDiseaseStrokeOrDiabetes { get; set; }

        [Score(DNAIntegrity = true, IsFamilyHistory = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.FamilyHistory)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FamilyHistoryOfCancerLabel)]
        public EYesNo? FamilyHistoryOfCancer { get; set; }

        [Score(AntiInflammatory = true, IsFamilyHistory = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.FamilyHistory)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FamilyHistoryOfAutoimmuneDiseaseLabel)]
        public EYesNo? FamilyHistoryOfAutoimmuneDisease { get; set; }

        [Score(IsFamilyHistory = true)]
        [UIHint("YesNo")]
        [QuestionCategory(Category = EQuestionCategory.FamilyHistory)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.FamilyHistoryOfSubstanceDependencyLabel)]
        public EYesNo? FamilyHistoryOfSubstanceDependency { get; set; }

        public List<EGenoType> GetGenoTypesFromFamilyHistory()
        {
            var results = new List<EGenoType>();
            if (FamilyHistoryOfAutoimmuneDisease == EYesNo.Yes)
            {
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
                results.Add(EGenoType.Hunter);
            }

            if (FamilyHistoryOfHeartDiseaseStrokeOrDiabetes == EYesNo.Yes)
            {
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);
                results.Add(EGenoType.Gatherer);

                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
                results.Add(EGenoType.Warrior);
            }

            if (FamilyHistoryOfCancer == EYesNo.Yes)
            {
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
                results.Add(EGenoType.Teacher);
            }

            if (FamilyHistoryOfNeurologicalDisease == EYesNo.Yes)
            {
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
                results.Add(EGenoType.Nomad);
            }

            return results;
        }

        public bool IsFamilyHistoryComplete() => IsCategoryComplete(e => e.Category == EQuestionCategory.FamilyHistory);

        public int GetFamilyHistoryScore()
        {
            return GetScore(e => e.IsFamilyHistory);
        }

        public List<PropertyInfo> FamilyHistoryProperties() => this
            .GetProperties().Where(e => e.GetAttribute<QuestionCategoryAttribute>()?.Category == EQuestionCategory.FamilyHistory).ToList();

        #endregion

        public List<EGenoType> CalculateGenoTypes()
        {
            var genotypes = new List<EGenoType>();

            if (IsTorsoLengthGreaterThanLegLength())
            {
                if (IsLowerLegLengthGreaterThanUpperLegLength())
                {
                    if (IndexFingersAreLongerThanRingFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Gatherer,
                                EGenoType.Explorer,
                                EGenoType.Warrior,
                                EGenoType.Nomad
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (Gender == EGender.Male)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Nomad
                                    });
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Gatherer,
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Gatherer
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (RingFingersAreLongerThanIndexFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Hunter,
                                EGenoType.Teacher,
                                EGenoType.Explorer,
                                EGenoType.Nomad
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Teacher,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Teacher
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Teacher,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Nomad,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Hunter,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Hunter,
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    // Finger length assymetrical
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Gatherer,
                                EGenoType.Teacher,
                                EGenoType.Explorer,
                                EGenoType.Nomad
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Teacher
                                });
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Teacher,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Nomad,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                else
                // Upper leg is longer
                {
                    if (IndexFingersAreLongerThanRingFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Gatherer,
                                EGenoType.Teacher,
                                EGenoType.Explorer,
                                EGenoType.Nomad
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Teacher,
                                        EGenoType.Explorer
                                    });
                                }
                                else if (RhesusFactor == ERhesusFactor.Positive)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher
                                        });
                                    }
                                }
                                else if (RhesusFactor == ERhesusFactor.Negative)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer,
                                                EGenoType.Nomad
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Female)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                }
                                else
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else if (RingFingersAreLongerThanIndexFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {

                                EGenoType.Hunter,
                                EGenoType.Teacher,
                                EGenoType.Explorer,
                                EGenoType.Nomad
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Teacher,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Teacher
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Teacher,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Nomad
                                        });
                                    }
                                    else
                                    {
                                        if (RhesusFactor == ERhesusFactor.Negative)
                                        {
                                            if (Gender == EGender.Male)
                                            {
                                                genotypes.AddRange(new[]
                                                {
                                                    EGenoType.Nomad,
                                                    EGenoType.Explorer
                                                });
                                            }
                                            else
                                            {
                                                genotypes.AddRange(new[]
                                                {
                                                    EGenoType.Explorer
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer,
                                                EGenoType.Nomad
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male && IsIsLGBTQPlus != EYesNo.Yes)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male && IsIsLGBTQPlus != EYesNo.Yes)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Hunter
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Hunter,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male && IsIsLGBTQPlus != EYesNo.Yes)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Hunter
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Hunter,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    // Finger length assymetrical
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Gatherer,
                                EGenoType.Teacher,
                                EGenoType.Explorer,
                                EGenoType.Warrior
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Teacher
                                });
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            // Legs are longer than torso
            {
                if (IsLowerLegLengthGreaterThanUpperLegLength())
                {
                    if (IndexFingersAreLongerThanRingFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Gatherer,
                                EGenoType.Explorer,
                                EGenoType.Nomad,
                                EGenoType.Warrior
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer,
                                                EGenoType.Warrior
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Nomad
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Gatherer,
                                                EGenoType.Nomad
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Explorer
                                    });
                                }
                                else if (RhesusFactor == ERhesusFactor.Positive)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                }
                                else if (RhesusFactor == ERhesusFactor.Negative)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else if (RingFingersAreLongerThanIndexFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            if (Gender == EGender.Male)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Hunter,
                                    EGenoType.Teacher,
                                    EGenoType.Warrior,
                                    EGenoType.Nomad
                                });
                            }
                            else
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Warrior,
                                    EGenoType.Nomad,
                                    EGenoType.Explorer,
                                    EGenoType.Hunter
                                });
                            }
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Teacher
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Teacher
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                }
                                else if (RhesusFactor == ERhesusFactor.Positive)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Nomad
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                }
                                else if (RhesusFactor == ERhesusFactor.Negative)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Warrior
                                    });
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Nomad,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad,
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Hunter
                                });
                            }
                        }
                    }
                    else
                    // Finger lengths are assymetrical
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Hunter,
                                EGenoType.Teacher,
                                EGenoType.Warrior,
                                EGenoType.Nomad,
                                EGenoType.Explorer
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Teacher,
                                    EGenoType.Warrior
                                });
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (Gender == EGender.Male)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.NotSure)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else
                                    {
                                        if (RhesusFactor == ERhesusFactor.Positive)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                        else if (RhesusFactor == ERhesusFactor.Negative)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Nomad,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad,
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Hunter,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                else
                // Upper leg is longer
                {
                    if (IndexFingersAreLongerThanRingFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            if (Gender == EGender.Male)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Warrior,
                                    EGenoType.Explorer,
                                    EGenoType.Gatherer
                                });
                            }
                            else
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Warrior,
                                    EGenoType.Explorer,
                                    EGenoType.Gatherer,
                                    EGenoType.Nomad
                                });
                            }
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (Gender == EGender.Male)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.NotSure)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else
                                    {
                                        if (RhesusFactor == ERhesusFactor.Positive)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad
                                            });
                                        }
                                        else if (RhesusFactor == ERhesusFactor.Negative)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior
                                            });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (Gender == EGender.Male)
                                {
                                    if (RhesusFactor != ERhesusFactor.NotSure)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.NotSure)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Nomad,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Gatherer,
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Gatherer
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                            EGenoType.Gatherer,
                                            EGenoType.Explorer
                                        });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Gatherer
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (RingFingersAreLongerThanIndexFingersOnBothHands)
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Hunter,
                                EGenoType.Explorer,
                                EGenoType.Warrior,
                                EGenoType.Nomad
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer,
                                                EGenoType.Warrior
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Warrior
                                            });
                                        }
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer,
                                            EGenoType.Warrior
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Warrior
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Warrior,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Explorer,
                                        EGenoType.Nomad
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Nomad
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Nomad,
                                                EGenoType.Explorer
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                            EGenoType.Explorer
                                        });
                                        }
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    if (Gender == EGender.Male && IsIsLGBTQPlus != EYesNo.Yes)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter,
                                        });
                                    }
                                    else
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Hunter
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        if (Gender == EGender.Male)
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Hunter
                                            });
                                        }
                                        else
                                        {
                                            genotypes.AddRange(new[]
                                            {
                                                EGenoType.Explorer
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    // Finger lengths are assymetrical
                    {
                        if (BloodGroup == EBloodGroup.NotSure)
                        {
                            genotypes.AddRange(new[]
                            {
                                EGenoType.Hunter,
                                EGenoType.Gatherer,
                                EGenoType.Teacher,
                                EGenoType.Explorer
                            });
                        }
                        else
                        {
                            if (BloodGroup == EBloodGroup.A)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Teacher,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Teacher,
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.AB)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Teacher,
                                    EGenoType.Explorer
                                });
                            }
                            else if (BloodGroup == EBloodGroup.B)
                            {
                                if (RhesusFactor == ERhesusFactor.NotSure)
                                {
                                    genotypes.AddRange(new[]
                                    {
                                        EGenoType.Gatherer,
                                        EGenoType.Explorer
                                    });
                                }
                                else
                                {
                                    if (RhesusFactor == ERhesusFactor.Positive)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Gatherer
                                        });
                                    }
                                    else if (RhesusFactor == ERhesusFactor.Negative)
                                    {
                                        genotypes.AddRange(new[]
                                        {
                                            EGenoType.Explorer
                                        });
                                    }
                                }
                            }
                            else if (BloodGroup == EBloodGroup.O)
                            {
                                genotypes.AddRange(new[]
                                {
                                    EGenoType.Hunter,
                                    EGenoType.Gatherer
                                });
                            }
                        }
                    }
                }
            }


            return genotypes;
        }
    }
}