using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    public class ArticleController : BaseRootController
    {
        private readonly IRepository<Article> _articlesRepository;

        public ArticleController(IRepository<Article> articlesRepository, IServicePackage servicePackage)
            : base(servicePackage)
        {
            _articlesRepository = articlesRepository;
        }

        [Route("article/all")]
        public ActionResult Index()
        {
            return View(_articlesRepository.GetQuery($"SELECT * FROM [{nameof(Article)}] ORDER BY [{nameof(Article.CreatedOn)}] DESC").ToList());
        }

        [Route("article/{seoFriendlyId}")]
        public ActionResult Details(string seoFriendlyId)
        {
            var article = _articlesRepository.Find(e => e.SeoFriendlyId == seoFriendlyId).FirstOrDefault();
            if (article == null)
            {
                return HttpNotFound();
            }
            LoadUploadedFiles(article);
            return View(article);
        }

        [ChildActionOnly]
        public PartialViewResult ArticlesSummary()
        {
            return PartialView("_ArticlesSummary", _articlesRepository.GetQuery($"SELECT TOP 10 * FROM [{nameof(Article)}] ORDER BY [{nameof(Article.PublishedOn)}]").ToList());
        }

        public override string GetObjectName()
        {
            return typeof(NewsItem).Name;
        }
    }
}
