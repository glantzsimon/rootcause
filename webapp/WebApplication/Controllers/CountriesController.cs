using K9.Base.DataAccessLayer.Models;
using K9.Base.WebApplication.UnitsOfWork;
using K9.WebApplication.Packages;

namespace K9.WebApplication.Controllers
{
    public class CountriesController : BaseRootController<Country>
	{
		public CountriesController(IControllerPackage<Country> controllerPackage, IServicePackage servicePackage) 
		    : base(controllerPackage, servicePackage)
		{
		}
	}
}
