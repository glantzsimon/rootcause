using K9.DataAccessLayer.Enums;
using System.Collections.Generic;

namespace K9.WebApplication.Constants
{
    public static class Constants
    {
        public const string Administrator = "Administrator";
        public const string PowerUser = "PowerUser";
        public const string DefaultUser = "DefaultUser";
        public const string ClientUser = "Client User";

        public const int CategoryGap = 100000;
        public const int ItemCodeGap = 222;
        public const int OneWeek = 604800;

        public static List<ECategory> IngredientCategories = new List<ECategory>
        {
            ECategory.AminoAcid,
            ECategory.Vitamin,
            ECategory.Mineral,
            ECategory.Phytonutrient,
            ECategory.Oil,
            ECategory.Herb,
            ECategory.Superfood,
            ECategory.Other
        };

        public static List<ECategory> ProductCategories = new List<ECategory>
        {
            ECategory.Alchemy,
            ECategory.Detox,
            ECategory.GeneticProfiles,
            ECategory.Herbal,
            ECategory.VitaminsMinerals,
            ECategory.DietarySupplement,
            ECategory.PersonalCare,
            ECategory.Other,
        };
    }
}