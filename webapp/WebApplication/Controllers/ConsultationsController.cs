using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public class ConsultationsController : BaseRootController<Consultation>
    {
        public ConsultationsController(IControllerPackage<Consultation> controllerPackage, IServicePackage servicePackage)
            : base(controllerPackage, servicePackage)
        {
            RecordBeforeDetails += ConsultationsController_RecordBeforeDetails;
        }

        private void ConsultationsController_RecordBeforeDetails(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            var consultation = e.Item as Consultation;
            consultation.Client = My.ClientsRepository.Find(consultation.ContactId);
        }
    }
}