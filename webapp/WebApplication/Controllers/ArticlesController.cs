using K9.Base.WebApplication.EventArgs;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.WebApplication.Extensions;
using K9.WebApplication.Helpers;
using K9.WebApplication.Packages;
using Microsoft.Ajax.Utilities;
using System;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
	public class ArticlesController : BaseRootController<Article>
	{
		public ArticlesController(IControllerPackage<Article> controllerPackage, IControllerPackage pureControllerPackage) : base(controllerPackage, pureControllerPackage)
		{
		    RecordBeforeCreate += ArticlesController_RecordBeforeCreate;
            RecordBeforeCreated += ArticlesController_RecordBeforeCreated;
            RecordBeforeUpdated += ArticlesController_RecordBeforeUpdated;
		}

        private void ArticlesController_RecordBeforeUpdated(object sender, CrudEventArgs e)
        {
            UpdateNameAndSeo(e);
            
        }

        private void ArticlesController_RecordBeforeCreated(object sender, CrudEventArgs e)
        {
            UpdateNameAndSeo(e);
        }

	    void ArticlesController_RecordBeforeCreate(object sender, CrudEventArgs e)
	    {
	        var article = e.Item as Article;
	        article.UserId = Current.UserId;
	        article.Name = Guid.NewGuid().ToString();
	        article.PublishedBy = WebSecurity.CurrentUserName;
	        article.PublishedOn = DateTime.Now;
	    }

	    private static void UpdateNameAndSeo(CrudEventArgs e)
	    {
	        var article = e.Item as Article;
	        article.SeoFriendlyId = article.Subject.ToSeoFriendlyString();
	        
	        if (article.Name.IsNullOrWhiteSpace())
	        {
	            article.Name = article.Subject;
	        }
	    }
	}
}
