using K9.Base.WebApplication.Controllers;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.Base.WebApplication.ViewModels;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Models;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
	[RequirePermissions(Role = RoleNames.Administrators)]
	public class ProtocolDietaryRecommendationsController : BaseController<ProtocolDietaryRecommendation>
	{
	    private readonly IRepository<Protocol> _protocolRepository;

	    public ProtocolDietaryRecommendationsController(IControllerPackage<ProtocolDietaryRecommendation> controllerPackage, IRepository<Protocol> protocolRepository)
			: base(controllerPackage)
	    {
	        _protocolRepository = protocolRepository;
	    }

	    public override ActionResult Index()
	    {
	        return RedirectToAction("Index", "Protocols");
	    }
        
	    [RequirePermissions(Permission = Permissions.Edit)]
	    public ActionResult EditRecommendationsForProtocol(int id = 0)
	    {
	        return EditMultiple<Protocol, DietaryRecommendation>(_protocolRepository.Find(id));
	    }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    [RequirePermissions(Permission = Permissions.Edit)]
	    public ActionResult EditRecommendationsForProtocol(MultiSelectViewModel model)
	    {
	        return EditMultiple<Protocol, DietaryRecommendation>(model);
	    }
	}
}
