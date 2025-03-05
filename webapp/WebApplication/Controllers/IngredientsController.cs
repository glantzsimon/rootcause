using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Extensions;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public partial class IngredientsController : BaseRootController<Ingredient>
    {
        private readonly IIngredientService _ingredientService;
        private readonly IProductService _productService;

        public IngredientsController(IControllerPackage<Ingredient> controllerPackage, IIngredientService ingredientService, IProductService productService, IServicePackage servicePackage) : base(controllerPackage, servicePackage)
        {
            _ingredientService = ingredientService;
            _productService = productService;
            RecordBeforeCreated += IngredientsController_RecordBeforeCreated;
            RecordBeforeUpdated += IngredientsController_RecordBeforeUpdated;
        }
        
        public ActionResult EditList()
        {
            return View(Repository.List().OrderBy(e => e.Name).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Permission = Permissions.Edit)]
        public ActionResult EditList(List<Ingredient> model)
        {
            foreach (var ingredient in model)
            {
                var item = Repository.Find(ingredient.Id);
                item.Name = ingredient.Name;
                item.Cost = ingredient.Cost;
                item.Quantity = ingredient.Quantity;
                item.QuantityInStock = ingredient.QuantityInStock;
                item.Concentration = ingredient.Concentration;
                item.RecommendedDailyAllownace = ingredient.RecommendedDailyAllownace;
                item.Category = ingredient.Category;
                item.ItemCode = ingredient.ItemCode;

                Repository.Update(item);
            }

            return RedirectToAction("EditList");
        }
       
        [Route("ingredients/export/csv")]
        public ActionResult DownloadIngredientsCsv()
        {
            var ingredientsItems = _ingredientService.ListIngredientItems();
            var data = ingredientsItems.ToCsv();

            Response.Clear();
            Response.ContentType = "application/CSV";
            Response.AddHeader("content-disposition", $"attachment; filename=\"Ingredients.csv\"");
            Response.Write(data);
            Response.End();

            return new EmptyResult();
        }

        [Route("ingredients/categories/export/csv")]
        public ActionResult DownloadIngredientCategoriesCsv()
        {
            var ingredients = _ingredientService.List();

            var data = ingredients.Select(e => e.CategoryText).Distinct()
                .Select(e => new CategoryItem
                {
                    Name = e.ToUpper()
                }).ToCsv();

            Response.Clear();
            Response.ContentType = "application/CSV";
            Response.AddHeader("content-disposition", $"attachment; filename=\"IngredientCategories.csv\"");
            Response.Write(data);
            Response.End();

            return new EmptyResult();
        }
        
        private void IngredientsController_RecordBeforeUpdated(object sender, CrudEventArgs e)
        {
            var ingredient = e.Item as Ingredient;
            var original = Repository.Find(ingredient.Id);
            var titleHasChanged = original.Name != ingredient.Name;
            if (string.IsNullOrEmpty(ingredient.SeoFriendlyId) || titleHasChanged && original.SeoFriendlyId == original.Name.ToSeoFriendlyString())
            {
                ingredient.SeoFriendlyId = ingredient.Name.ToSeoFriendlyString();
            }
        }

        private void IngredientsController_RecordBeforeCreated(object sender, CrudEventArgs e)
        {
            var ingredient = e.Item as Ingredient;
            if (string.IsNullOrEmpty(ingredient.SeoFriendlyId))
            {
                ingredient.SeoFriendlyId = ingredient.Name.ToSeoFriendlyString();
            }
        }
    }
}
