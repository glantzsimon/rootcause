using System;
using System.Diagnostics;
using System.Linq;
using K9.Base.DataAccessLayer.Enums;
using K9.SharedLibrary.Models;
using K9.WebApplication.Enums;
using K9.WebApplication.Models;
using K9.WebApplication.Services;
using Moq;
using Xunit;

namespace K9.WebApplication.Tests.Unit.Services
{
    public class BioRhythmsServiceTests
    {
        public BioRhythmsServiceTests()
        {
        }

        [Theory]
        [InlineData(1979, 06, 16, 1979, 06, 16, EGender.Male, EGetToTheRootKiEnergy.Thunder, 0, 0, 50, 50)]
        [InlineData(1979, 06, 16, 1979, 06, 19, EGender.Male, EGetToTheRootKiEnergy.Thunder, 3, 3, 77, 81)]
        [InlineData(1979, 06, 16, 1979, 06, 24, EGender.Male, EGetToTheRootKiEnergy.Thunder, 8, 8, 100, 99)]
        [InlineData(1979, 06, 16, 1979, 06, 26, EGender.Male, EGetToTheRootKiEnergy.Thunder, 10, 10, 97, 89)]
        [InlineData(1979, 06, 16, 1979, 07, 03, EGender.Male, EGetToTheRootKiEnergy.Thunder, 17, 17, 45, 19)]
        [InlineData(1979, 06, 16, 1979, 07, 19, EGender.Male, EGetToTheRootKiEnergy.Thunder, 33, 0, 50, 95)]
        [InlineData(1979, 06, 16, 1979, 08, 02, EGender.Male, EGetToTheRootKiEnergy.Thunder, 47, 14, 73, 5)]
        public void Biorhythms_HappyPath(int birthYear, int birthMonth, int birthDay, int dateYear, int dateMonth, int dateDay, EGender gender, EGetToTheRootKiEnergy expectedEnergy, int expectedDaysElapsedSinceBirth, int expectedDayInterval, double expectedIntellectualValue, double expectedEmotionalValue)
        {
            var biorhythmsService = new BiorhythmsService(new Mock<IRoles>().Object, new Mock<IMembershipService>().Object, new Mock<IAuthentication>().Object);

            var result = biorhythmsService.Calculate(new NineStarKiModel(
                new PersonModel
                {
                    DateOfBirth = new DateTime(birthYear, birthMonth, birthDay)
                }), new DateTime(dateYear, dateMonth, dateDay));

            Assert.Equal(expectedEnergy, result.GetToTheRootKiBioRhythms.GetToTheRootKiModel.MainEnergy.Energy);
            Assert.Equal(expectedDaysElapsedSinceBirth, result.GetToTheRootKiBioRhythms.DaysElapsedSinceBirth);
            
            var intellectualResult = result.BioRhythms.GetResultByType(EBiorhythm.Intellectual);
            var emotionalResult = result.BioRhythms.GetResultByType(EBiorhythm.Emotional);

            Assert.Equal(expectedDayInterval, intellectualResult?.DayInterval);
            Assert.Equal(expectedIntellectualValue, Math.Round(intellectualResult.Value, MidpointRounding.ToEven));
            Assert.Equal(expectedEmotionalValue, Math.Round(emotionalResult.Value, MidpointRounding.ToEven));
        }

        [Theory]
        [InlineData(1979, 06, 16, 1979, 06, 16, EGender.Male, EGetToTheRootKiEnergy.Thunder, 50, 50.2254)]
        public void GetToTheRootKiBiorhythms_PhysicalEnergy_HappyPath(int birthYear, int birthMonth, int birthDay, int dateYear, int dateMonth, int dateDay, EGender gender, EGetToTheRootKiEnergy expectedEnergy, double expectedBiorhythmValue, double expectedGetToTheRootKiBiorhythmsValue)
        {
            var biorhythmsService = new BiorhythmsService(new Mock<IRoles>().Object, new Mock<IMembershipService>().Object, new Mock<IAuthentication>().Object);

            var result = biorhythmsService.Calculate(new NineStarKiModel(
                new PersonModel
                {
                    DateOfBirth = new DateTime(birthYear, birthMonth, birthDay)
                }), new DateTime(dateYear, dateMonth, dateDay));

            var physicalResult = result.BioRhythms.GetResultByType(EBiorhythm.Physical);
            var GetToTheRootPhysicalResult = result.GetToTheRootKiBioRhythms.GetResultByType(EBiorhythm.Physical);

            Assert.Equal(expectedEnergy, result.GetToTheRootKiBioRhythms.GetToTheRootKiModel.MainEnergy.Energy);
            Assert.Equal(expectedBiorhythmValue, physicalResult.Value);
            //Assert.Equal(expectedGetToTheRootKiBiorhythmsValue, Math.Round(GetToTheRootPhysicalResult.Value, 4));
        }

    }
}
