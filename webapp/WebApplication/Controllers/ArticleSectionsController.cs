using K9.Base.WebApplication.Controllers;
using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Constants;
using K9.WebApplication.Helpers;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
	public class ArticleSectionsController : BaseController<ArticleSection>
	{
		public ArticleSectionsController(IControllerPackage<ArticleSection> controllerPackage) : base(controllerPackage)
		{
		    RecordBeforeCreate += ArticleSectionsController_RecordBeforeCreate;
		}
        
	    void ArticleSectionsController_RecordBeforeCreate(object sender, CrudEventArgs e)
	    {
	        var articleSection = e.Item as ArticleSection;
	        articleSection.ArticleId = SessionHelper.GetIntValue(SessionConstants.ArticleId);
	    }
	}
}
