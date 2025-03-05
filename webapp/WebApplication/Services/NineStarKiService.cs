using K9.Base.DataAccessLayer.Enums;
using K9.Globalisation;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.ViewModels;
using System;
using System.Collections.Generic;

namespace K9.WebApplication.Services
{
    public class NineStarKiService : BaseService, INineStarKiService
    {
        public NineStarKiService(IServiceBasePackage my) : base(my)
        {
        }

        public NineStarKiModel CalculateNineStarKiKiProfile(DateTime dateOfBirth, EGender gender = EGender.Male)
        {
            var cacheKey = $"CalculateGetToTheRootKiProfile_{dateOfBirth.ToString()}_{gender}";
            return GetOrAddToCache(cacheKey, () =>
            {
                return CalculateNineStarKiKiProfile(new PersonModel
                {
                    DateOfBirth = dateOfBirth,
                    Gender = gender
                });

            }, TimeSpan.FromDays(30));
        }

        public NineStarKiModel CalculateNineStarKiKiProfile(PersonModel personModel, bool isCompatibility = false,
            bool isMyProfile = false, DateTime? today = null)
        {
            var cacheKey = $"CalculateGetToTheRootKiProfileFromModel_{personModel.DateOfBirth.ToString()}_{personModel.Name}_{personModel.Gender}_{isCompatibility}_{isMyProfile}_{today.ToString()}";
            return GetOrAddToCache(cacheKey, () =>
            {
                var model = new NineStarKiModel(personModel, today);

                model.MainEnergy.EnergyDescription = GetMainEnergyDescription(model.MainEnergy.Energy);
                model.CharacterEnergy.EnergyDescription = GetCharacterEnergyDescription(model.CharacterEnergy.Energy);
                model.SurfaceEnergy.EnergyDescription = GetSurfaceEnergyDescription(model.SurfaceEnergy.Energy);
                model.Health = GetHealth(model.MainEnergy.Energy);
                model.Occupations = GetOccupations(model.MainEnergy.Energy);
                model.PersonalDevelopemnt = GetPersonalDevelopemnt(model.MainEnergy.Energy);
                model.Summary = GetSummary(model);
                model.Overview = GetOverview(model.MainEnergy.Energy);

                if (isCompatibility)
                {
                    model.IsShowSummary = false;
                }

                model.IsMyProfile = isMyProfile;
                model.IsProcessed = true;
                model.IsCompatibility = isCompatibility;

                return model;

            }, TimeSpan.FromDays(30));
        }

        public CompatibilityModel CalculateCompatibility(DateTime dateOfBirth1, EGender gender1, DateTime dateOfBirth2,
            EGender gender2)
        {
            var cacheKey = $"CalculateCompatibility_{dateOfBirth1.ToString()}_{gender1}_{dateOfBirth2.ToString()}_{gender2}";
            return GetOrAddToCache(cacheKey, () =>
            {
                return CalculateCompatibility(
                    new PersonModel
                    {
                        DateOfBirth = dateOfBirth1,
                        Gender = gender1
                    }, new PersonModel
                    {
                        DateOfBirth = dateOfBirth2,
                        Gender = gender2
                    }, false);

            }, TimeSpan.FromDays(30));
        }

        public CompatibilityModel CalculateCompatibility(PersonModel personModel1, PersonModel personModel2,
            bool isHideSexuality)
        {
            var cacheKey = $"CalculateCompatibilityFromModel_{personModel1.DateOfBirth.ToString()}_{personModel1.Name}_{personModel1.Gender}_{personModel2.DateOfBirth.ToString()}_{personModel2.Name}_{personModel2.Gender}";
            return GetOrAddToCache(cacheKey, () =>
            {
                var GetToTheRootKiModel1 = CalculateNineStarKiKiProfile(personModel1, true);
                var GetToTheRootKiModel2 = CalculateNineStarKiKiProfile(personModel2, true);

                var model = new CompatibilityModel(GetToTheRootKiModel1, GetToTheRootKiModel2)
                {
                    IsProcessed = true,
                    IsHideSexualChemistry = isHideSexuality
                };

                if (string.IsNullOrEmpty(GetToTheRootKiModel1.PersonModel.Name))
                {
                    GetToTheRootKiModel1.PersonModel.Name = Dictionary.FirstPerson;
                }

                if (string.IsNullOrEmpty(GetToTheRootKiModel2.PersonModel.Name))
                {
                    GetToTheRootKiModel2.PersonModel.Name = Dictionary.SecondPerson;
                }

                return model;
            }, TimeSpan.FromDays(30));
        }

        public GetToTheRootKiSummaryViewModel GetGetToTheRootKiSummaryViewModel()
        {
            return GetOrAddToCache("GetGetToTheRootKiSummaryViewModel", () =>
            {

                var mainEnergies = new List<NineStarKiModel>
            {
                CalculateNineStarKiKiProfile(new DateTime(1981, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1979, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1978, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1977, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1976, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1984, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1974, 3, 3)),
                CalculateNineStarKiKiProfile(new DateTime(1973, 3, 3)),
            };
                var characterEnergies = new List<NineStarKiModel>
            {
                CalculateNineStarKiKiProfile(new DateTime(1980, 2, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 3, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 4, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 5, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 6, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 7, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 8, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 9, 10)),
                CalculateNineStarKiKiProfile(new DateTime(1980, 10, 10)),
            };
                var dynamicEnergies = new List<NineStarKiEnergy>
            {
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergyType.MainEnergy),
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergyType.MainEnergy),
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergyType.MainEnergy)
            };
                var staticEnergies = new List<NineStarKiEnergy>
            {
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Soil, EGetToTheRootKiEnergyType.MainEnergy),
                new NineStarKiEnergy(EGetToTheRootKiEnergy.CoreEarth, EGetToTheRootKiEnergyType.MainEnergy),
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Mountain, EGetToTheRootKiEnergyType.MainEnergy)
            };
                var flexibleEnergies = new List<NineStarKiEnergy>
            {
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Water, EGetToTheRootKiEnergyType.MainEnergy),
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Wind, EGetToTheRootKiEnergyType.MainEnergy),
                new NineStarKiEnergy(EGetToTheRootKiEnergy.Lake, EGetToTheRootKiEnergyType.MainEnergy)
            };

                return new GetToTheRootKiSummaryViewModel(mainEnergies, characterEnergies, dynamicEnergies, staticEnergies,
                    flexibleEnergies);
            }, TimeSpan.FromDays(30));
        }

        private string GetOverview(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetOverview_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {
                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_overview;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_overview;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_overview;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_overview;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_overview;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_overview;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_overview;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_overview;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_overview;
                }
                return string.Empty;

            }, TimeSpan.FromDays(30));
        }

        private string GetSummary(NineStarKiModel model)
        {
            var cacheKey = $"GetSummary_{model.PersonModel.IsAdult()}_{model.MainEnergy.Energy}_{model.CharacterEnergy.Energy}";
            return GetOrAddToCache(cacheKey, () =>
            {
                if (!model.PersonModel.IsAdult())
                {
                    switch (model.CharacterEnergy.Energy)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            return Dictionary.water_child;

                        case EGetToTheRootKiEnergy.Soil:
                            return Dictionary.soil_child;

                        case EGetToTheRootKiEnergy.Thunder:
                            return Dictionary.thunder_child;

                        case EGetToTheRootKiEnergy.Wind:
                            return Dictionary.wind_child;

                        case EGetToTheRootKiEnergy.CoreEarth:
                            return Dictionary.coreearth_child;

                        case EGetToTheRootKiEnergy.Heaven:
                            return Dictionary.heaven_child;

                        case EGetToTheRootKiEnergy.Lake:
                            return Dictionary.lake_child;

                        case EGetToTheRootKiEnergy.Mountain:
                            return Dictionary.mountain_child;

                        case EGetToTheRootKiEnergy.Fire:
                            return Dictionary.fire_child;
                    }
                }
                else
                {
                    switch (model.MainEnergy.Energy)
                    {
                        case EGetToTheRootKiEnergy.Water:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._115;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._124;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._133;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._142;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._151;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._169;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._178;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._187;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._196;
                            }

                            break;

                        case EGetToTheRootKiEnergy.Soil:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._216;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._225;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._234;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._243;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._252;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._261;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._279;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._288;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._297;
                            }

                            break;

                        case EGetToTheRootKiEnergy.Thunder:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._317;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._326;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._335;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._344;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._353;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._362;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._371;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._389;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._398;
                            }

                            break;

                        case EGetToTheRootKiEnergy.Wind:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._418;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._427;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._436;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._445;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._454;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._463;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._472;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._481;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._499;
                            }

                            return string.Empty;

                        case EGetToTheRootKiEnergy.CoreEarth:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._519;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._528;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._537;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._546;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._555;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._564;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._573;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._582;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._591;
                            }

                            return string.Empty;

                        case EGetToTheRootKiEnergy.Heaven:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._611;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._629;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._638;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._647;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._656;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._665;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._674;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._683;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._692;
                            }

                            return string.Empty;

                        case EGetToTheRootKiEnergy.Lake:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._712;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._721;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._739;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._748;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._757;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._766;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._775;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._784;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._793;
                            }

                            return string.Empty;

                        case EGetToTheRootKiEnergy.Mountain:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._813;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._822;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._831;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._849;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._858;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._867;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._876;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._885;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._894;
                            }

                            return string.Empty;

                        case EGetToTheRootKiEnergy.Fire:
                            switch (model.CharacterEnergy.Energy)
                            {
                                case EGetToTheRootKiEnergy.Water:
                                    return Dictionary._914;

                                case EGetToTheRootKiEnergy.Soil:
                                    return Dictionary._923;

                                case EGetToTheRootKiEnergy.Thunder:
                                    return Dictionary._932;

                                case EGetToTheRootKiEnergy.Wind:
                                    return Dictionary._941;

                                case EGetToTheRootKiEnergy.CoreEarth:
                                    return Dictionary._959;

                                case EGetToTheRootKiEnergy.Heaven:
                                    return Dictionary._968;

                                case EGetToTheRootKiEnergy.Lake:
                                    return Dictionary._977;

                                case EGetToTheRootKiEnergy.Mountain:
                                    return Dictionary._986;

                                case EGetToTheRootKiEnergy.Fire:
                                    return Dictionary._995;
                            }

                            return string.Empty;
                    }
                }

                return string.Empty;
            }, TimeSpan.FromDays(30));
        }

        private string GetMainEnergyDescription(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetMainEnergyDescription_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {
                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_description;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_description;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_description;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_description;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_description;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_description;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_description;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_description;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_description;
                }

                return string.Empty;

            }, TimeSpan.FromDays(30));
        }

        private string GetCharacterEnergyDescription(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetCharacterEnergyDescription_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {
                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_emotional_description;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_emotional_description;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_emotional_description;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_emotional_description;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_emotional_description;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_emotional_description;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_emotional_description;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_emotional_description;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_emotional_description;
                }

                return string.Empty;

            }, TimeSpan.FromDays(30));
        }

        private string GetSurfaceEnergyDescription(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetSurfaceEnergyDescription_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {
                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_surface_description;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_surface_description;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_surface_description;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_surface_description;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_surface_description;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_surface_description;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_surface_description;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_surface_description;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_surface_description;
                }

                return string.Empty;

            }, TimeSpan.FromDays(30));
        }

        private string GetHealth(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetHealth_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {

                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_health;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_health;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_health;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_health;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_health;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_health;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_health;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_health;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_health;
                }

                return string.Empty;

            }, TimeSpan.FromDays(30));
        }

        private string GetOccupations(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetOccupations_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {

                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_occupations;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_occupations;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_occupations;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_occupations;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_occupations;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_occupations;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_occupations;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_occupations;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_occupations;
                }

                return string.Empty;

            }, TimeSpan.FromDays(30));
        }

        private string GetPersonalDevelopemnt(EGetToTheRootKiEnergy energy)
        {
            var cacheKey = $"GetPersonalDevelopemnt_{energy}";
            return GetOrAddToCache(cacheKey, () =>
            {

                switch (energy)
                {
                    case EGetToTheRootKiEnergy.Water:
                        return Dictionary.water_personal_development;

                    case EGetToTheRootKiEnergy.Soil:
                        return Dictionary.soil_personal_development;

                    case EGetToTheRootKiEnergy.Thunder:
                        return Dictionary.thunder_personal_development;

                    case EGetToTheRootKiEnergy.Wind:
                        return Dictionary.wind_personal_development;

                    case EGetToTheRootKiEnergy.CoreEarth:
                        return Dictionary.coreearth_personal_development;

                    case EGetToTheRootKiEnergy.Heaven:
                        return Dictionary.heaven_personal_development;

                    case EGetToTheRootKiEnergy.Lake:
                        return Dictionary.lake_personal_development;

                    case EGetToTheRootKiEnergy.Mountain:
                        return Dictionary.mountain_personal_development;

                    case EGetToTheRootKiEnergy.Fire:
                        return Dictionary.fire_personal_development;
                }

                return string.Empty;

            }, TimeSpan.FromDays(30));
        }
    }
}