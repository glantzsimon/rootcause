using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.ViewModels
{
    public class RecommendedFoodsViewModel
    {
        public int? ClientId { get; set; }
        public EGenoType GenoType { get; set; }
        public Protocol Protocol { get; set; }

        public List<ECompatibilityLevel> Levels => new List<ECompatibilityLevel>
        {
            ECompatibilityLevel.Optimal,
            ECompatibilityLevel.Excellent,
            ECompatibilityLevel.Neutral,
            ECompatibilityLevel.Suboptimal,
            ECompatibilityLevel.Unsuitable
        };

        public List<EFoodGroup> FoodGroups => new List<EFoodGroup>
        {
            EFoodGroup.Vegetables,
            EFoodGroup.Fruits,
            EFoodGroup.Proteins,
            EFoodGroup.Carbohydrates,
            EFoodGroup.FatsAndOils,
            EFoodGroup.Dairy,
            EFoodGroup.HerbsAndSpices,
            EFoodGroup.Other
        };

        public List<FoodItem> RecommendedFoods { get; set; }

        public List<FoodItem> GetBestFoods(EFoodGroup foodGroup)
        {
            var items = new List<FoodItem>();
            items.AddRange(GetRecommendedFoodsForLevelAndGroup(foodGroup, ECompatibilityLevel.Optimal));
            items.AddRange(GetRecommendedFoodsForLevelAndGroup(foodGroup, ECompatibilityLevel.Excellent));
            return items.OrderByDescending(e => e.Score).ThenBy(e => e.Name).ToList();
        }

        public List<FoodItem> GetWorstFoods(EFoodGroup foodGroup)
        {
            var items = new List<FoodItem>();
            items.AddRange(GetRecommendedFoodsForLevelAndGroup(foodGroup, ECompatibilityLevel.Suboptimal));
            items.AddRange(GetRecommendedFoodsForLevelAndGroup(foodGroup, ECompatibilityLevel.Unsuitable));
            return items.OrderByDescending(e => e.Score).ThenBy(e => e.Name).ToList();
        }

        public List<FoodItem> GetRecommendedFoodsForLevelAndGroup(EFoodGroup foodGroup, ECompatibilityLevel level)
        {
            switch (GenoType)
            {
                case EGenoType.Hunter:
                    if (foodGroup == EFoodGroup.Other)
                    {
                        return RecommendedFoods?.Where(e =>
                            e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                            (e.HunterCompatibilityLevel == level && level != ECompatibilityLevel.Neutral)).ToList();
                    }

                    return RecommendedFoods?.Where(e =>
                        e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                        (e.HunterCompatibilityLevel == level)).ToList();

                case EGenoType.Gatherer:
                    if (foodGroup == EFoodGroup.Other)
                    {
                        return RecommendedFoods?.Where(e =>
                            e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                            (e.GathererCompatibilityLevel == level && level != ECompatibilityLevel.Neutral)).ToList();
                    }

                    return RecommendedFoods?.Where(e =>
                        e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                        (e.GathererCompatibilityLevel == level)).ToList();

                case EGenoType.Teacher:
                    if (foodGroup == EFoodGroup.Other)
                    {
                        return RecommendedFoods?.Where(e =>
                            e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                            (e.TeacherCompatibilityLevel == level && level != ECompatibilityLevel.Neutral)).ToList();
                    }

                    return RecommendedFoods?.Where(e =>
                        e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                        (e.TeacherCompatibilityLevel == level)).ToList();

                case EGenoType.Explorer:
                    if (foodGroup == EFoodGroup.Other)
                    {
                        return RecommendedFoods?.Where(e =>
                            e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                            (e.ExplorerCompatibilityLevel == level && level != ECompatibilityLevel.Neutral)).ToList();
                    }

                    return RecommendedFoods?.Where(e =>
                        e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                        (e.ExplorerCompatibilityLevel == level)).ToList();

                case EGenoType.Warrior:
                    if (foodGroup == EFoodGroup.Other)
                    {
                        return RecommendedFoods?.Where(e =>
                            e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                            (e.WarriorCompatibilityLevel == level && level != ECompatibilityLevel.Neutral)).ToList();
                    }

                    return RecommendedFoods?.Where(e =>
                        e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                        (e.WarriorCompatibilityLevel == level)).ToList();

                case EGenoType.Nomad:
                    if (foodGroup == EFoodGroup.Other)
                    {
                        return RecommendedFoods?.Where(e =>
                            e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                            (e.NomadCompatibilityLevel == level && level != ECompatibilityLevel.Neutral)).ToList();
                    }

                    return RecommendedFoods?.Where(e =>
                        e.Category.GetAttribute<EFoodGroupMetaDataAttribute>().FoodGroup == foodGroup &&
                        (e.NomadCompatibilityLevel == level)).ToList();
            }

            return null;
        }
        
    }
}