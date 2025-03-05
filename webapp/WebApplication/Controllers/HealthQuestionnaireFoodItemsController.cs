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
	public class HealthQuestionnaireFoodItemsController : BaseController<HealthQuestionnaireFoodItem>
	{
	    private readonly IRepository<HealthQuestionnaire> _healthQuestionnaiRepository;

	    public HealthQuestionnaireFoodItemsController(IControllerPackage<HealthQuestionnaireFoodItem> controllerPackage, IRepository<HealthQuestionnaire> healthQuestionnaiRepository)
			: base(controllerPackage)
	    {
	        _healthQuestionnaiRepository = healthQuestionnaiRepository;
	    }

	    public override ActionResult Index()
	    {
	        return RedirectToAction("Index", "HealthQuestionnaire");
	    }
        
	    [RequirePermissions(Permission = Permissions.Edit)]
	    public ActionResult EditFoodItemsForHealthQuestionnaire(int id = 0)
	    {
	        return EditMultiple<HealthQuestionnaire, FoodItem>(_healthQuestionnaiRepository.Find(id));
	    }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    [RequirePermissions(Permission = Permissions.Edit)]
	    public ActionResult EditFoodItemsForHealthQuestionnaire(MultiSelectViewModel model)
	    {
	        return EditMultiple<HealthQuestionnaire, FoodItem>(model);
	    }
	}
}
