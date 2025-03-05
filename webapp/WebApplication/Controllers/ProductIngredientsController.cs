using K9.Base.WebApplication.Controllers;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.Base.WebApplication.ViewModels;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Models;
using K9.WebApplication.Services;
using System.Web.Mvc;
using K9.Base.WebApplication.Extensions;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public partial class ProductIngredientsController : BaseController<ProductIngredient>
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IProductService _productService;

        public ProductIngredientsController(IControllerPackage<ProductIngredient> controllerPackage, IRepository<Product> productsRepository, IProductService productService)
            : base(controllerPackage)
        {
            _productsRepository = productsRepository;
            _productService = productService;

            RecordEditMultipleUpdated += ProductIngredientsController_RecordEditMultipleUpdated;
        }

        public override ActionResult Index()
        {
            return RedirectToAction("Index", "Products");
        }

        [RequirePermissions(Permission = Permissions.Edit)]
        public ActionResult EditIngredientsForProduct(int id = 0)
        {
            return EditMultiple<Product, Ingredient>(_productsRepository.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Permission = Permissions.Edit)]
        public ActionResult EditIngredientsForProduct(MultiSelectViewModel model)
        {
            _productService.ClearCache();
            return EditMultiple<Product, Ingredient>(model);
        }

        /// <summary>
        ///  Not firing - not sure why
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductIngredientsController_RecordEditMultipleUpdated(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            e.IsRedirect = true;
            e.Controller = typeof(ProductsController).GetControllerName();
            e.Action = nameof(ProductsController.EditIngredientQuantities);
            e.RouteValues = new { id = e.Item.Id };

            _productService.ClearCache();
        }
    }
}
