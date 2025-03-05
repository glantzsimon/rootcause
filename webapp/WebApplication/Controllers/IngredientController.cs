using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public class IngredientController : BaseRootController
    {
        private readonly IRepository<Ingredient> _ingredientsRepository;
        private readonly IIngredientService _ingredientService;

        public IngredientController(IRepository<Ingredient> ingredientsRepository, IIngredientService ingredientService, IServicePackage servicePackage)
            : base(servicePackage)
        {
            _ingredientsRepository = ingredientsRepository;
            _ingredientService = ingredientService;
        }

        [Route("ingredient/all")]
        public ActionResult Index()
        {
            return View(_ingredientsRepository.Find(e => !e.IsHidden).OrderBy(e => e.Name).ToList());
        }

        [Route("ingredients/export/json")]
        public ActionResult GetIngredientsJson()
        {
            return Json(new { success = true, data = _ingredientService.ListIngredientItems() }, JsonRequestBehavior.AllowGet);
        }

        [Route("ingredient/{seoFriendlyId}")]
        public ActionResult Details(string seoFriendlyId)
        {
            var ingredient = _ingredientsRepository.Find(e => e.SeoFriendlyId == seoFriendlyId && !e.IsHidden).FirstOrDefault();
            if (ingredient == null)
            {
                return HttpNotFound();
            }
            LoadUploadedFiles(ingredient);
            return View(ingredient);
        }
        
        public override string GetObjectName()
        {
            return typeof(NewsItem).Name;
        }
    }
}
