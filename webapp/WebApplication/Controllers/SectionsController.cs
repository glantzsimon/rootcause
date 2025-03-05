using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public class SectionsController : BaseRootController<Section>
    {
        public SectionsController(IControllerPackage<Section> controllerPackage, IServicePackage servicePackage) : base(controllerPackage, servicePackage)
        {
            RecordBeforeCreate += ProtocolSectionsController_RecordBeforeCreate;
        }

        public ActionResult View(int protocolSectionId)
        {
            return RedirectToAction("Details", null, new { id = protocolSectionId });
        }

        private void ProtocolSectionsController_RecordBeforeCreate(object sender, CrudEventArgs e)
        {
            var protocolSection = e.Item as Section;
            UpdateDisplayOrder(protocolSection);
        }

        private void UpdateDisplayOrder(Section section)
        {
            var lastSection = Repository.CustomQuery<Section>(
                $"SELECT TOP 1 * FROM {nameof(Section)} ORDER BY {nameof(Section.DisplayOrder)} DESC").FirstOrDefault();
            section.DisplayOrder = lastSection?.DisplayOrder + 1 ?? 0;
        }
    }
}
