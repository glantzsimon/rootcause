using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using K9.WebApplication.ViewModels;
using System;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public class ProtocolsController : BaseRootController<Protocol>
    {
        private readonly IProtocolService _protocolService;

        public ProtocolsController(IControllerPackage<Protocol> controllerPackage, IProtocolService protocolService, IControllerPackage pureControllerPackage) : base(controllerPackage, pureControllerPackage)
        {
            _protocolService = protocolService;
            RecordCreated += ProtocolsController_RecordCreated;
            RecordDeleted += ProtocolsController_RecordDeleted;
            RecordUpdated += ProtocolsController_RecordUpdated;
            RecordBeforeCreated += ProtocolsController_RecordBeforeCreated;
            RecordBeforeDetails += ProtocolsController_RecordBeforeDetails;
            RecordBeforeUpdate += ProtocolsController_RecordBeforeUpdate;
            RecordBeforeDelete += ProtocolsController_RecordBeforeDelete;
            RecordBeforeDeleted += ProtocolsController_RecordBeforeDeleted;
        }
        
        public ActionResult View(int protocolId)
        {
            return RedirectToAction("Details", null, new { id = protocolId });
        }

        [RequirePermissions(Permission = Permissions.Edit)]
        public ActionResult DuplicateProtocol(int id)
        {
            return View(Repository.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Permission = Permissions.Create)]
        public ActionResult DuplicateProtocol(Protocol protocol)
        {
            var duplicate = _protocolService.Duplicate(protocol.Id);
            _protocolService.ClearCache();
            return RedirectToAction("Edit", new { id = duplicate.Id });
        }

        public ActionResult EditDietaryRecommendations(int id = 0)
        {
            return RedirectToAction("EditRecommendationsForProtocol", "ProtocolDietaryRecommendations", new { id });
        }

        public ActionResult EditActivities(int id = 0)
        {
            return RedirectToAction("EditActivitiesForProtocol", "ProtocolActivities", new { id });
        }

        public ActionResult EditSections(int id = 0)
        {
            return RedirectToAction("EditProtocolProtocolSectionsForProtocol", "ProtocolSections", new { id });
        }

        public ActionResult EditProducts(int id = 0)
        {
            return RedirectToAction("EditProductsForProtocol", "ProtocolProducts", new { id });
        }

        public ActionResult EditProductPacks(int id = 0)
        {
            return RedirectToAction("EditProductPacksForProtocol", "ProtocolProductPacks", new { id });
        }

        public ActionResult Summary(int id, int? protocolId)
        {
            var protocol = _protocolService.GetProtocolWithProtocolSections(protocolId ?? id);
            return View(protocol);
        }
        
        public ActionResult ReviewSectionDetails(int id = 0)
        {
            var protocol = _protocolService.GetProtocolWithProtocolSections(id);
            return View(new ProtocolReviewViewModel
            {
                Protocol = protocol
            });
        }

        public ActionResult EditSectionDetails(int id = 0)
        {
            var protocol = _protocolService.GetProtocolWithProtocolSections(id);
            return View(protocol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Permission = Permissions.Edit)]
        public ActionResult EditSectionDetails(Protocol model)
        {
            try
            {
                _protocolService.UpdateSectionDetails(model);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            _protocolService.ClearCache();
            return RedirectToAction("ReviewSectionDetails", new { id = model.Id });
        }

        private void ProtocolsController_RecordBeforeDeleted(object sender, CrudEventArgs e)
        {
            _protocolService.DeleteChildRecords(e.Item.Id);
        }

        private void ProtocolsController_RecordBeforeDelete(object sender, CrudEventArgs e)
        {
            var protocol = e.Item as Protocol;
            _protocolService.GetFullProtocol(protocol);
        }

        private void ProtocolsController_RecordBeforeUpdate(object sender, CrudEventArgs e)
        {
            var protocol = e.Item as Protocol;
            protocol = _protocolService.GetFullProtocol(protocol);
            _protocolService.CheckProductsAndProductPacksDoNotOverlap(protocol);
        }

        private void ProtocolsController_RecordBeforeDetails(object sender, CrudEventArgs e)
        {
            var protocol = e.Item as Protocol;
            _protocolService.GetFullProtocol(protocol);
            _protocolService.CheckProductsAndProductPacksDoNotOverlap(protocol);
        }

        private void ProtocolsController_RecordBeforeCreated(object sender, CrudEventArgs e)
        {
            var protocol = e.Item as Protocol;
            protocol.ExternalId = Guid.NewGuid();
        }

        private void ProtocolsController_RecordCreated(object sender, CrudEventArgs e)
        {
            _protocolService.AddDefaultSections(e.Item.Id);
            _protocolService.ClearCache();
        }

        private void ProtocolsController_RecordDeleted(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }

        private void ProtocolsController_RecordUpdated(object sender, CrudEventArgs e)
        {
            _protocolService.ClearCache();
        }
    }
}
