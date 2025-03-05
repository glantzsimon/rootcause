using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Models;
using K9.WebApplication.Config;
using Microsoft.Extensions.Caching.Memory;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.Services
{
    public class ProtocolService : CacheableServiceBase<Protocol>, IProtocolService
    {
        private readonly ILogger _logger;
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<ProductPack> _productPackRepository;
        private readonly IRepository<Client> _clientsRepository;
        private readonly IRepository<User> _usersRepository;
        private readonly IRepository<Protocol> _protocolsRepository;
        private readonly IRepository<ProtocolProduct> _protocolProductsRepository;
        private readonly IRepository<ProtocolProductPack> _protocolProductPackRepository;
        private readonly IRepository<ProtocolSection> _protocolProtocolSectionRepository;
        private readonly IRepository<Section> _protocolSectionRepository;
        private readonly IRepository<ProtocolSectionProduct> _protocolProtocolSectionProductsRepository;
        private readonly IRepository<ProductPackProduct> _productPackProductRepository;
        private readonly IRepository<ProtocolActivity> _protocolActivitiesRepository;
        private readonly IRepository<Activity> _activitiesRepository;
        private readonly IRepository<ProtocolFoodItem> _protocolFoodItemsRepository;
        private readonly IRepository<HealthQuestionnaire> _healthQuestionnaiRepository;
        private readonly IRepository<ProductPackProduct> _productPackProductsRepository;
        private readonly IRepository<ClientForbiddenFood> _clientForbiddenFoodsRepository;
        private readonly IRepository<ProtocolDietaryRecommendation> _protocolDietaryRecommendationRepository;
        private readonly IRepository<DietaryRecommendation> _dietaryRecommendationRepository;
        private readonly DefaultValuesConfiguration _defaultValues;

        public ProtocolService(ILogger logger, IRepository<Product> productsRepository,
            IRepository<ProductPack> productPackRepository, IOptions<DefaultValuesConfiguration> defaultValues,
            IRepository<Client> clientsRepository, IRepository<User> usersRepository,
            IRepository<Protocol> protocolsRepository, IRepository<ProtocolProduct> protocolProductsRepository,
            IRepository<ProtocolProductPack> protocolProductPackRepository,
            IRepository<ProtocolSection> protocolProtocolSectionRepository,
            IRepository<Section> protocolSectionRepository,
            IRepository<ProtocolSectionProduct> protocolProtocolSectionProductsRepository,
            IRepository<ProductPackProduct> productPackProductRepository,
            IRepository<ProtocolActivity> protocolActivitiesRepository,
            IRepository<ProtocolDietaryRecommendation> protocolDietaryRecommendationRepository,
            IRepository<DietaryRecommendation> dietaryRecommendationRepository,
            IRepository<Ingredient> ingredientsRepository,
            IRepository<IngredientSubstitute> ingredientSubstitutesRepository,
            IRepository<ProductIngredient> productIngredientsRepository,
            IRepository<ProductIngredientSubstitute> productIngredientSubstitutesRepository,
            IRepository<Activity> activitiesRepository,
            IRepository<DietaryRecommendation> dietaryRecommendationsRepository,
            IRepository<FoodItem> foodItemsRepository,
            IRepository<ProtocolFoodItem> protocolFoodItemsRepository,
            IRepository<HealthQuestionnaire> healthQuestionnaiRepository,
            IRepository<ProductPackProduct> productPackProductsRepository,
            IRepository<ClientForbiddenFood> clientForbiddenFoodsRepository) :
            base(productsRepository, productPackRepository, ingredientsRepository, protocolsRepository,
                ingredientSubstitutesRepository, productIngredientsRepository, productIngredientSubstitutesRepository,
                activitiesRepository, dietaryRecommendationsRepository, productPackProductsRepository, foodItemsRepository)
        {

            _logger = logger;
            _productsRepository = productsRepository;
            _productPackRepository = productPackRepository;
            _clientsRepository = clientsRepository;
            _usersRepository = usersRepository;
            _protocolsRepository = protocolsRepository;
            _protocolProductsRepository = protocolProductsRepository;
            _protocolProductPackRepository = protocolProductPackRepository;
            _protocolProtocolSectionRepository = protocolProtocolSectionRepository;
            _protocolSectionRepository = protocolSectionRepository;
            _protocolProtocolSectionProductsRepository = protocolProtocolSectionProductsRepository;
            _productPackProductRepository = productPackProductRepository;
            _protocolActivitiesRepository = protocolActivitiesRepository;
            _activitiesRepository = activitiesRepository;
            _protocolFoodItemsRepository = protocolFoodItemsRepository;
            _healthQuestionnaiRepository = healthQuestionnaiRepository;
            _productPackProductsRepository = productPackProductsRepository;
            _clientForbiddenFoodsRepository = clientForbiddenFoodsRepository;
            _protocolDietaryRecommendationRepository = protocolDietaryRecommendationRepository;
            _dietaryRecommendationRepository = dietaryRecommendationRepository;
            _defaultValues = defaultValues.Value;
        }

        public Protocol FindAutoGenerated(int clientId)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(clientId), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(Constants.Constants.OneWeek));

                var protocol = _protocolsRepository.Find(e => e.ClientId == clientId && e.Type == EProtocolType.AutoGenerated).FirstOrDefault();
                if (protocol != null)
                {
                    protocol = GetFullProtocol(protocol);
                }

                return protocol;
            });
        }

        public Protocol Find(int id)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(id), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(Constants.Constants.OneWeek));

                var protocol = _protocolsRepository.Find(id);
                if (protocol != null)
                {
                    protocol = GetFullProtocol(protocol);
                }

                return protocol;
            });
        }

        public Protocol FindNext(int id)
        {
            var protocol = _protocolsRepository.Find(e => e.Id > id).OrderBy(e => e.Id).FirstOrDefault() ??
                           _protocolsRepository.GetQuery("SELECT TOP 1 * FROM [Protocol] ORDER BY [Id]")
                               .FirstOrDefault();
            if (protocol != null)
            {
                protocol = GetFullProtocol(protocol);
            }

            return protocol;
        }

        public Protocol FindPrevious(int id)
        {
            var protocol = _protocolsRepository.Find(e => e.Id < id).OrderByDescending(e => e.Id).FirstOrDefault() ??
                           _protocolsRepository.GetQuery("SELECT TOP 1 * FROM [Protocol] ORDER BY [Id] DESC")
                               .FirstOrDefault();
            if (protocol != null)
            {
                protocol = GetFullProtocol(protocol);
            }

            return protocol;
        }

        public Protocol Find(Guid id)
        {
            var protocol = _protocolsRepository.Find(e => e.ExternalId == id).FirstOrDefault();
            if (protocol != null)
            {
                protocol = GetFullProtocol(protocol);
            }

            return protocol;
        }

        public Protocol GetFullProtocol(Protocol protocol)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(protocol.Id), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(Constants.Constants.OneWeek));

                return GetFullProtocolNoCache(protocol);
            });
        }

        public Protocol GetFullProtocolNoCache(Protocol protocol)
        {
            HealthQuestionnaire hq = null;
            if (protocol.Type == EProtocolType.AutoGenerated && protocol.ClientId.HasValue)
            {
                // Copy food choices for display
                hq = _healthQuestionnaiRepository.Find(e => e.ClientId == protocol.ClientId).FirstOrDefault();

                protocol.EatsRedMeat = hq.EatsRedMeat;
                protocol.EatsPoultry = hq.EatsPoultry;
                protocol.EatsFishAndSeafood = hq.EatsFishAndSeafood;
                protocol.EatsEggsAndRoes = hq.EatsEggsAndRoes;
                protocol.EatsDairy = hq.EatsDairy;
                protocol.EatsVegetables = hq.EatsVegetables;
                protocol.EatsVegetableProtein = hq.EatsVegetableProtein;
                protocol.EatsFungi = hq.EatsFungi;
                protocol.EatsFruit = hq.EatsFruit;
                protocol.EatsGrains = hq.EatsGrains;
                protocol.IsLowOxalate = hq.IsLowOxalate;
                protocol.IsLowLectin = hq.IsLowLectin;
                protocol.IsLowPhytate = hq.IsLowPhytate;
                protocol.IsLowHistamine = hq.IsLowHistamine;
                protocol.IsLowMycotoxin = hq.IsLowMycotoxin;
                protocol.IsLowOmega6 = hq.IsLowOmega6;
                protocol.IsBulletProof = hq.IsBulletProof;
                protocol.IsSattvic = hq.IsSattvic;
                protocol.IsLowSulphur = hq.IsLowSulphur;
                protocol.IsKeto = hq.IsKeto;
                protocol.CurrentHealthLevel = hq.CurrentHealthLevel;
                protocol.AutomaticallyFilterFoods = hq.AutomaticallyFilterFoods;
                protocol.Vikruti = hq.GetVikrutiDoshas();
                protocol.Prakruti = hq.GetPrakrutiDoshas(hq);
                protocol.NineStarKi = hq.GetNineStarKiModel();
            }

            protocol.Activities = _protocolActivitiesRepository.Find(e => e.ProtocolId == protocol.Id).ToList();
            foreach (var protocolActivity in protocol.Activities)
            {
                protocolActivity.Activity = GetActivities().FirstOrDefault(e => e.Id == protocolActivity.ActivityId);
            }

            protocol.DietaryRecommendations = _protocolDietaryRecommendationRepository
                .Find(e => e.ProtocolId == protocol.Id).ToList();
            foreach (var protocolDietaryRecommendation in protocol.DietaryRecommendations)
            {
                protocolDietaryRecommendation.DietaryRecommendation =
                    GetDietaryRecommendations().FirstOrDefault(e => e.Id == protocolDietaryRecommendation.DietaryRecommendationId);
            }

            var clientForbiddenFoods = protocol.ClientId.HasValue
                ? _clientForbiddenFoodsRepository.Find(e => e.ClientId == protocol.ClientId.Value).ToList()
                : null;

            protocol.RecommendedFoods = _protocolFoodItemsRepository
                .Find(e => e.ProtocolId == protocol.Id).ToList();
            foreach (var protocolFoodItem in protocol.RecommendedFoods)
            {
                protocolFoodItem.FoodItem =
                    GetFoodItems().FirstOrDefault(e => e.Id == protocolFoodItem.FoodItemId);

                // Demote items marked by client as unsuitable
                if (protocol.GenoType != EGenoType.Unspecified)
                {
                    if (clientForbiddenFoods != null)
                    {
                        var forbidden =
                            clientForbiddenFoods.FirstOrDefault(e =>
                                e.FoodItemId == protocolFoodItem.FoodItem.FoodItemId);

                        var compatibilityLevel = GetExistingCompatibility(protocol.GenoType, protocolFoodItem.FoodItem);

                        if (forbidden != null)
                        {
                            if (forbidden.IsPromotion)
                            {
                                protocolFoodItem.FoodItem.IsPromoted = true;
                            }
                            else
                            {
                                protocolFoodItem.FoodItem.IsDemoted = true;
                            }

                            compatibilityLevel = forbidden.IsPromotion
                               ? ECompatibilityLevel.Neutral
                               : ECompatibilityLevel.Unsuitable;
                        }
                        else
                        {
                            if (protocolFoodItem.FoodItem.IsRedMeat && !protocol.EatsRedMeat)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsPoultry && !protocol.EatsPoultry)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsFishAndSeafood && !protocol.EatsFishAndSeafood)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsEggsAndRoes && !protocol.EatsEggsAndRoes)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsDairy && !protocol.EatsDairy)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsVegetables && !protocol.EatsVegetables)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsVegetableProtein && !protocol.EatsVegetableProtein)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsFungi && !protocol.EatsFungi)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsFruit && !protocol.EatsFruit)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsGrains && !protocol.EatsGrains)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }

                            else if (protocolFoodItem.FoodItem.IsHighOxalate && protocol.IsLowOxalate)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsHighLectin && protocol.IsLowLectin)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsHighPhytate && protocol.IsLowPhytate)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsHighHistamine && protocol.IsLowHistamine)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsHighMycotoxin && protocol.IsLowMycotoxin)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsHighOmega6 && protocol.IsLowOmega6)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (protocolFoodItem.FoodItem.IsHighSulphur && protocol.IsLowSulphur)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (!protocolFoodItem.FoodItem.IsKeto && protocol.IsKeto)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }
                            else if (!protocolFoodItem.FoodItem.IsHighGoitrogen && protocol.IsLowGoitrogen)
                            {
                                compatibilityLevel = ECompatibilityLevel.Unsuitable;
                            }

                            if (protocolFoodItem.FoodItem.IsBulletProof && protocol.IsBulletProof)
                            {
                                if (compatibilityLevel == ECompatibilityLevel.Excellent || compatibilityLevel == ECompatibilityLevel.Optimal)
                                {
                                    compatibilityLevel = ECompatibilityLevel.Optimal;
                                    protocolFoodItem.FoodItem.RelativeScore = 99;
                                }
                                else if (compatibilityLevel == ECompatibilityLevel.Neutral)
                                {
                                    compatibilityLevel = ECompatibilityLevel.Excellent;
                                    protocolFoodItem.FoodItem.RelativeScore = 77;
                                };
                            }

                            if (protocolFoodItem.FoodItem.IsSattvic && protocol.IsSattvic)
                            {
                                if (compatibilityLevel == ECompatibilityLevel.Excellent || compatibilityLevel == ECompatibilityLevel.Optimal)
                                {
                                    compatibilityLevel = ECompatibilityLevel.Optimal;
                                    protocolFoodItem.FoodItem.RelativeScore = 99;
                                }
                                else if (compatibilityLevel == ECompatibilityLevel.Neutral)
                                {
                                    compatibilityLevel = ECompatibilityLevel.Excellent;
                                    protocolFoodItem.FoodItem.RelativeScore = 77;
                                };
                            }
                        }

                        switch (protocol.GenoType)
                        {
                            case EGenoType.Gatherer:
                                protocolFoodItem.FoodItem.GathererCompatibilityLevel = GetCompatibilityLevel(protocolFoodItem.FoodItem, compatibilityLevel);
                                break;

                            case EGenoType.Hunter:
                                protocolFoodItem.FoodItem.HunterCompatibilityLevel = GetCompatibilityLevel(protocolFoodItem.FoodItem, compatibilityLevel);
                                break;

                            case EGenoType.Teacher:
                                protocolFoodItem.FoodItem.TeacherCompatibilityLevel = GetCompatibilityLevel(protocolFoodItem.FoodItem, compatibilityLevel);
                                break;

                            case EGenoType.Explorer:
                                protocolFoodItem.FoodItem.ExplorerCompatibilityLevel = GetCompatibilityLevel(protocolFoodItem.FoodItem, compatibilityLevel);
                                break;

                            case EGenoType.Warrior:
                                protocolFoodItem.FoodItem.WarriorCompatibilityLevel = GetCompatibilityLevel(protocolFoodItem.FoodItem, compatibilityLevel);
                                break;

                            case EGenoType.Nomad:
                                protocolFoodItem.FoodItem.NomadCompatibilityLevel = GetCompatibilityLevel(protocolFoodItem.FoodItem, compatibilityLevel);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            protocol.Products = _protocolProductsRepository.Find(e => e.ProtocolId == protocol.Id).ToList();
            foreach (var protocolProduct in protocol.Products)
            {
                protocolProduct.Product = GetProducts().FirstOrDefault(e => e.Id == protocolProduct.ProductId);
            }

            protocol.ProductPacks = _protocolProductPackRepository.Find(e => e.ProtocolId == protocol.Id).ToList();
            foreach (var protocolProductPack in protocol.ProductPacks)
            {
                protocolProductPack.ProductPack =
                    GetProductPacks().FirstOrDefault(e => e.Id == protocolProductPack.ProductPackId);

                protocolProductPack.ProductPack.Products =
                                _productPackProductRepository.Find(e => e.ProductPackId == protocolProductPack.ProductPack.Id)
                                    .ToList();

                foreach (var productPackProduct in protocolProductPack.ProductPack.Products)
                {
                    productPackProduct.Product =
                        GetProducts().FirstOrDefault(e => e.Id == productPackProduct.ProductId);
                }
            }

            protocol.ProtocolSections = _protocolProtocolSectionRepository.Find(e => e.ProtocolId == protocol.Id);

            foreach (var section in protocol.ProtocolSections)
            {
                section.Section = _protocolSectionRepository.Find(section.SectionId);

                section.ProtocolSectionProducts =
                    _protocolProtocolSectionProductsRepository.Find(e => e.ProtocolSectionId == section.Id)
                        .ToList();

                foreach (var protocolProtocolSectionProduct in section.ProtocolSectionProducts)
                {
                    protocolProtocolSectionProduct.Product =
                        GetProducts().FirstOrDefault(e => e.Id == protocolProtocolSectionProduct.ProductId);
                    protocolProtocolSectionProduct.FormattedAmount =
                                            protocolProtocolSectionProduct.GetFormattedAmount();
                }
            }

            protocol.ProtocolSections = protocol.ProtocolSections.OrderBy(e => e.Section.DisplayOrder).ToList();

            protocol.Client = _clientsRepository.Find(protocol.ClientId ?? 0);
            protocol.ClientName = protocol.Client?.FullName;

            UpdateProtocolProductsAndProductPackQuantities(protocol);

            return protocol;
        }

        public Protocol GetProtocolWithProtocolSections(Guid id)
        {
            var protocol = _protocolsRepository.Find(e => e.ExternalId == id).FirstOrDefault();
            return GetProtocolWithProtocolSections(protocol?.Id ?? 0);
        }

        public Protocol GetProtocolWithProtocolSections(int id)
        {
            var protocol = _protocolsRepository.Find(id);
            if (protocol != null)
            {
                protocol = GetFullProtocol(protocol);
            }

            foreach (var protocolProtocolSection in protocol.ProtocolSections)
            {
                foreach (var protocolProduct in protocol.Products)
                {
                    var existing = protocolProtocolSection.ProtocolSectionProducts.FirstOrDefault(e =>
                        e.ProductId == protocolProduct.ProductId);

                    if (existing != null)
                    {
                        existing.IsVisible =
                            protocolProduct.Product.CheckRecommendations(
                                protocolProtocolSection.Section.Recommendations);
                    }
                    else
                    {
                        var protocolSectionProduct = new ProtocolSectionProduct
                        {
                            ProtocolSectionId = protocolProtocolSection.Id,
                            ProductId = protocolProduct.Product.Id,
                            Product = protocolProduct.Product,
                            IsVisible = protocolProduct.Product.CheckRecommendations(protocolProtocolSection.Section
                                .Recommendations)
                        };
                        protocolProtocolSection.ProtocolSectionProducts.Add(protocolSectionProduct);
                    }
                }

                foreach (var protocolProductPack in protocol.ProductPacks)
                {
                    foreach (var productPackProduct in protocolProductPack.ProductPack.Products)
                    {
                        var existing = protocolProtocolSection.ProtocolSectionProducts.FirstOrDefault(e =>
                            e.ProductId == productPackProduct.ProductId);

                        if (existing != null)
                        {
                            existing.IsVisible =
                                productPackProduct.Product.CheckRecommendations(
                                    protocolProtocolSection.Section.Recommendations);
                        }
                        else
                        {
                            var protocolSectionProduct = new ProtocolSectionProduct
                            {
                                ProtocolSectionId = protocolProtocolSection.Id,
                                ProductId = productPackProduct.ProductId,
                                Product = productPackProduct.Product,
                                IsVisible = productPackProduct.Product.CheckRecommendations(protocolProtocolSection
                                    .Section.Recommendations)
                            };
                            protocolProtocolSection.ProtocolSectionProducts.Add(protocolSectionProduct);
                        }
                    }
                }
            }

            return protocol;
        }

        public List<Protocol> List(bool retrieveFullProtocol = false)
        {
            return MemoryCacheHelper.Cache.GetOrCreate(GetCacheKey(retrieveFullProtocol), entry =>
            {
                entry.SetOptions(GetMemoryCacheEntryOptions(SharedLibrary.Constants.OutputCacheConstants.TenMinutes));

                var protocols = _protocolsRepository.Find(e => !e.IsDeleted).OrderBy(e => e.Name).ToList();

                if (retrieveFullProtocol)
                {
                    var fullProtocols = new List<Protocol>();
                    foreach (var protocol in protocols)
                    {
                        fullProtocols.Add(GetFullProtocol(protocol));
                    }

                    return fullProtocols;
                }

                return protocols;
            });
        }

        public Protocol Duplicate(int id)
        {
            var protocol = _protocolsRepository.Find(id);
            if (protocol != null)
            {
                protocol = GetFullProtocolNoCache(protocol);
            }

            var newProtocolExternalId = Guid.NewGuid();

            var newProtocol = new Protocol
            {
                Name = $"{protocol.Name} (Copy)",
                ShortDescription = protocol.ShortDescription,
                Body = protocol.Body,
                ClientId = protocol.ClientId,
                ExternalId = newProtocolExternalId
            };

            _protocolsRepository.Create(newProtocol);

            // Get Id
            newProtocol = Find(newProtocolExternalId);

            // Copy products
            foreach (var protocolProduct in protocol.Products)
            {
                var newProduct = new ProtocolProduct
                {
                    ProtocolId = newProtocol.Id,
                    ProductId = protocolProduct.ProductId
                };

                _protocolProductsRepository.Create(newProduct);
            }

            // Copy product packs
            foreach (var protocolProductPack in protocol.ProductPacks)
            {
                var newProductPack = new ProtocolProductPack
                {
                    ProtocolId = newProtocol.Id,
                    ProductPackId = protocolProductPack.ProductPackId
                };

                _protocolProductPackRepository.Create(newProductPack);
            }

            // Copy sections
            foreach (var section in protocol.ProtocolSections)
            {
                var newProtocolSection = new ProtocolSection
                {
                    ProtocolId = newProtocol.Id,
                    SectionId = section.SectionId
                };

                _protocolProtocolSectionRepository.Create(newProtocolSection);

                // Copy products and product packs
                var protocolSection =
                    _protocolProtocolSectionRepository.Find(e => e.SectionId == section.SectionId).FirstOrDefault();

                var sectionProducts = _protocolProtocolSectionProductsRepository.Find(e =>
                    e.ProtocolSectionId == protocolSection.SectionId).ToList();

                foreach (var product in sectionProducts)
                {
                    // ProtocolId = newProtocol.Id,
                    var newProtocolSectionProduct = new ProtocolSectionProduct
                    {
                        ProtocolSectionId = protocolSection.SectionId,
                        ProductId = product.ProductId
                    };

                    _protocolProtocolSectionProductsRepository.Create(newProtocolSectionProduct);
                }
            }

            return newProtocol;
        }

        public void DeleteChildRecords(int id)
        {
            var protocol = Find(id);

            foreach (var product in protocol.Products)
            {
                _protocolProductsRepository.Delete(product.Id);
            }

            foreach (var pack in protocol.ProductPacks)
            {
                _protocolProductPackRepository.Delete(pack.Id);
            }

            foreach (var section in protocol.ProtocolSections)
            {
                foreach (var sectionProtocolSectionProduct in section.ProtocolSectionProducts)
                {
                    _protocolProtocolSectionProductsRepository.Delete(sectionProtocolSectionProduct.Id);
                }

                _protocolProtocolSectionRepository.Delete(section.Id);
            }
        }

        public void AddDefaultSections(int id)
        {
            var protocol = Find(id);
            AddDefaultSections(protocol);
        }

        public void UpdateProtocolProductsAndProductPackQuantities(Protocol protocol)
        {
            if (protocol.Products != null && protocol.Products.Any())
            {
                foreach (var protocolProduct in protocol.Products)
                {
                    var productSectionProducts = protocol.ProtocolSections
                        .SelectMany(s => s.ProtocolSectionProducts)
                        .Where(e => e.ProductId == protocolProduct.ProductId).ToList();

                    var numberOfProductDosesRequiredPerDay = productSectionProducts.Sum(e => e.Amount);
                    var numberPerDuration =
                        protocol.GetNumberOfDaysOnPerDuration() * numberOfProductDosesRequiredPerDay;

                    if (numberPerDuration > 0)
                    {
                        protocolProduct.AmountRequired = protocolProduct.Product.Amount > 0
                            ? (int)Math.Ceiling(numberPerDuration / (decimal)protocolProduct.Product.Amount)
                            : 0;
                        protocolProduct.Amount = protocolProduct.AmountRequired;
                    }
                }
            }

            if (protocol.ProductPacks != null && protocol.ProductPacks.Any())
            {
                foreach (var protocolProductPack in protocol.ProductPacks)
                {
                    foreach (var productPackProduct in protocolProductPack.ProductPack.Products)
                    {
                        var productSectionProducts = protocol.ProtocolSections
                            .SelectMany(s => s.ProtocolSectionProducts)
                            .Where(e => e.ProductId == productPackProduct.ProductId).ToList();

                        var numberOfProductDosesRequiredPerDay = productSectionProducts.Sum(e => e.Amount);
                        var numberPerDuration =
                            protocol.GetNumberOfDaysOnPerDuration() * numberOfProductDosesRequiredPerDay;

                        if (numberPerDuration > 0)
                        {
                            productPackProduct.AmountRequired =
                                (int)Math.Ceiling(numberPerDuration /
                                                   (decimal)productPackProduct.Product.Amount);
                        }
                    }
                }
            }
        }

        public void UpdateSectionDetails(Protocol protocol)
        {
            foreach (var protocolProtocolSection in protocol.ProtocolSections)
            {
                if (protocolProtocolSection.ProtocolSectionProducts != null && protocolProtocolSection.ProtocolSectionProducts.Any())
                {
                    foreach (var protocolProduct in protocolProtocolSection.ProtocolSectionProducts)
                    {
                        var items = _protocolProtocolSectionProductsRepository.Find(e =>
                            e.ProtocolSectionId == protocolProtocolSection.Id && e.ProductId == protocolProduct.Product.Id);
                        var item = items.FirstOrDefault();
                        var product = _productsRepository.Find(protocolProduct.Product.Id);

                        if (protocolProduct.Amount == 0 && !items.Any())
                        {
                            continue;
                        }

                        if (protocolProduct.Amount > 0 && !items.Any())
                        {
                            // Create new
                            var newItem = new ProtocolSectionProduct
                            {
                                ProtocolSectionId = protocolProtocolSection.Id,
                                ProductId = protocolProduct.Product.Id,
                                Amount = protocolProduct.Amount
                            };

                            product = _productsRepository.Find(protocolProduct.Product.Id);

                            var sectionMessage =
                                $"ProductName: {product.Name}, {nameof(ProtocolSection.SectionName)}: {protocolProtocolSection.Section.Name}";
                            var acceptableMessage = string.Format(Globalisation.Dictionary.AcceptableValuesMessage,
                                product.MinDosage, product.MaxDosage);

                            if (newItem.Amount > product.MaxDosage)
                            {
                                throw new ArgumentOutOfRangeException("Amount",
                                    $"{Globalisation.Dictionary.ValueTooHighException} {acceptableMessage} {sectionMessage}");
                                ;
                            }

                            if (newItem.Amount < product.MinDosage)
                            {
                                throw new ArgumentOutOfRangeException("Amount",
                                    $"{Globalisation.Dictionary.ValueTooLowException} {acceptableMessage} {sectionMessage}");
                                ;
                            }

                            _protocolProtocolSectionProductsRepository.Create(newItem);
                        }
                        else if (items.Any() && protocolProduct.Amount == 0)
                        {
                            // Remove item
                            _protocolProtocolSectionProductsRepository.Delete(item.Id);
                        }
                        else if (items.Any() && item.Amount != protocolProduct.Amount)
                        {
                            // Change amount
                            item.Amount = protocolProduct.Amount;
                            _protocolProtocolSectionProductsRepository.Update(item);
                        }
                    }
                }
            }
        }

        public static bool CheckSchedule(Protocol protocol, DayOfWeek dayofWeek)
        {
            if (protocol.ProtocolFrequency == EProtocolFrequency.Daily)
            {
                if (protocol.NumberOfPeriodsOff >= 7)
                {
                    return false;
                }

                switch (dayofWeek)
                {
                    case DayOfWeek.Monday:
                        return true;

                    case DayOfWeek.Tuesday:
                        return protocol.NumberOfPeriodsOff <= 2;

                    case DayOfWeek.Wednesday:
                        return new[] { 4, 3, 1, 0 }.Contains(protocol.NumberOfPeriodsOff);

                    case DayOfWeek.Thursday:
                        return new[] { 5, 2, 1, 0 }.Contains(protocol.NumberOfPeriodsOff);

                    case DayOfWeek.Friday:
                        return protocol.NumberOfPeriodsOff <= 4;

                    case DayOfWeek.Saturday:
                        return protocol.NumberOfPeriodsOff <= 3;

                    case DayOfWeek.Sunday:
                        return protocol.NumberOfPeriodsOff == 0;
                }
            }

            return false;
        }

        public static int[] GetSchedule(Protocol protocol)
        {
            if (protocol.ProtocolFrequency == EProtocolFrequency.Monthly)
            {
                var intervalLength = protocol.GetPeriodLength();
                var interval = (int)Math.Floor((double)intervalLength / protocol.NumberOfPeriodsOn);
                var list = new List<int>();

                for (int i = 1; i <= intervalLength; i += interval)
                {
                    list.Add(i);
                }

                return list.ToArray();
            }

            return null;
        }

        public void CheckProductsAndProductPacksDoNotOverlap(Protocol protocol)
        {
            if (protocol.ProtocolProductPacks != null && protocol.ProtocolProductPacks.Any() &&
                protocol.ProtocolProducts != null && protocol.ProtocolProducts.Any())
            {
                // Check that the products in the product packs don't duplicate the products and raise an error if so!
                if (protocol.ProtocolProductPacks.SelectMany(e => e.ProductPack.Products).Select(e => e.ProductId)
                    .Any(pid => protocol.ProtocolProducts.Select(pp => pp.ProductId).Contains(pid)))
                {
                    throw new Exception(Globalisation.Dictionary.ProtocolProductsAndProductPacksOverlap);
                }
            }
        }

        private void AddDefaultSections(Protocol protocol)
        {
            if (!protocol.ProtocolSections.Any())
            {
                var defaultSections = _protocolSectionRepository.List();

                // Copy default sections
                foreach (var section in defaultSections)
                {
                    var newProtocolSection = new ProtocolSection
                    {
                        ProtocolId = protocol.Id,
                        SectionId = section.Id
                    };

                    _protocolProtocolSectionRepository.Create(newProtocolSection);
                }
            }
        }

        private ECompatibilityLevel GetExistingCompatibility(EGenoType genoType, FoodItem foodItem)
        {
            switch (genoType)
            {
                case EGenoType.Gatherer:
                    return foodItem.GathererCompatibilityLevel;

                case EGenoType.Hunter:
                    return foodItem.HunterCompatibilityLevel;

                case EGenoType.Teacher:
                    return foodItem.TeacherCompatibilityLevel;

                case EGenoType.Explorer:
                    return foodItem.ExplorerCompatibilityLevel;

                case EGenoType.Warrior:
                    return foodItem.WarriorCompatibilityLevel;

                case EGenoType.Nomad:
                    return foodItem.NomadCompatibilityLevel;

                default:
                    return ECompatibilityLevel.Unspecified;
            }
        }

        private static ECompatibilityLevel GetCompatibilityLevel(FoodItem recommendedFood, ECompatibilityLevel compatibilityLevel)
        {
            var score = recommendedFood.GetScore();

            // Upgrade highest-scoring items
            if (score == EScore.VeryHigh
                && compatibilityLevel != ECompatibilityLevel.Unsuitable
                && compatibilityLevel != ECompatibilityLevel.Suboptimal)
            {
                return ECompatibilityLevel.Optimal;
            }

            else if (score == EScore.High
                     && compatibilityLevel == ECompatibilityLevel.Neutral)
            {
                return ECompatibilityLevel.Excellent;
            }

            // Remove "hearts"
            else if (score >= EScore.High && (compatibilityLevel == ECompatibilityLevel.Suboptimal ||
                                              compatibilityLevel == ECompatibilityLevel.Unsuitable || compatibilityLevel == ECompatibilityLevel.Neutral))

            {
                recommendedFood.RelativeScore = 0;
            }

            // Downgrade lowest scoring items
            else if (score <= EScore.Negative)
            {
                if (compatibilityLevel == ECompatibilityLevel.Optimal)
                {
                    return ECompatibilityLevel.Excellent;
                }

                else if (compatibilityLevel == ECompatibilityLevel.Excellent)
                {
                    return ECompatibilityLevel.Neutral;
                }

                else if (compatibilityLevel == ECompatibilityLevel.Neutral)
                {
                    return ECompatibilityLevel.Suboptimal;
                }

                else if (compatibilityLevel == ECompatibilityLevel.Suboptimal)
                {
                    return ECompatibilityLevel.Unsuitable;
                }
            }

            return compatibilityLevel;
        }

    }
}