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
    public class ActivitiesController : BaseRootController<Activity>
    {
        private readonly IProtocolService _protocolService;

        public ActivitiesController(IControllerPackage<Activity> controllerPackage, IControllerPackage pureControllerPackage, IProtocolService protocolService) : base(controllerPackage, pureControllerPackage)
        {
            _protocolService = protocolService;
            RecordBeforeCreated += ActivitiesController_RecordBeforeCreated;
            RecordBeforeUpdated += ActivitiesController_RecordBeforeUpdated;
            RecordCreated += ActivitiesController_RecordCreated;
            RecordUpdated += ActivitiesController_RecordUpdated;
            RecordDeleted += ActivitiesController_RecordDeleted;
        }

        private void ActivitiesController_RecordDeleted(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void ActivitiesController_RecordUpdated(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void ActivitiesController_RecordCreated(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void ActivitiesController_RecordBeforeUpdated(object sender, CrudEventArgs e)
        {
            var activity = e.Item as Activity;
        }

        private void ActivitiesController_RecordBeforeCreated(object sender, CrudEventArgs e)
        {
            var activity = e.Item as Activity;
            activity.ExternalId = Guid.NewGuid();
        }
    }
}
