using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using System;
using System.Web.Mvc;
using K9.WebApplication.Services;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public class DietaryRecommendationsController : BaseRootController<DietaryRecommendation>
    {
        private readonly IProtocolService _protocolService;

        public DietaryRecommendationsController(IControllerPackage<DietaryRecommendation> controllerPackage, IControllerPackage pureControllerPackage, IProtocolService protocolService) : base(controllerPackage, pureControllerPackage)
        {
            _protocolService = protocolService;
            RecordBeforeCreated += DietaryRecommendationsController_RecordBeforeCreated;
            RecordBeforeUpdated += DietaryRecommendationsController_RecordBeforeUpdated;
            RecordCreated += DietaryRecommendationsController_RecordCreated;
            RecordUpdated += DietaryRecommendationsController_RecordUpdated;
            RecordDeleted += DietaryRecommendationsController_RecordDeleted;
        }

        private void DietaryRecommendationsController_RecordDeleted(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void DietaryRecommendationsController_RecordUpdated(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void DietaryRecommendationsController_RecordCreated(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void DietaryRecommendationsController_RecordBeforeUpdated(object sender, CrudEventArgs e)
        {
            var item = e.Item as DietaryRecommendation;
        }

        private void DietaryRecommendationsController_RecordBeforeCreated(object sender, CrudEventArgs e)
        {
            var item = e.Item as DietaryRecommendation;
            item.ExternalId = Guid.NewGuid();
        }
    }
}
