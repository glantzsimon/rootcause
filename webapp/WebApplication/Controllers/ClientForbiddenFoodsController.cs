using K9.Base.WebApplication.Controllers;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.Base.WebApplication.ViewModels;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Models;
using K9.WebApplication.Services;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
	public partial class ClientForbiddenFoodsController : BaseController<ClientForbiddenFood>
	{
	    private readonly IRepository<Client> _clientsRepository;
	    private readonly IProtocolService _protocolService;

	    public ClientForbiddenFoodsController(IControllerPackage<ClientForbiddenFood> controllerPackage, IRepository<Client> clientsRepository, IProtocolService protocolService)
			: base(controllerPackage)
	    {
	        _clientsRepository = clientsRepository;
	        _protocolService = protocolService;
	    }

	    public override ActionResult Index()
	    {
	        return RedirectToAction("Index", "Protocols");
	    }
        
	    [RequirePermissions(Permission = Permissions.Edit)]
	    public ActionResult EditForbiddenFoodItemsClient(int id = 0)
	    {
	        return EditMultiple<Client, FoodItem>(_clientsRepository.Find(id));
	    }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    [RequirePermissions(Permission = Permissions.Edit)]
	    public ActionResult EditForbiddenFoodItemsClient(MultiSelectViewModel model)
	    {
	        return EditMultiple<Client, FoodItem>(model);
	    }
	}
}
