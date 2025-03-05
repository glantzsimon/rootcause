using K9.Base.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.WebApplication.Enums;
using K9.WebApplication.Extensions;
using K9.WebApplication.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K9.WebApplication.Models
{

    public class NineStarKiModel : CachableBase
    {
        private const bool invertCycleYinEnergies = true;

        public NineStarKiModel()
        {
            var dateOfBirth = new DateTime(DateTime.Now.Year - (27), DateTime.Now.Month, DateTime.Now.Day);
            var personModel = new PersonModel
            {
                DateOfBirth = dateOfBirth,
                Gender = Methods.GetRandomGender()
            };

            PersonModel = personModel;
        }

        public NineStarKiModel(PersonModel personModel, DateTime? selectedDate = null)
        {
            PersonModel = personModel;

            MainEnergy = GetOrAddToCache($"MainEnergy_{PersonModel.DateOfBirth}_{PersonModel.Gender}",
                () => GetMainEnergy(PersonModel.DateOfBirth, PersonModel.Gender), TimeSpan.FromDays(30));

            CharacterEnergy = GetOrAddToCache($"CharacterEnergy_{PersonModel.DateOfBirth}_{PersonModel.Gender}",
                () => GetCharacterEnergy(PersonModel.DateOfBirth, PersonModel.Gender), TimeSpan.FromDays(30));

            SurfaceEnergy = GetOrAddToCache($"SurfaceEnergy_{MainEnergy.EnergyNumber}_{CharacterEnergy.EnergyNumber}",
                GetSurfaceEnergy, TimeSpan.FromDays(30));

            YearlyCycleEnergy = GetOrAddToCache($"YearlyCycleEnergy_{PersonModel.DateOfBirth}_{PersonModel.Gender}_{SelectedDate.Value.ToString()}",
                GetYearlyCycleEnergy, TimeSpan.FromDays(30));

            MonthlyCycleEnergy = GetOrAddToCache($"MonthlyCycleEnergy_{PersonModel.DateOfBirth}_{PersonModel.Gender}_{SelectedDate.Value.ToString()}",
                GetMonthlyCycleEnergy, TimeSpan.FromDays(30));

            MainEnergy.RelatedEnergy = CharacterEnergy.Energy;
            CharacterEnergy.RelatedEnergy = MainEnergy.Energy;
            SurfaceEnergy.RelatedEnergy = MainEnergy.Energy;
        }

        public PersonModel PersonModel { get; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.MainEnergyLabel)]
        public NineStarKiEnergy MainEnergy { get; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CharacterEnergyLabel)]
        public NineStarKiEnergy CharacterEnergy { get; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SurfaceEnergyLabel)]
        public NineStarKiEnergy SurfaceEnergy { get; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.HealthLabel)]
        public string Health { get; set; }

        /// <summary>
        /// Determines the 9 Star Ki energy of the current year
        /// </summary>
        public NineStarKiEnergy YearlyCycleEnergy { get; }

        /// <summary>
        /// Determines the 9 Star Ki energy of the current month
        /// </summary>
        public NineStarKiEnergy MonthlyCycleEnergy { get; }

        [UIHint("Organ")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StrongYinOrgans)]
        public EOrgan? StrongYinOrgans => MainEnergy != null ? MainEnergy.MetaData?.StrongYinOrgans : null;

        [UIHint("Organ")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.StrongYangOrgans)]
        public EOrgan? StrongYangOrgans => MainEnergy != null ? MainEnergy.MetaData?.StrongYangOrgans : null;

        [UIHint("Organs")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WeakYinOrgans)]
        public EOrgan[] WeakYinOrgans => MainEnergy != null ? MainEnergy.MetaData?.WeakYinOrgans : null;

        [UIHint("Organs")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.WeakYangOrgans)]
        public EOrgan[] WeakYangOrgans => MainEnergy != null ? MainEnergy.MetaData?.WeakYangOrgans : null;

        public List<Tuple<int, NineStarKiEnergy>> GetYearlyPlanner()
        {
            return GetOrAddToCache($"YearlyPlanner_{DateTime.Today.Year}", () =>
            {
                var cycles = new List<Tuple<int, NineStarKiEnergy>>();
                var today = new DateTime(DateTime.Today.Year, 2, 15);

                for (int i = -20; i <= 20; i++)
                {
                    today = today.AddYears(i);
                    cycles.Add(new Tuple<int, NineStarKiEnergy>(today.Year, GetYearlyCycleEnergy(today)));
                }

                return cycles;
            }, TimeSpan.FromDays(30));
        }

        public List<Tuple<int, int, string, NineStarKiEnergy>> GetMonthlyPlanner()
        {
            return GetOrAddToCache($"MonthlyPlanner_{DateTime.Today.Year}", () =>
            {
                var cycles = new List<Tuple<int, int, string, NineStarKiEnergy>>();
                var today = new DateTime(DateTime.Today.Year, 2, 15);

                for (int i = -1; i <= 10; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        var year = today.AddYears(i).Year;
                        var date = new DateTime(year, j + 1, 15);
                        cycles.Add(new Tuple<int, int, string, NineStarKiEnergy>(date.Year, date.Month, date.ToString("MMMM"), GetMonthlyCycleEnergy(date)));
                    }
                }
                
                return cycles;

            }, TimeSpan.FromDays(30));
        }

        private NineStarKiEnergy GetMainEnergy(DateTime date, EGender gender)
        {
            var month = date.Month;
            var day = date.Day;
            var year = date.Year;

            year = (month == 2 && day <= 3) || month == 1 ? year - 1 : year;
            var energyNumber = 3 - ((year - 1979) % 9);

            var GetToTheRootKiEnergy = ProcessEnergy(energyNumber, gender);
            GetToTheRootKiEnergy.Gender = gender;

            return GetToTheRootKiEnergy;
        }

        private int GetMonth(DateTime date)
        {
            var month = date.Month;
            var day = date.Day;

            switch (month)
            {
                case 2:
                    month = day >= 4 ? month : month - 1;
                    break;

                case 3:
                case 6:
                    month = day >= 6 ? month : month - 1;
                    break;

                case 4:
                case 5:
                    month = day >= 5 ? month : month - 1;
                    break;

                case 1:
                    month = day >= 5 ? month : 12;
                    break;

                case 7:
                case 8:
                case 11:
                case 12:
                    month = day >= 7 ? month : month - 1;
                    break;

                case 9:
                case 10:
                    month = day >= 8 ? month : month - 1;
                    break;
            }

            return month;
        }

        private int GetMonthStartDay(int month)
        {
            switch (month)
            {
                case 2:
                    return 4;

                case 3:
                case 6:
                    return 6;

                case 4:
                case 5:
                case 1:
                    return 5;

                case 7:
                case 8:
                case 11:
                case 12:
                    return 7;

                case 9:
                case 10:
                    return 8;
            }

            throw new ArgumentOutOfRangeException();
        }

        public NineStarKiDates GetMonthlyPeriod(DateTime date, EGender gender)
        {
            var month = GetMonth(date);
            var yearlyEnergy = GetMainEnergy(date, gender);
            var energyNumber = GetEnergyNumberFromYearlyEnergy(yearlyEnergy.Energy, month);
            var monthlyEnergy = ProcessEnergy(energyNumber, gender, EGetToTheRootKiEnergyType.CharacterEnergy);

            var startDate = new DateTime(date.Year, month, GetMonthStartDay(month));
            var nextMonthDate = startDate.AddMonths(1);
            nextMonthDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 15);

            var nextMonth = GetMonth(nextMonthDate);
            var nextMonthStartDay = GetMonthStartDay(nextMonth);
            var endDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, nextMonthStartDay - 1);

            return new NineStarKiDates
            {
                YearlyEnergy = yearlyEnergy.Energy,
                MonthlyEnergy = monthlyEnergy.Energy,
                MonthlyPeriodStartsOn = startDate,
                MonthlyPeriodEndsOn = endDate
            };
        }

        private int GetEnergyNumberFromYearlyEnergy(EGetToTheRootKiEnergy yearlyEnergy, int month)
        {
            var energyNumber = 0;

            switch (yearlyEnergy)
            {
                case EGetToTheRootKiEnergy.Thunder:
                case EGetToTheRootKiEnergy.Heaven:
                case EGetToTheRootKiEnergy.Fire:
                    energyNumber = 5;
                    break;

                case EGetToTheRootKiEnergy.Water:
                case EGetToTheRootKiEnergy.Wind:
                case EGetToTheRootKiEnergy.Lake:
                    energyNumber = 8;
                    break;

                case EGetToTheRootKiEnergy.Soil:
                case EGetToTheRootKiEnergy.CoreEarth:
                case EGetToTheRootKiEnergy.Mountain:
                    energyNumber = 2;
                    break;
            }

            switch (month)
            {
                case 2:
                case 11:
                    // no need to change
                    break;

                case 3:
                case 12:
                    energyNumber -= 1;
                    break;

                case 4:
                case 1:
                    energyNumber -= 2;
                    break;

                case 5:
                    energyNumber -= 3;
                    break;

                case 6:
                    energyNumber -= 4;
                    break;

                case 7:
                    energyNumber -= 5;
                    break;

                case 8:
                    energyNumber -= 6;
                    break;

                case 9:
                    energyNumber -= 7;
                    break;

                case 10:
                    energyNumber -= 8;
                    break;
            }

            return energyNumber;
        }

        private NineStarKiEnergy GetCharacterEnergy(DateTime date, EGender gender = EGender.Male)
        {
            var month = GetMonth(date);
            var yearlyEnergy = GetMainEnergy(date, gender).Energy;
            var energyNumber = GetEnergyNumberFromYearlyEnergy(yearlyEnergy, month);

            return ProcessEnergy(energyNumber, gender, EGetToTheRootKiEnergyType.CharacterEnergy);
        }

        private NineStarKiEnergy GetSurfaceEnergy()
        {
            return ProcessEnergy(5 - (CharacterEnergy.EnergyNumber - MainEnergy.EnergyNumber), EGender.Male, EGetToTheRootKiEnergyType.SurfaceEnergy);
        }

        private NineStarKiEnergy GetYearlyCycleEnergy(DateTime? date)
        {
            var todayYearEnergy = (int)GetMainEnergy(date ?? DateTime.Today, EGender.Male).Energy;
            var personalYearEnergy = PersonModel.Gender.IsYin() ? InvertEnergy(MainEnergy.EnergyNumber) : MainEnergy.EnergyNumber;
            var offset = todayYearEnergy - personalYearEnergy;
            var lifeCycleYearEnergy = LoopEnergyNumber(5 - offset);

            var energy = (EGetToTheRootKiEnergy)(PersonModel.Gender.IsYin() && invertCycleYinEnergies ? InvertEnergy(lifeCycleYearEnergy) : lifeCycleYearEnergy);

            return new NineStarKiEnergy(energy, EGetToTheRootKiEnergyType.MainEnergy, PersonModel.IsAdult(), EGetToTheRootKiEnergyCycleType.YearlyCycleEnergy);
        }

        private NineStarKiEnergy GetMonthlyCycleEnergy(DateTime? date)
        {
            var month = GetMonth(date ?? DateTime.Today);
            var yearlyCycleEnergy = GetYearlyCycleEnergy(date).Energy;
            var energyNumber = GetEnergyNumberFromYearlyEnergy(yearlyCycleEnergy, month);
            var monthlyEnergy = ProcessEnergy(energyNumber, PersonModel.Gender, EGetToTheRootKiEnergyType.CharacterEnergy);
            monthlyEnergy.EnergyCycleType = EGetToTheRootKiEnergyCycleType.MonthlyCycleEnergy;
            return monthlyEnergy;
        }

        private NineStarKiEnergy ProcessEnergy(int energyNumber, EGender gender, EGetToTheRootKiEnergyType type = EGetToTheRootKiEnergyType.MainEnergy)
        {
            energyNumber = LoopEnergyNumber(energyNumber);
            if (gender.IsYin())
            {
                energyNumber = InvertEnergy(energyNumber);
            }

            return new NineStarKiEnergy((EGetToTheRootKiEnergy)energyNumber, type, PersonModel.IsAdult());
        }

        private static readonly Dictionary<int, int> _invertedEnergies = new Dictionary<int, int>
        {
            { 1, 5 }, { 2, 4 }, { 4, 2 }, { 5, 1 },
            { 6, 9 }, { 7, 8 }, { 8, 7 }, { 9, 6 }
        };

        private static int LoopEnergyNumber(int energyNumber) => (energyNumber + 8) % 9 + 1;

        private int InvertEnergy(int energyNumber) =>
            _invertedEnergies.TryGetValue(energyNumber, out var inverted) ? inverted : energyNumber;

        private static readonly Dictionary<bool, Dictionary<(EGetToTheRootKiYinYang, EGetToTheRootKiYinYang), ESexualityRelationType>> _sexualityRelations
            = new Dictionary<bool, Dictionary<(EGetToTheRootKiYinYang, EGetToTheRootKiYinYang), ESexualityRelationType>>
            {
                // If PersonModel.Gender.IsYin() == true
                [true] = new Dictionary<(EGetToTheRootKiYinYang, EGetToTheRootKiYinYang), ESexualityRelationType>
                {
                    { (EGetToTheRootKiYinYang.Yin, EGetToTheRootKiYinYang.Yin), ESexualityRelationType.MatchMatch },
                    { (EGetToTheRootKiYinYang.Yang, EGetToTheRootKiYinYang.Yang), ESexualityRelationType.OppositeOpposite },
                    { (EGetToTheRootKiYinYang.Yin, EGetToTheRootKiYinYang.Yang), ESexualityRelationType.MatchOpposite },
                    { (EGetToTheRootKiYinYang.Yang, EGetToTheRootKiYinYang.Yin), ESexualityRelationType.OppositeMatch }
                },

                // If PersonModel.Gender.IsYin() == false
                [false] = new Dictionary<(EGetToTheRootKiYinYang, EGetToTheRootKiYinYang), ESexualityRelationType>
                {
                    { (EGetToTheRootKiYinYang.Yin, EGetToTheRootKiYinYang.Yin), ESexualityRelationType.OppositeOpposite },
                    { (EGetToTheRootKiYinYang.Yang, EGetToTheRootKiYinYang.Yang), ESexualityRelationType.MatchMatch },
                    { (EGetToTheRootKiYinYang.Yin, EGetToTheRootKiYinYang.Yang), ESexualityRelationType.OppositeMatch },
                    { (EGetToTheRootKiYinYang.Yang, EGetToTheRootKiYinYang.Yin), ESexualityRelationType.MatchOpposite }
                }
            };

        private ESexualityRelationType GetSexualityRelationType()
        {
            if (PersonModel == null || MainEnergy == null || CharacterEnergy == null)
            {
                return ESexualityRelationType.Unspecified;
            }

            bool isPersonYin = PersonModel.Gender.IsYin();

            return _sexualityRelations[isPersonYin].TryGetValue((MainEnergy.YinYang, CharacterEnergy.YinYang), out var relation)
                ? relation
                : ESexualityRelationType.Unspecified;
        }
    }
}