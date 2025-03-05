using K9.Base.WebApplication.Filters;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public class SharedController : BaseRootController
    {
        private readonly IAuthentication _authentication;
        private readonly IProductService _productService;
        private readonly IIngredientService _ingredientService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IHealthQuestionnaireService _healthQuestionnaireService;
        private readonly IRepository<FoodItem> _foodItemsRepository;

        public SharedController(IMembershipService membershipService, IProductService productService, IIngredientService ingredientService, IMaintenanceService maintenanceService, IHealthQuestionnaireService healthQuestionnaireService, IRepository<FoodItem> foodItemsRepository, IServicePackage servicePackage)
            : base(servicePackage)
        {
            _productService = productService;
            _ingredientService = ingredientService;
            _maintenanceService = maintenanceService;
            _healthQuestionnaireService = healthQuestionnaireService;
            _foodItemsRepository = foodItemsRepository;
        }

        public override string GetObjectName()
        {
            return string.Empty;
        }
        
        [Authorize]
        [RequirePermissions(Role = RoleNames.Administrators)]
        public ActionResult RunMaintenanceScript()
        {
            //_productService.UpdateProductCategories();
            //_maintenanceService.AddFoodItemsAndActivities();

            return RedirectToAction("MaintenanceComplete");
        }

        public ActionResult MaintenanceComplete()
        {
            return View();
        }
        
    }
}
