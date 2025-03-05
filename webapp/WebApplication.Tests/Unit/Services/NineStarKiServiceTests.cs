using K9.Base.DataAccessLayer.Enums;
using K9.SharedLibrary.Models;
using K9.WebApplication.Enums;
using K9.WebApplication.Models;
using K9.WebApplication.Services;
using Moq;
using System;
using System.Diagnostics;
using Xunit;

namespace K9.WebApplication.Tests.Unit.Services
{
    public class GetToTheRootKiServiceTests
    {
        public GetToTheRootKiServiceTests()
        {
        }

        [Theory]
        [InlineData(1979, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        [InlineData(1980, EGetToTheRootKiEnergy.Soil, EGender.Male)]
        [InlineData(1981, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1982, EGetToTheRootKiEnergy.Fire, EGender.Male)]
        [InlineData(1983, EGetToTheRootKiEnergy.Mountain, EGender.Male)]
        [InlineData(1984, EGetToTheRootKiEnergy.Lake, EGender.Male)]
        [InlineData(1985, EGetToTheRootKiEnergy.Heaven, EGender.Male)]
        [InlineData(1986, EGetToTheRootKiEnergy.CoreEarth, EGender.Male)]
        [InlineData(1987, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        public void YearEnergy_HappyPath(int year, EGetToTheRootKiEnergy energy, EGender gender)
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year, 2, 4),
                Gender = gender
            });
            Assert.Equal(energy, GetToTheRoot.MainEnergy.Energy);
        }

        [Theory]
        [InlineData(1979, EGetToTheRootKiEnergy.Thunder, EGender.Female)]
        [InlineData(1980, EGetToTheRootKiEnergy.Wind, EGender.Female)]
        [InlineData(1982, EGetToTheRootKiEnergy.Heaven, EGender.Female)]
        [InlineData(1983, EGetToTheRootKiEnergy.Lake, EGender.Female)]
        [InlineData(1984, EGetToTheRootKiEnergy.Mountain, EGender.Female)]
        [InlineData(1985, EGetToTheRootKiEnergy.Fire, EGender.Other)]
        [InlineData(1986, EGetToTheRootKiEnergy.Water, EGender.Female)]
        [InlineData(1987, EGetToTheRootKiEnergy.Soil, EGender.Female)]
        public void YearEnergyFemale_HappyPath(int year, EGetToTheRootKiEnergy energy, EGender gender)
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year, 2, 4),
                Gender = gender
            });
            Assert.Equal(energy, GetToTheRoot.MainEnergy.Energy);
        }

        [Fact]
        public void YearEnergyBeforeFeb4_HappyPath()
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(1979, 2, 3),
                Gender = EGender.Male
            });
            Assert.Equal(EGetToTheRootKiEnergy.Wind, GetToTheRoot.MainEnergy.Energy);
        }

        [Theory]
        [InlineData(1979, 1976, 1973, 2, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.CoreEarth, EGender.Male)]
        [InlineData(1979, 1976, 1973, 3, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        [InlineData(1979, 1976, 1973, 4, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        [InlineData(1979, 1976, 1973, 5, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Soil, EGender.Male)]
        [InlineData(1979, 1976, 1973, 6, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1979, 1976, 1973, 7, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Fire, EGender.Male)]
        [InlineData(1979, 1976, 1973, 8, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Mountain, EGender.Male)]
        [InlineData(1979, 1976, 1973, 9, 8, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Lake, EGender.Male)]
        [InlineData(1979, 1976, 1973, 10, 8, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGender.Male)]
        [InlineData(1979, 1976, 1973, 11, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.CoreEarth, EGender.Male)]
        [InlineData(1979, 1976, 1973, 12, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        [InlineData(1980, 1977, 1974, 1, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        [InlineData(1980, 1977, 1974, 2, 3, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        [InlineData(1979, 1976, 1973, 2, 3, EGetToTheRootKiEnergy.Wind, EGetToTheRootKiEnergy.Lake, EGetToTheRootKiEnergy.Water, EGetToTheRootKiEnergy.Heaven, EGender.Male)]
        [InlineData(1979, 1976, 1973, 3, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.CoreEarth, EGender.Male)]
        [InlineData(1979, 1976, 1973, 4, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        [InlineData(1979, 1976, 1973, 5, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        [InlineData(1979, 1976, 1973, 6, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Soil, EGender.Male)]
        [InlineData(1979, 1976, 1973, 7, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1979, 1976, 1973, 8, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Fire, EGender.Male)]
        [InlineData(1979, 1976, 1973, 9, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Mountain, EGender.Male)]
        [InlineData(1979, 1976, 1973, 10, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Lake, EGender.Male)]
        [InlineData(1979, 1976, 1973, 11, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGender.Male)]
        [InlineData(1979, 1976, 1973, 12, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.CoreEarth, EGender.Male)]
        [InlineData(1980, 1977, 1974, 1, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        [InlineData(1980, 1977, 1974, 2, 3, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        public void MonthEnergy_HappyPath(int year, int year2, int year3, int month, int day, EGetToTheRootKiEnergy year1Energy, EGetToTheRootKiEnergy year2Energy, EGetToTheRootKiEnergy year3Energy, EGetToTheRootKiEnergy monthEnergy, EGender gender)
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year, month, day),
                Gender = gender
            });
            var GetToTheRoot2 = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year2, month, day),
                Gender = gender
            });
            var GetToTheRoot3 = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year3, month, day),
                Gender = gender
            });
            Assert.Equal(year1Energy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(year2Energy, GetToTheRoot2.MainEnergy.Energy);
            Assert.Equal(year3Energy, GetToTheRoot3.MainEnergy.Energy);

            Assert.Equal(year1Energy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(year2Energy, GetToTheRoot2.MainEnergy.Energy);
            Assert.Equal(year3Energy, GetToTheRoot3.MainEnergy.Energy);

            Assert.Equal(year1Energy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(year2Energy, GetToTheRoot2.MainEnergy.Energy);
            Assert.Equal(year3Energy, GetToTheRoot3.MainEnergy.Energy);

            Assert.Equal(monthEnergy, GetToTheRoot.CharacterEnergy.Energy);
            Assert.Equal(monthEnergy, GetToTheRoot2.CharacterEnergy.Energy);
            Assert.Equal(monthEnergy, GetToTheRoot3.CharacterEnergy.Energy);
        }

        [Theory]
        [InlineData(1979, 1976, 1973, 2, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Water, EGender.Female)]
        [InlineData(1979, 1976, 1973, 3, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Soil, EGender.Female)]
        [InlineData(1979, 1976, 1973, 4, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Thunder, EGender.Female)]
        [InlineData(1979, 1976, 1973, 5, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Wind, EGender.Female)]
        [InlineData(1979, 1976, 1973, 6, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.CoreEarth, EGender.Female)]
        [InlineData(1979, 1976, 1973, 7, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Heaven, EGender.Female)]
        [InlineData(1979, 1976, 1973, 8, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Lake, EGender.Female)]
        [InlineData(1979, 1976, 1973, 9, 8, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Mountain, EGender.Female)]
        [InlineData(1979, 1976, 1973, 10, 8, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGender.Female)]
        [InlineData(1979, 1976, 1973, 11, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Water, EGender.Female)]
        [InlineData(1979, 1976, 1973, 12, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Soil, EGender.Female)]
        [InlineData(1980, 1977, 1974, 1, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Thunder, EGender.Female)]
        [InlineData(1980, 1977, 1974, 2, 3, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Thunder, EGender.Female)]
        [InlineData(1979, 1976, 1973, 2, 3, EGetToTheRootKiEnergy.Soil, EGetToTheRootKiEnergy.Mountain, EGetToTheRootKiEnergy.CoreEarth, EGetToTheRootKiEnergy.Heaven, EGender.Female)]
        [InlineData(1979, 1976, 1973, 3, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Water, EGender.Female)]
        [InlineData(1979, 1976, 1973, 4, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Soil, EGender.Female)]
        [InlineData(1979, 1976, 1973, 5, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Thunder, EGender.Female)]
        [InlineData(1979, 1976, 1973, 6, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Wind, EGender.Female)]
        [InlineData(1979, 1976, 1973, 7, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.CoreEarth, EGender.Female)]
        [InlineData(1979, 1976, 1973, 8, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Heaven, EGender.Female)]
        [InlineData(1979, 1976, 1973, 9, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Lake, EGender.Female)]
        [InlineData(1979, 1976, 1973, 10, 7, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Mountain, EGender.Female)]
        [InlineData(1979, 1976, 1973, 11, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Fire, EGender.Female)]
        [InlineData(1979, 1976, 1973, 12, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Water, EGender.Female)]
        [InlineData(1980, 1977, 1974, 1, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Soil, EGender.Female)]
        [InlineData(1980, 1977, 1974, 2, 3, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Thunder, EGender.Female)]
        public void MonthEnergy_Yin_HappyPath(int year, int year2, int year3, int month, int day, EGetToTheRootKiEnergy year1Energy, EGetToTheRootKiEnergy year2Energy, EGetToTheRootKiEnergy year3Energy, EGetToTheRootKiEnergy monthEnergy, EGender gender)
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year, month, day),
                Gender = gender
            });
            var GetToTheRoot2 = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year2, month, day),
                Gender = gender
            });
            var GetToTheRoot3 = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year3, month, day),
                Gender = gender
            });
            Assert.Equal(year1Energy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(year2Energy, GetToTheRoot2.MainEnergy.Energy);
            Assert.Equal(year3Energy, GetToTheRoot3.MainEnergy.Energy);

            Assert.Equal(year1Energy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(year2Energy, GetToTheRoot2.MainEnergy.Energy);
            Assert.Equal(year3Energy, GetToTheRoot3.MainEnergy.Energy);

            Assert.Equal(year1Energy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(year2Energy, GetToTheRoot2.MainEnergy.Energy);
            Assert.Equal(year3Energy, GetToTheRoot3.MainEnergy.Energy);

            Assert.Equal(monthEnergy, GetToTheRoot.CharacterEnergy.Energy);
            Assert.Equal(monthEnergy, GetToTheRoot2.CharacterEnergy.Energy);
            Assert.Equal(monthEnergy, GetToTheRoot3.CharacterEnergy.Energy);
        }

        [Theory]
        [InlineData(1979, 2, 4, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.CoreEarth, EGetToTheRootKiEnergy.Thunder, EGender.Male)]
        [InlineData(1976, 3, 6, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Wind, EGetToTheRootKiEnergy.Lake, EGender.Male)]
        [InlineData(1973, 4, 5, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Soil, EGender.Male)]
        [InlineData(1979, 5, 5, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Soil, EGetToTheRootKiEnergy.Heaven, EGender.Male)]
        [InlineData(1976, 6, 6, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Water, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1980, 7, 7, EGetToTheRootKiEnergy.Soil, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1981, 8, 7, EGetToTheRootKiEnergy.Water, EGetToTheRootKiEnergy.Soil, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        [InlineData(1982, 9, 8, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Lake, EGetToTheRootKiEnergy.Lake, EGender.Male)]
        [InlineData(1983, 10, 8, EGetToTheRootKiEnergy.Mountain, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1984, 11, 7, EGetToTheRootKiEnergy.Lake, EGetToTheRootKiEnergy.Mountain, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        [InlineData(1985, 3, 5, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.CoreEarth, EGetToTheRootKiEnergy.Heaven, EGender.Male)]
        [InlineData(1986, 12, 7, EGetToTheRootKiEnergy.CoreEarth, EGetToTheRootKiEnergy.Water, EGetToTheRootKiEnergy.Fire, EGender.Male)]
        [InlineData(1987, 2, 4, EGetToTheRootKiEnergy.Wind, EGetToTheRootKiEnergy.Mountain, EGetToTheRootKiEnergy.Water, EGender.Male)]
        [InlineData(1988, 3, 6, EGetToTheRootKiEnergy.Thunder, EGetToTheRootKiEnergy.Wind, EGetToTheRootKiEnergy.Wind, EGender.Male)]
        public void SurfaceEnergy_HappyPath(int year, int month, int day, EGetToTheRootKiEnergy yearEnergy, EGetToTheRootKiEnergy monthEnergy, EGetToTheRootKiEnergy surfaceEnergy, EGender gender)
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(year, month, day),
                Gender = gender
            });

            Assert.Equal(yearEnergy, GetToTheRoot.MainEnergy.Energy);
            Assert.Equal(monthEnergy, GetToTheRoot.CharacterEnergy.Energy);
            Assert.Equal(surfaceEnergy, GetToTheRoot.SurfaceEnergy.Energy);
        }

        [Theory]
        [InlineData(1979, 6, 16, 2020, 12, 14, EGender.Male, EGetToTheRootKiEnergy.Water, EGetToTheRootKiEnergy.Lake)]
        [InlineData(1979, 6, 16, 2020, 12, 14, EGender.Female, EGetToTheRootKiEnergy.CoreEarth, EGetToTheRootKiEnergy.CoreEarth)]
        [InlineData(1980, 6, 16, 2020, 12, 14, EGender.Male, EGetToTheRootKiEnergy.Fire, EGetToTheRootKiEnergy.Wind)]
        [InlineData(1978, 6, 16, 2020, 12, 14, EGender.Male, EGetToTheRootKiEnergy.Soil, EGetToTheRootKiEnergy.Water)]
        [InlineData(1978, 6, 16, 2020, 12, 14, EGender.Female, EGetToTheRootKiEnergy.Wind, EGetToTheRootKiEnergy.Mountain)]
        [InlineData(1980, 6, 16, 2020, 12, 14, EGender.Female, EGetToTheRootKiEnergy.Heaven, EGetToTheRootKiEnergy.Soil)]
        public void LifeCycle_HappyPath(int birthYear, int birthMonth, int birthDay, int year, int month, int day, EGender gender, EGetToTheRootKiEnergy yearlyCycleEnergy, EGetToTheRootKiEnergy monthlyCycleEnergy)
        {
            var GetToTheRoot = new NineStarKiModel(new PersonModel
            {
                DateOfBirth = new DateTime(birthYear, birthMonth, birthDay),
                Gender = gender
            }, new DateTime(year, month, day));

            Assert.Equal(yearlyCycleEnergy, GetToTheRoot.YearlyCycleEnergy.Energy);
            Assert.Equal(monthlyCycleEnergy, GetToTheRoot.MonthlyCycleEnergy.Energy);
        }

        [Fact]
        public void OutputEnergies()
        {
            var dobYear = 1979;
            for (int yearNumber = 0; yearNumber < 9; yearNumber++)
            {
                var dobMonth = 1;
                for (int monthNumber = 0; monthNumber < 9; monthNumber++)
                {
                    var dob = new DateTime(dobYear, dobMonth, 10);
                    var energy = new NineStarKiModel(new PersonModel
                    {
                        Gender = EGender.Male,
                        DateOfBirth = dob
                    });
                    Debug.WriteLine($"{energy.MainEnergy.EnergyNumber} - {energy.CharacterEnergy.EnergyNumber} - {energy.SurfaceEnergy.EnergyNumber}");
                    dobMonth++;
                }
                dobYear++;
            }
        }

        //[Theory]
        //[InlineData(1979, 6, 16, EGender.Male, 1984, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyHigh)]
        //[InlineData(1981, 6, 16, EGender.Male, 1984, 6, 21, EGender.Male, ECompatibilityScore.MediumToHigh)]
        //[InlineData(1980, 6, 16, EGender.Male, 1984, 6, 21, EGender.Male, ECompatibilityScore.Medium)]
        //[InlineData(1979, 6, 16, EGender.Male, 1983, 6, 21, EGender.Male, ECompatibilityScore.VeryHigh)]
        //[InlineData(1979, 6, 16, EGender.Male, 1985, 6, 21, EGender.Male, ECompatibilityScore.High)]
        //[InlineData(1979, 6, 16, EGender.Male, 1978, 6, 21, EGender.Male, ECompatibilityScore.LowToMedium)]
        //[InlineData(1979, 6, 16, EGender.Male, 1979, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyLow)]
        //[InlineData(1979, 6, 16, EGender.Male, 1982, 6, 21, EGender.Male, ECompatibilityScore.MediumToHigh)]
        //[InlineData(1979, 6, 16, EGender.Male, 1980, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyHigh)]
        //[InlineData(1982, 6, 16, EGender.Male, 1985, 6, 21, EGender.Male, ECompatibilityScore.VeryHigh)]
        //public void Calculate_ChemistryLevel(int year1, int month1, int day1, EGender gender1, int year2, int month2, int day2, EGender gender2, ECompatibilityScore chemistryScore)
        //{
        //    var mockAuthentication = new Mock<IAuthentication>();
        //    mockAuthentication.SetupGet(e => e.CurrentUserId).Returns(2);
        //    mockAuthentication.SetupGet(e => e.IsAuthenticated).Returns(true);

        //    var GetToTheRootKiService = new GetToTheRootKiService(new Mock<IMembershipService>().Object, mockAuthentication.Object, new Mock<IRoles>().Object);

        //    var compatibility = GetToTheRootKiService.CalculateCompatibility(new PersonModel
        //    {
        //        DateOfBirth = new DateTime(year1, month1, day1),
        //        Gender = gender1
        //    }, new PersonModel
        //    {
        //        DateOfBirth = new DateTime(year2, month2, day2),
        //        Gender = gender2
        //    }); 
            
        //    Assert.Equal(chemistryScore, compatibility.CompatibilityDetails.Score.SparkScore);
        //}

        //[Theory]
        //[InlineData(1979, 6, 16, EGender.Male, 1984, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyHigh)]
        //[InlineData(1981, 6, 16, EGender.Male, 1984, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyLow)]
        //[InlineData(1980, 6, 16, EGender.Male, 1984, 6, 21, EGender.Male, ECompatibilityScore.Low)]
        //[InlineData(1979, 6, 16, EGender.Male, 1983, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyHigh)]
        //[InlineData(1979, 6, 16, EGender.Male, 1985, 6, 21, EGender.Male, ECompatibilityScore.VeryHigh)]
        //[InlineData(1979, 6, 16, EGender.Male, 1978, 6, 21, EGender.Male, ECompatibilityScore.Low)]
        //[InlineData(1979, 6, 16, EGender.Male, 1979, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyLow)]
        //[InlineData(1979, 6, 16, EGender.Male, 1982, 6, 21, EGender.Male, ECompatibilityScore.Low)]
        //[InlineData(1979, 6, 16, EGender.Male, 1980, 6, 21, EGender.Male, ECompatibilityScore.ExtremelyHigh)]
        //[InlineData(1982, 6, 16, EGender.Male, 1985, 6, 21, EGender.Male, ECompatibilityScore.High)]
        //public void Calculate_ConflictLevel(int year1, int month1, int day1, EGender gender1, int year2, int month2, int day2, EGender gender2, ECompatibilityScore conflictScore)
        //{
        //    var mockAuthentication = new Mock<IAuthentication>();
        //    mockAuthentication.SetupGet(e => e.CurrentUserId).Returns(2);
        //    mockAuthentication.SetupGet(e => e.IsAuthenticated).Returns(true);

        //    var GetToTheRootKiService = new GetToTheRootKiService(new Mock<IMembershipService>().Object, mockAuthentication.Object, new Mock<IRoles>().Object);

        //    var compatibility = GetToTheRootKiService.CalculateCompatibility(new PersonModel
        //    {
        //        DateOfBirth = new DateTime(year1, month1, day1),
        //        Gender = gender1
        //    }, new PersonModel
        //    {
        //        DateOfBirth = new DateTime(year2, month2, day2),
        //        Gender = gender2
        //    }); 

        //    Assert.Equal(conflictScore, compatibility.CompatibilityDetails.Score.ConflictScore);
        //}

    }
}
