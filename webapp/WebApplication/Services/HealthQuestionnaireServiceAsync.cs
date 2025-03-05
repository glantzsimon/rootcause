using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Attributes;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using K9.WebApplication.Config;
using K9.WebApplication.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K9.WebApplication.Services
{
    public class HealthQuestionnaireServiceAsync : HealthQuestionnaireServiceBase, IHealthQuestionnaireServiceAsync
    {
        private readonly IRepository<HealthQuestionnaire> _healthQuestionnaireRepository;
        private readonly IRepository<User> _usersRepository;
        private readonly IRepository<Client> _clientsRepository;
        private readonly IClientService _clientService;
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<ProductPack> _productPacksRepository;
        private readonly IRepository<Ingredient> _ingredientsRepository;
        private readonly IRepository<ProductIngredient> _productIingredientsRepository;
        private readonly IRepository<Protocol> _protocolsRepository;
        private readonly IRepository<Activity> _activitiesRepository;
        private readonly IRepository<DietaryRecommendation> _dietaryRecommendationsRepository;
        private readonly IRepository<FoodItem> _foodItemsRepository;
        private readonly IRepository<ProtocolActivity> _protocolActivitiesRepository;
        private readonly IRepository<ProtocolDietaryRecommendation> _protocolDietaryRecommendationRepository;
        private readonly IRepository<ProtocolFoodItem> _protocolFoodItemsRepository;
        private readonly IRepository<ProtocolProductPack> _protocolProductPackRepository;
        private readonly IRepository<ProtocolProduct> _protocolProductsRepository;
        private readonly IProtocolService _protocolService;

        public HealthQuestionnaireServiceAsync(ILogger logger,
            IRepository<Product> productsRepository,
            IRepository<ProductPack> productPackRepository, IOptions<DefaultValuesConfiguration> defaultValues,
            IRepository<HealthQuestionnaire> healthQuestionnaireRepository,
            IRepository<User> usersRepository,
            IRepository<Client> clientsRepository,
            IClientService clientService,
            IRepository<ProductPack> productPacksRepository,
            IRepository<Protocol> protocolsRepository, IRepository<Activity> activitiesRepository,
            IRepository<DietaryRecommendation> dietaryRecommendationsRepository,
            IRepository<FoodItem> foodItemsRepository,
            IRepository<Ingredient> ingredientsRepository,
            IRepository<ProductIngredient> productIngredientsRepository,
            IRepository<IngredientSubstitute> ingredientSubstitutesRepository,
            IRepository<ProtocolActivity> protocolActivitiesRepository,
            IRepository<ProductIngredientSubstitute> productIngredientSubstitutesRepository,
            IRepository<ProtocolDietaryRecommendation> protocolDietaryRecommendationRepository,
            IRepository<ProtocolFoodItem> protocolFoodItemsRepository,
            IRepository<ProtocolProductPack> protocolProductPackRepository,
            IRepository<ProductPackProduct> productPackProductsRepository,
            IRepository<ProtocolProduct> protocolProductsRepository,
            IProtocolService protocolService) :
            base(logger, productsRepository, productPackRepository, defaultValues, healthQuestionnaireRepository, usersRepository,
                clientsRepository, clientService, productPacksRepository, protocolsRepository, activitiesRepository, dietaryRecommendationsRepository,
                foodItemsRepository, ingredientsRepository, productIngredientsRepository, ingredientSubstitutesRepository, protocolActivitiesRepository, productIngredientSubstitutesRepository, protocolDietaryRecommendationRepository, protocolFoodItemsRepository, protocolProductPackRepository, productPackProductsRepository, protocolProductsRepository, protocolService)
        {
            _healthQuestionnaireRepository = healthQuestionnaireRepository;
            _usersRepository = usersRepository;
            _clientsRepository = clientsRepository;
            _clientService = clientService;
            _productsRepository = productsRepository;
            _productPacksRepository = productPacksRepository;
            _protocolsRepository = protocolsRepository;
            _ingredientsRepository = ingredientsRepository;
            _productIingredientsRepository = productIngredientsRepository;
            _activitiesRepository = activitiesRepository;
            _dietaryRecommendationsRepository = dietaryRecommendationsRepository;
            _foodItemsRepository = foodItemsRepository;
            _protocolActivitiesRepository = protocolActivitiesRepository;
            _protocolDietaryRecommendationRepository = protocolDietaryRecommendationRepository;
            _protocolFoodItemsRepository = protocolFoodItemsRepository;
            _protocolProductPackRepository = protocolProductPackRepository;
            _protocolProductsRepository = protocolProductsRepository;
            _protocolService = protocolService;
        }

        public async Task AutoGenerateProtocolFromGeneticProfileAsync(HealthQuestionnaire hq)
        {
            await Task.Run(() => AutoGenerateProtocolFromGeneticProfile(hq));
        }

        public void AutoGenerateProtocolFromGeneticProfile(HealthQuestionnaire hq)
        {
            if (hq.IsComplete())
            {
                var protocol = _protocolsRepository
                    .Find(e => e.Type == EProtocolType.AutoGenerated && e.ClientId == hq.ClientId).FirstOrDefault();

                if (protocol == null)
                {
                    // Create new one
                    var externalId = Guid.NewGuid();

                    var clientRecord = _clientService.Find(hq.ClientId);

                    protocol = new Protocol
                    {
                        Name = $"{K9.Globalisation.Dictionary.GenoTypePersonalisedProtocol} - {clientRecord.FullName}",
                        ShortDescription = K9.Globalisation.Dictionary.GenoTypePersonalisedProtocol,
                        ClientId = hq.ClientId,
                        ExternalId = externalId,
                        Type = EProtocolType.AutoGenerated,
                        Period = EPeriod.Days,
                        ProtocolFrequency = EProtocolFrequency.Daily,
                        NumberOfPeriodsOff = 1,
                        Duration = EProtocolDuration.ThreeMonths,
                        GenoType = hq.CalculateGenotype().GenoType
                    };

                    _protocolsRepository.Create(protocol);
                    protocol = _protocolsRepository.Find(e => e.ExternalId == externalId).First();
                }
                else
                {
                    protocol.GenoType = hq.CalculateGenotype().GenoType;
                    _protocolsRepository.Update(protocol);
                }

                DeleteProtocolChildRecords(protocol.Id);
                RecreateChildRecords(hq, protocol);

                ClearCache();
            }
        }

        private void RecreateChildRecords(HealthQuestionnaire hq, Protocol protocol)
        {
            var matchedItems = GetGeneticProfileMatchedItems(hq.Id);

            if (matchedItems != null)
            {
                protocol.Activities = matchedItems.Activities.Select(e => new ProtocolActivity
                {
                    ProtocolId = protocol.Id,
                    ActivityId = e.ActivityId,
                    Score = e.Score,
                    RelativeScore = e.RelativeScore
                }).ToList();

                protocol.DietaryRecommendations = matchedItems.DietaryRecommendations.Select(e =>
                    new ProtocolDietaryRecommendation
                    {
                        ProtocolId = protocol.Id,
                        DietaryRecommendationId = e.DietaryRecommendationId,
                        Score = e.Score,
                        RelativeScore = e.RelativeScore
                    }).ToList();

                protocol.RecommendedFoods = matchedItems.Foods.Select(e => new ProtocolFoodItem
                {
                    ProtocolId = protocol.Id,
                    FoodItemId = e.FoodItemId,
                    Score = e.Score,
                    RelativeScore = e.RelativeScore
                }).ToList();

                protocol.Products = matchedItems.Products.Select(e => new ProtocolProduct
                {
                    ProtocolId = protocol.Id,
                    ProductId = e.ProductId,
                    Score = e.Score,
                    RelativeScore = e.RelativeScore
                }).ToList();

                protocol.ProductPacks = matchedItems.ProductPacks.Select(e => new ProtocolProductPack
                {
                    ProtocolId = protocol.Id,
                    ProductPackId = e.ProductPackId,
                    Score = e.Score,
                    RelativeScore = e.RelativeScore
                }).ToList();

                _protocolActivitiesRepository.CreateBatch(protocol.Activities);
                _protocolDietaryRecommendationRepository.CreateBatch(protocol.DietaryRecommendations);
                _protocolFoodItemsRepository.CreateBatch(protocol.RecommendedFoods);
                _protocolProductsRepository.CreateBatch(protocol.Products);
                _protocolProductPackRepository.CreateBatch(protocol.ProductPacks);
            }

            ClearCache();
        }

        private void DeleteProtocolChildRecords(int id)
        {
            var activities = _protocolActivitiesRepository.Find(e => e.ProtocolId == id).ToList();
            if (activities != null)
            {
                _protocolActivitiesRepository.DeleteBatch(activities);
            }

            var dietaryRecommendations = _protocolDietaryRecommendationRepository.Find(e => e.ProtocolId == id).ToList();
            if (dietaryRecommendations != null)
            {
                _protocolDietaryRecommendationRepository.DeleteBatch(dietaryRecommendations);
            }

            var recommendedFoods = _protocolFoodItemsRepository.Find(e => e.ProtocolId == id).ToList();
            if (recommendedFoods != null)
            {
                _protocolFoodItemsRepository.DeleteBatch(recommendedFoods);
            }

            var products = _protocolProductsRepository.Find(e => e.ProtocolId == id).ToList();
            if (products != null)
            {
                _protocolProductsRepository.DeleteBatch(products);
            }

            var productPacks = _protocolProductPackRepository.Find(e => e.ProtocolId == id).ToList();
            if (productPacks != null)
            {
                _protocolProductPackRepository.DeleteBatch(productPacks);
            }
        }

    }
}