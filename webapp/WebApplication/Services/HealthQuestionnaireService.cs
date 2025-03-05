using K9.Base.DataAccessLayer.Models;
using K9.Base.WebApplication.Config;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Services
{
    public class HealthQuestionnaireService : HealthQuestionnaireServiceBase, IHealthQuestionnaireService
    {
        private readonly UrlHelper _urlHelper;
        private readonly IMailer _mailer;
        private readonly WebsiteConfiguration _config;
        private readonly IRepository<HealthQuestionnaire> _healthQuestionnaireRepository;
        private readonly IRepository<User> _usersRepository;
        private readonly IRepository<Client> _clientsRepository;
        private readonly IClientService _clientService;
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<Ingredient> _ingredientsRepository;
        private readonly IRepository<ProductIngredient> _productIingredientsRepository;
        private readonly IRepository<Protocol> _protocolsRepository;
        private readonly IRepository<Activity> _activitiesRepository;
        private readonly IRepository<DietaryRecommendation> _dietaryRecommendationsRepository;
        private readonly IRepository<FoodItem> _foodItemsRepository;
        private readonly IRepository<ProtocolActivity> _protocolActivitiesRepository;
        private readonly IRepository<ProtocolDietaryRecommendation> _protocolDietaryRecommendationRepository;
        private readonly IRepository<ProtocolFoodItem> _protocolFoodItemsRepository;
        private readonly IRepository<ProtocolProduct> _protocolProductsRepository;
        private readonly IProtocolService _protocolService;

        public HealthQuestionnaireService(
            IRepository<Product> productsRepository,
            IRepository<HealthQuestionnaire> healthQuestionnaireRepository,
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
            IRepository<ProtocolProduct> protocolProductsRepository,
            IProtocolService protocolService,
            IServiceBasePackage serviceBasePackage) :

            base(productsRepository, healthQuestionnaireRepository, protocolsRepository, activitiesRepository, dietaryRecommendationsRepository,
                foodItemsRepository, ingredientsRepository, productIngredientsRepository, ingredientSubstitutesRepository, protocolActivitiesRepository,
                productIngredientSubstitutesRepository, protocolDietaryRecommendationRepository, protocolFoodItemsRepository, 
                protocolProductsRepository, protocolService, serviceBasePackage)
        {
            _healthQuestionnaireRepository = healthQuestionnaireRepository;
            _productsRepository = productsRepository;
            _protocolsRepository = protocolsRepository;
            _ingredientsRepository = ingredientsRepository;
            _productIingredientsRepository = productIngredientsRepository;
            _activitiesRepository = activitiesRepository;
            _dietaryRecommendationsRepository = dietaryRecommendationsRepository;
            _foodItemsRepository = foodItemsRepository;
            _protocolActivitiesRepository = protocolActivitiesRepository;
            _protocolDietaryRecommendationRepository = protocolDietaryRecommendationRepository;
            _protocolFoodItemsRepository = protocolFoodItemsRepository;
            _protocolProductsRepository = protocolProductsRepository;
            _protocolService = protocolService;
        }

        public HealthQuestionnaire GetHealthQuestionnaireForUser(int userId)
        {
            var user = _usersRepository.Find(userId);
            var client = _clientsRepository.Find(e => e.UserId == userId).FirstOrDefault();

            if (client == null)
            {
                client = _clientsRepository.Find(e => e.EmailAddress == user.EmailAddress).FirstOrDefault();

                if (client == null)
                {
                    client = _clientService.GetOrCreateClient("", user.FullName, user.EmailAddress,
                        user.PhoneNumber,
                        user.Id);
                }
            }

            return GetHealthQuestionnaireForClient(client.Id);
        }

        public HealthQuestionnaire GetHealthQuestionnaireForClient(int clientId)
        {
            var client = _clientsRepository.Find(clientId);
            return GetHealthQuestionnaireForClient(client);
        }

        public HealthQuestionnaire GetHealthQuestionnaireById(Guid externalId)
        {
            return GetHealthQuestionnaire(externalId);
        }

        public List<Protocol> GetGeneticProfileMatchedProtocols(int clientId)
        {
            var hq = GetHealthQuestionnaireForClient(clientId);
            if (hq == null)
            {
                return null;
            }

            var genoType = hq.CalculateGenotype();
            if (genoType == null)
            {
                return null;
            }

            return GetGenoTypeFilteredItems(hq, genoType.GenoType, new List<Protocol>(_protocolsRepository.List()), 3);
        }

        public void Save(HealthQuestionnaire model)
        {
            _healthQuestionnaireRepository.Update(model);
            var client = _clientsRepository.Find(e => e.Id == model.ClientId).First();
            {
                var user = _usersRepository.Find(e => e.Id == client.UserId).FirstOrDefault();
                if (user != null)
                {
                    if (user.BirthDate != model.DateOfBirth && model.DateOfBirth.HasValue)
                    {
                        user.BirthDate = model.DateOfBirth.Value;
                        _usersRepository.Update(user);
                    }
                }
            }
        }

        public void SendHealthQuestionnaireCompleteEmailToAdmin(HealthQuestionnaire hq, Client client)
        {
            var template = Globalisation.Dictionary.ConsultationBookedThankYouEmail;
            var title = Globalisation.Dictionary.HealthQuestionnaireCompleteTitle;
            _mailer.SendEmail(title, TemplateParser.Parse(template, new
            {
                Title = title,
                Client = client.FullName,
                ClientEmail = client.EmailAddress,
                client.PhoneNumber,
                Link = _urlHelper.AbsoluteAction("GeneticProfileTestOverview", "HealthQuestionnaire", new { hq.ExternalId }),
                ImageUrl = _urlHelper.AbsoluteContent(_config.CompanyLogoUrl),
                PrivacyPolicyLink = _urlHelper.AbsoluteAction("PrivacyPolicy", "Home"),
                UnsubscribeLink = _urlHelper.AbsoluteAction("Unsubscribe", "Account", new { code = client.Name }),
                DateTime.Now.Year
            }), client.EmailAddress, client.FullName, _config.SupportEmailAddress, _config.CompanyName);
        }

        public Protocol GetAutoGeneratedProtocolFromGeneticProfile(int clientId)
        {
            var hq = GetHealthQuestionnaireForClient(clientId);
            if (hq != null)
            {
                return GetAutoGeneratedProtocolFromGeneticProfile(hq);
            }

            return null;
        }

        public Protocol GetAutoGeneratedProtocolFromGeneticProfile(HealthQuestionnaire hq)
        {
            var protocol = _protocolsRepository
                .Find(e => e.Type == EProtocolType.AutoGenerated && e.ClientId == hq.ClientId).FirstOrDefault();
            if (protocol != null)
            {
                return _protocolService.Find(protocol.Id);
            }

            return null;
        }

        private HealthQuestionnaire GetHealthQuestionnaireForClient(Client client)
        {
            var hq = _healthQuestionnaireRepository.Find(e => e.ClientId == client.Id).FirstOrDefault();

            if (hq == null)
            {
                var hqId = Guid.NewGuid();

                hq = new HealthQuestionnaire
                {
                    ExternalId = hqId,
                    ClientId = client.Id,
                    Name = $"{client.Name} - {Globalisation.Dictionary.HealthQuestionnaire}"
                };

                _healthQuestionnaireRepository.Create(hq);

                hq = _healthQuestionnaireRepository.Find(e => e.ExternalId == hqId).First();
            }

            hq.Client = client;
            return hq;
        }

        private HealthQuestionnaire GetHealthQuestionnaire(Guid externalId)
        {
            var hq = _healthQuestionnaireRepository.Find(e => e.ExternalId == externalId).FirstOrDefault();

            if (hq != null)
            {
                var client = _clientService.Find(hq.ClientId);
                hq.Client = client;

                return hq;
            }

            return null;
        }

    }
}