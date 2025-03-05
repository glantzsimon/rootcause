using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Models;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public class ProductController : BaseRootController
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IProductService _productService;

        public ProductController(IRepository<Product> productsRepository, IMembershipService membershipService, IProductService productService, IServicePackage servicePackage)
            : base(servicePackage)
        {
            _productsRepository = productsRepository;
            _productService = productService;
        }

        [Route("product/all")]
        public ActionResult Index()
        {
            var products = _productService.List(true);
            foreach (var product in products)
            {
                LoadUploadedFiles(product);
            }
            return View(products);
        }

        [Route("products/export/json")]
        private ActionResult GetProductsJson(List<ProductItem> items)
        {
            return Json(new { success = true, data = items }, JsonRequestBehavior.AllowGet);
        }

        [Route("products/export/json/all")]
        public ActionResult GetProductsJsonAll()
        {
            return GetProductsJson(_productService.ListProductItemsAll());
        }
        
        [Route("product/{seoFriendlyId}")]
        public ActionResult Details(string seoFriendlyId)
        {
            var product = _productService.Find(seoFriendlyId);
            if (product == null)
            {
                return HttpNotFound();
            }

            LoadUploadedFiles(product);
            return View(product);
        }
        
        public override string GetObjectName()
        {
            return typeof(NewsItem).Name;
        }
    }
}
