using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.WebApplication.Packages;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    public class DonationsController : BaseRootController<Donation>
    {

        public DonationsController(IControllerPackage<Donation> controllerPackage, IServicePackage servicePackage)
            : base(controllerPackage, servicePackage)
        {
        }

    }
}