using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [RequirePermissions(Role = RoleNames.Administrators)]
    [Authorize]
    [RoutePrefix("email-queue")]
    public class EmailQueueController : BaseRootController<EmailQueueItem>
    {
        private readonly IMailingListService _mailingListService;
        private readonly IMailerService _mailerService;

        public EmailQueueController(IControllerPackage<EmailQueueItem> controllerPackage, IServicePackage servicePackage, IMailingListService mailingListService, IMailerService mailerService)
            : base(controllerPackage, servicePackage)
        {
            _mailingListService = mailingListService;
            _mailerService = mailerService;
        }
    }
}