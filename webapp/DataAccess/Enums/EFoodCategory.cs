using K9.Base.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Attributes;

namespace K9.DataAccessLayer.Enums
{
    public enum EFoodCategory
    {
        [Score(
            Carnivore = true,
            Vegan = false,
            Vegetarian = false,
            Fruitarian = false,
            Pescatarian = false)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.RedMeat)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Proteins)]
        RedMeat,

        [Score(
            Carnivore = true,
            Vegan = false,
            Vegetarian = false,
            Fruitarian = false,
            Pescatarian = false)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Poultry)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Proteins)]
        Poultry,

        [Score(
            Carnivore = true,
            Vegan = false,
            Vegetarian = false,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FishAndSeafood)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Proteins)]
        FishAndSeafood,

        [Score(
            Carnivore = true,
            Vegan = false,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.EggsAndRoes)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Proteins)]
        EggsAndRoes,

        [Score(
            Carnivore = true,
            Vegan = false,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = false)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Dairy)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Dairy)]
        Dairy,

        [Score(
            Carnivore = false,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = true,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.VegetableProtein)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Proteins)]
        VegetableProtein,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FatsAndOils)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.FatsAndOils)]
        FatsAndOils,

        [Score(
            Carnivore = false,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Carbohydrates)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Carbohydrates)]
        Carbohydrates,

        [Score(
            Carnivore = false,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = true,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Vegetables)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Vegetables)]
        Vegetables,

        [Score(
            Carnivore = false,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Fungi)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Other)]
        Fungi,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = true,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Fruits)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Fruits)]
        Fruits,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = true,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.HerbsAndSpices)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.HerbsAndSpices)]
        HerbsAndSpices,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = true,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Beverages)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Other)]
        Beverages,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Condiments)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Other)]
        Condiments,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = false,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.FermentedFoods)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Other)]
        FermentedFoods,

        [Score(
            Carnivore = true,
            Vegan = true,
            Vegetarian = true,
            Fruitarian = true,
            Pescatarian = true)]
        [EnumDescription(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.Other)]
        [EFoodGroupMetaData(FoodGroup = EFoodGroup.Other)]
        Other
    }
}