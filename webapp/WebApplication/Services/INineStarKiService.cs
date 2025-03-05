using K9.Base.DataAccessLayer.Enums;
using K9.WebApplication.Models;
using System;

namespace K9.WebApplication.Services
{
    public interface INineStarKiService : IBaseService
    {
        NineStarKiModel CalculateNineStarKiKiProfile(DateTime dateOfBirth, EGender gender = EGender.Male);
        NineStarKiModel CalculateNineStarKiKiProfile(PersonModel personModel, bool isCompatibility = false, bool isMyProfile = false,  DateTime? today = null);
    }
}